Shader "Unlit/BillboardGrass" {
  Properties{
      _MainTex("Texture", 2D)="white" {}
  }

    SubShader{
        Cull Off
        Zwrite On

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct VertexData {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(VertexData v) {
                v2f o;
                o.vertex=UnityObjectToClipPos(v.vertex);
                o.uv=TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 col=tex2D(_MainTex, i.uv);
                clip(-(0.6-col.a));
                return col;
            }

            ENDCG
        }
  }
}
