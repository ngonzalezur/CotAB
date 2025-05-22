Shader "Custom/CRT_URP"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScanlineIntensity ("Scanline Intensity", Range(0,1)) = 0.3
        _ScanlineFrequency ("Scanline Frequency", Float) = 480.0
        _DistortionAmount ("Distortion Amount", Range(0,0.1)) = 0.03
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 100
        ZTest Always 
        ZWrite Off 
        Cull Off

        Pass
        {
            Name "CRT Effect"
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            
            float _ScanlineIntensity;
            float _ScanlineFrequency;
            float _DistortionAmount;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;
                
                // Aplicar curvatura (distorsión)
                float2 centeredUV = uv * 2.0 - 1.0; // De 0-1 a -1 a 1
                float2 offset = centeredUV * centeredUV * _DistortionAmount;
                offset = offset * (centeredUV / max(abs(centeredUV), 0.0001)); // Evitar división por cero
                uv = uv + offset;
                
                // Limitar UV para no leer fuera
                uv = saturate(uv);
                
                // Muestra textura
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                
                // Aplicar scanlines
                float scanline = sin(uv.y * _ScanlineFrequency * 3.14159);
                float scanlineEffect = lerp(1.0, 1.0 - _ScanlineIntensity, scanline * scanline);
                col.rgb *= scanlineEffect;
                
                return col;
            }
            ENDHLSL
        }
    }
}
