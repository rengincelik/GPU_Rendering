
Shader "Custom/GrassProcedural_URP"
{
    Properties
    {
        _Width ("Grass Width", Float) = 0.1
        _Height ("Grass Height", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            Cull Off
            ZWrite On
            ZTest LEqual
            Lighting Off
            Blend Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 5.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            StructuredBuffer<float4> _PosScaleBuffer;
            StructuredBuffer<float4> _ColorBuffer;

            float _Width;
            float _Height;

            struct appdata
            {
                uint vertexID : SV_VertexID;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;

                uint bladeIndex = v.vertexID / 6;
                uint cornerIndex = v.vertexID % 6;

                float4 blade = _PosScaleBuffer[bladeIndex];
                float4 bladeColor = _ColorBuffer[bladeIndex];

                float halfW = _Width * 0.5 * blade.w;
                float h = _Height * blade.w;

                float3 localPos;
                if (cornerIndex == 0) localPos = float3(-halfW, 0, 0);
                if (cornerIndex == 1) localPos = float3(-halfW, h, 0);
                if (cornerIndex == 2) localPos = float3( halfW, h, 0);
                if (cornerIndex == 3) localPos = float3(-halfW, 0, 0);
                if (cornerIndex == 4) localPos = float3( halfW, h, 0);
                if (cornerIndex == 5) localPos = float3( halfW, 0, 0);

                float3 worldPos = float3(blade.x, blade.y, blade.z) + localPos;

                o.pos = TransformWorldToHClip(worldPos);
                o.color = bladeColor;

                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                return i.color;
            }

            ENDHLSL
        }
    }
}
