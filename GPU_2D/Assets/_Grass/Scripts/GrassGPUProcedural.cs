
using UnityEngine;
using UnityEngine.Rendering;

public class GrassGPUProcedural : MonoBehaviour
{
    [Header("References")]
    public ComputeShader computeShader;
    public Material grassMaterial;

    [Header("Grass Parameters")]
    public float width = 0.1f;
    public float height = 1f;
    [Range(0f, 1f)] public float positionNoise = 0.5f;
    [Range(0f, 1f)] public float scaleNoise = 0.3f;
    public Gradient colorGradient;

    [Header("Spawn Area")]
    public Vector2 areaSize = new Vector2(100, 100);
    [Range(1, 1000)] public int grassCount = 1000;

    [Header("Debug")]
    public bool enableDebug = true;

    private ComputeBuffer posScaleBuffer;
    private ComputeBuffer colorBuffer;
    private RenderTexture gradientRT;
    private bool isInitialized = false;

    void Start()
    {
        InitializeGrass();
    }

    void InitializeGrass()
    {
        Cleanup();

        grassCount = Mathf.Max(1, grassCount);

        // Create buffers
        posScaleBuffer = new ComputeBuffer(grassCount, sizeof(float) * 4);
        colorBuffer = new ComputeBuffer(grassCount, sizeof(float) * 4);

        // Create gradient texture
        CreateGradientTexture();

        // Setup compute shader
        int kernelHandle = computeShader.FindKernel("CSMain");

        // Set parameters
        computeShader.SetInt("grassCount", grassCount);
        computeShader.SetVector("areaSize", areaSize);
        computeShader.SetFloat("width", width);
        computeShader.SetFloat("height", height);
        computeShader.SetFloat("positionNoise", positionNoise);
        computeShader.SetFloat("scaleNoise", scaleNoise);
        computeShader.SetBuffer(kernelHandle, "PosScaleBuffer", posScaleBuffer);
        computeShader.SetBuffer(kernelHandle, "ColorBuffer", colorBuffer);
        computeShader.SetTexture(kernelHandle, "gradientTex", gradientRT);

        // Dispatch
        uint threadGroupSize;
        computeShader.GetKernelThreadGroupSizes(kernelHandle, out threadGroupSize, out _, out _);
        int threadGroups = Mathf.CeilToInt((float)grassCount / threadGroupSize);
        computeShader.Dispatch(kernelHandle, threadGroups, 1, 1);

        // DEBUG: Read back data to verify it's working
        if (enableDebug)
        {
            Vector4[] debugData = new Vector4[Mathf.Min(grassCount, 10)];
            posScaleBuffer.GetData(debugData);
            Debug.Log("First 10 grass positions:");
            for (int i = 0; i < debugData.Length; i++)
            {
                Debug.Log($"Grass {i}: Pos({debugData[i].x}, {debugData[i].y}, {debugData[i].z}) Scale:{debugData[i].w}");
            }
        }

        // Setup material - CRITICAL: Enable instancing
        grassMaterial.enableInstancing = true;
        grassMaterial.SetBuffer("_PosScaleBuffer", posScaleBuffer);
        grassMaterial.SetBuffer("_ColorBuffer", colorBuffer);
        grassMaterial.SetFloat("_Width", width);
        grassMaterial.SetFloat("_Height", height);

        // Set material properties for the shader
        grassMaterial.SetVector("_AreaSize", areaSize);
        grassMaterial.SetInt("_GrassCount", grassCount);

        isInitialized = true;
        Debug.Log($"Grass system initialized with {grassCount} blades");
    }

    void CreateGradientTexture()
    {
        if (gradientRT != null) gradientRT.Release();

        gradientRT = new RenderTexture(256, 1, 0, RenderTextureFormat.ARGB32)
        {
            enableRandomWrite = true,
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp
        };
        gradientRT.Create();

        Texture2D gradTexCPU = new Texture2D(256, 1, TextureFormat.RGBA32, false);
        for (int i = 0; i < 256; i++)
            gradTexCPU.SetPixel(i, 0, colorGradient.Evaluate(i / 255f));
        gradTexCPU.Apply();

        Graphics.Blit(gradTexCPU, gradientRT);
        DestroyImmediate(gradTexCPU);
    }

    void Update()
    {
        if (!isInitialized) return;

        // Use CommandBuffer for more control
        Graphics.DrawProcedural(
            grassMaterial,
            new Bounds(transform.position, new Vector3(areaSize.x, height * 2, areaSize.y)),
            MeshTopology.Triangles,
            6, // 6 vertices per quad (2 triangles)
            grassCount,
            camera: null,
            properties: null,
            ShadowCastingMode.On,
            true,
            gameObject.layer
        );
    }

    void OnDestroy()
    {
        Cleanup();
    }

    void Cleanup()
    {
        posScaleBuffer?.Release();
        colorBuffer?.Release();
        if (gradientRT != null) gradientRT.Release();
        isInitialized = false;
    }

    void OnValidate()
    {
        if (isInitialized && Application.isPlaying)
        {
            InitializeGrass();
        }
    }
}
