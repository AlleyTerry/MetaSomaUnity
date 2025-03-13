Shader "Unlit/TestingNormalMap"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _LightDir ("Light Direction", Vector) = (0, 0, 1, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        //LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            //#pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _BumpMap;
            float4 _LightDir;
            
            //float4 _MainTex_ST;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // unpack normal map
                fixed3 normal = UnpackNormal(tex2D(_BumpMap, i.uv));
                // compute lighting
                fixed3 lightDir = normalize(_LightDir.xyz); // normalize light direction

                float diff = max(0, dot(normal, lightDir)); // diffuse lighting

                fixed3 color = tex2D(_MainTex, i.uv).rgb * diff; // apply normal map lighting
                return fixed4(color, 1.0f);
            }
            ENDCG
        }
    }
}
