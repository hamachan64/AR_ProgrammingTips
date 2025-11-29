Shader "Custom/SampleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AmbientLight ("Ambient Light", Color) = (0.5,0.5,0.5,1) // <-　追加
    }
    SubShader
    {
        Tags {
            "RenderType"="Opaque"
            "RenderPipeline"="UniversalPipeline"
        }
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl" // <-　追加

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL; // <-　追加
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float fogFactor: TEXCOORD1;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL; // <-　追加
            };



            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            CBUFFER_END

            float3 _AmbientLight; // <-　追加

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.fogFactor = ComputeFogFactor(o.vertex.z);
                o.normal = TransformObjectToWorldNormal(v.normal); // <-　追加
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                ////////////////////////
                // ▼ここから

                // ライト情報を取得
                Light light = GetMainLight();

                // ピクセルの法線とライトの方向の内積を計算する
                float t = dot(i.normal, light.direction);
                // 内積の値を0以上の値にする
                t = max(0, t);
                // 拡散反射光を計算する
                float3 diffuseLight = light.color * t;

                // 拡散反射光と環境光を足し算して、最終的な光を求める
                col.rgb *= diffuseLight + _AmbientLight;
                // ▲ここまで追加
                ////////////////////////

                // apply fog
                col.rgb = MixFog(col.rgb, i.fogFactor);
                return col;
            }
            ENDHLSL
        }
    }
}
