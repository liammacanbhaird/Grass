Shader "Unlit/TestShader"
{
    Properties
    {
        //_MainTex ("Texture", 2D) = "white" {}
        _PhaseOffset ("Offset", float) = 0
        _Scale ("Scale", float) = 1
        _Frequency("Frequency", float) = 1
    }
    SubShader
    {
        Tags {
            "RenderType"="Opaque"
            "Qeue" = "Opaque" 
        }
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            #define TAU 6.28318530718

            float _PhaseOffset;
            float _Scale;
            float _Frequency;

            struct MeshData
            {
                float4 vertex : POSITION;
                float3 normals : NORMAL;
                float2 uv0 : TEXCOORD0;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float3 normal : TEXTCOORD0;
                float2 uv : TEXCOORD1;
            };


            Interpolators vert (MeshData v)
            {
                Interpolators o;

                //float4 vert = v.vertex + _Scale * cos(v.uv0.x * _Frequency + _PhaseOffset);
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normals);
                o.uv = v.uv0;
                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                float4 o = float4(_PhaseOffset,1,0,1);
                return float4(i.uv.yyy, 1);
            }
            ENDCG
        }
    }
}
