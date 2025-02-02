Shader "Unlit/CpatureShader_URP"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags {  "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Tags { "LightMode"="UniversalForward" }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionNDC : TEXCOORD1;
                float3 positionWS : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            Varyings vert (Attributes v)
            {
                Varyings o = (Varyings)0;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
                o.vertex = vertexInput.positionCS;
                o.positionNDC = vertexInput.positionNDC;
                o.positionWS = vertexInput.positionWS;
                o.uv = v.uv;
                return o;
            }
            half remap(half value, half low1, half high1, half low2, half high2)
            {
                return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
            }
            half4 frag (Varyings i) : SV_Target
            {
                float2 screenUV = i.positionNDC.xy/i.positionNDC.w;
                half2 modelUV = i.uv;
                half cameraDistance = remap(length(i.positionWS - _WorldSpaceCameraPos),0,2,0,1 );
                half2 finalUV = lerp(screenUV, modelUV, cameraDistance);
                half4 col = tex2D(_MainTex, finalUV);
                return col;
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
