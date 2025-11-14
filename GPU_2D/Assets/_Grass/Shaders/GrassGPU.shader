
Shader "Custom/GrassGPU"
{
    Properties { }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" "RenderType"="Opaque" }

        Pass
        {
            Name "ForwardUnlit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            StructuredBuffer<float4> _InstanceData_PosScale; // xyz = pos, w = scale
            StructuredBuffer<float4> _InstanceData_Color;

            struct Attributes
            {
                float3 positionOS : POSITION;
                uint instanceID  : SV_InstanceID;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float4 color       : COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                float4 posScale = _InstanceData_PosScale[v.instanceID];
                float4 col      = _InstanceData_Color[v.instanceID];

                // float3 pos = v.positionOS * posScale.w + posScale.xyz;
                float3 pos = float3(
                    v.positionOS.x,
                    v.positionOS.y * posScale.w,
                    v.positionOS.z
                ) + posScale.xyz;


                o.positionHCS = TransformObjectToHClip(float4(pos,1));
                o.color = col;

                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                return i.color;
            }
            ENDHLSL
        }
    }
}
