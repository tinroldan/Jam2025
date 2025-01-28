Shader "Custom/WaterShaderWithInteraction"
{
    Properties
    {
        _Color("Water Color", Color) = (0, 0.5, 1, 0.5)
        _WaveSpeed("Wave Speed", Float) = 0.2
        _WaveHeight("Wave Height", Float) = 0.1
        _WaveFrequency("Wave Frequency", Float) = 1.0
        _RefractionStrength("Refraction Strength", Float) = 0.1
        _ReflectionTex("Reflection Texture", 2D) = "white" { }
        _ObjectPosition("Object Position", Vector) = (0, 0, 0, 0) // Pasar la posición del objeto al shader
    }

        SubShader
        {
            Tags { "Queue" = "Overlay" "RenderType" = "Opaque" }

            Pass
            {
                Name "WaterPass"
                Tags { "LightMode" = "UniversalForward" }

    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #pragma multi_compile_fog

    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

    struct Attributes
    {
        float4 position : POSITION;
        float2 uv : TEXCOORD0;
    };

    struct Varyings
    {
        float2 uv : TEXCOORD0;
        float4 position : SV_POSITION;
    };

    cbuffer WaterProperties : register(b0)
    {
        float4 _Color;
        float _WaveSpeed;
        float _WaveHeight;
        float _WaveFrequency;
        float _RefractionStrength;
        float4 _ObjectPosition; // Posición del objeto
    };

    Texture2D _ReflectionTex : register(t0);
    SamplerState sampler_ReflectionTex : register(s0);

    Varyings vert(Attributes v)
    {
        Varyings o;
        o.uv = v.uv;

        // Calcular la distancia desde la posición del objeto hasta la posición en la malla
        float2 objectDist = v.position.xy - _ObjectPosition.xy;

        // Simular ondas de impacto alrededor del objeto
        float wave = sin(length(objectDist) * _WaveFrequency + unity_Time.y * _WaveSpeed) * _WaveHeight;
        v.position.y += wave;

        o.position = TransformObjectToHClip(v.position.xyz);
        return o;
    }

    float4 frag(Varyings i) : SV_Target
    {
        // Color base del agua
        float4 waterColor = _Color;

        // Simulando la refracción del agua
        float2 refractUV = i.uv + sin(i.uv.x * _WaveFrequency + unity_Time.y * _WaveSpeed) * _RefractionStrength;
        float4 reflection = _ReflectionTex.Sample(sampler_ReflectionTex, refractUV);

        // Combinar el color base del agua con la textura de reflejo
        return lerp(waterColor, reflection, 0.5);
    }
    ENDCG
            }
        }

            FallBack "Universal Forward"
}
