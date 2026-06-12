Shader "Custom/BossShieldShader"
{
    Properties
    {
        _Color ("Cor do Escudo", Color) = (0, 1, 1, 1)
        _Opacity ("Transparencia Base", Range(0, 1)) = 0.2
        _ImpactColor ("Cor do Impacto", Color) = (1, 1, 1, 1)
        _ImpactRadius ("Tamanho do Impacto", Float) = 1.5

        // 4 variáveis para guardar os 4 impactos (X,Y,Z = Posição, W = Intensidade)
        _Impact0 ("Impact 0", Vector) = (0,0,0,0)
        _Impact1 ("Impact 1", Vector) = (0,0,0,0)
        _Impact2 ("Impact 2", Vector) = (0,0,0,0)
        _Impact3 ("Impact 3", Vector) = (0,0,0,0)
    }
    SubShader
    {
        // Configurações para ser transparente e funcionar na URP
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "RenderPipeline" = "UniversalPipeline" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
            };

            // Variáveis ligadas ao Inspector
            float4 _Color;
            float _Opacity;
            float4 _ImpactColor;
            float _ImpactRadius;
            float4 _Impact0, _Impact1, _Impact2, _Impact3;

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz); // Converte para Mundo
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float finalAlpha = _Opacity;
                float3 finalColor = _Color.rgb;

                // Verifica Impacto 0
                if (_Impact0.w > 0) {
                    float dist = distance(input.positionWS, _Impact0.xyz);
                    float wave = saturate(1 - (dist / _ImpactRadius)) * _Impact0.w;
                    finalColor += _ImpactColor.rgb * wave;
                    finalAlpha += wave;
                }
                // Verifica Impacto 1
                if (_Impact1.w > 0) {
                    float dist = distance(input.positionWS, _Impact1.xyz);
                    float wave = saturate(1 - (dist / _ImpactRadius)) * _Impact1.w;
                    finalColor += _ImpactColor.rgb * wave;
                    finalAlpha += wave;
                }
                // Verifica Impacto 2
                if (_Impact2.w > 0) {
                    float dist = distance(input.positionWS, _Impact2.xyz);
                    float wave = saturate(1 - (dist / _ImpactRadius)) * _Impact2.w;
                    finalColor += _ImpactColor.rgb * wave;
                    finalAlpha += wave;
                }
                // Verifica Impacto 3
                if (_Impact3.w > 0) {
                    float dist = distance(input.positionWS, _Impact3.xyz);
                    float wave = saturate(1 - (dist / _ImpactRadius)) * _Impact3.w;
                    finalColor += _ImpactColor.rgb * wave;
                    finalAlpha += wave;
                }

                return half4(finalColor, saturate(finalAlpha));
            }
            ENDHLSL
        }
    }
}