using UnityEngine;

public class GPU_Instancer : MonoBehaviour
{
    public GameObject prefab;
    private Mesh mesh;
    private Material material;
    private ComputeBuffer positionBuffer;

    void Start()
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab atanmadı!");
            return;
        }

        MeshFilter mf = prefab.GetComponent<MeshFilter>();
        MeshRenderer mr = prefab.GetComponent<MeshRenderer>();
        if (mf == null || mr == null)
        {
            Debug.LogError("Prefab MeshFilter ve MeshRenderer içermeli!");
            return;
        }

        mesh = mf.sharedMesh;
        material = mr.sharedMaterial;

        InitBuffer();
    }

    void InitBuffer()
    {
        var data = DataManager.Instance.positions;
        if (data == null || data.Count == 0)
        {
            Debug.LogError("Position listesi boş!");
            return;
        }

        positionBuffer = new ComputeBuffer(data.Count, sizeof(float) * 3);
        positionBuffer.SetData(data);
        material.SetBuffer("_Positions", positionBuffer);

        Debug.Log($"[GPUInstancer] Buffer oluşturuldu. Instance sayısı: {data.Count}");
    }

    void Update()
    {
        if (mesh == null || material == null || positionBuffer == null) return;

        Graphics.DrawMeshInstancedProcedural(
            mesh,
            0,
            material,
            new Bounds(Vector3.zero, Vector3.one * 1000f),
            DataManager.Instance.positions.Count
        );
    }

    void OnDisable()
    {
        if (positionBuffer != null)
        {
            positionBuffer.Release();
            positionBuffer = null;
        }
    }
}
