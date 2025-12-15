Shader "Unlit/TestShaderCode"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _SecondTex ("Second Texture", 2D) = "white" {}
        _ThirdTex ("Third Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue"="Transparent"
            // DisableBatching: <None>
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalSpriteUnlitSubTarget"
        }
        Pass
        {
            Name "Sprite Unlit"
            Tags
            {
                "LightMode" = "Universal2D"
            }
            Cull Off
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Declare the first texture and its sampler.
            UNITY_DECLARE_TEX2D(_MainTex);

            // Declare the second and third textures without samplers.
            UNITY_DECLARE_TEX2D_NOSAMPLER(_SecondTex);
            UNITY_DECLARE_TEX2D_NOSAMPLER(_ThirdTex);

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // Sample the first texture using its own sampler.
                half4 color1 = UNITY_SAMPLE_TEX2D(_MainTex, i.uv);

                // Sample the second and third textures using the sampler from the first texture.
                half4 color2 = UNITY_SAMPLE_TEX2D_SAMPLER(_SecondTex, _MainTex, i.uv);
                half4 color3 = UNITY_SAMPLE_TEX2D_SAMPLER(_ThirdTex, _MainTex, i.uv);

                // Return the combined color from all three textures.
                return color1;//(color1 + color2 + color3) / 3.0;
            }
            ENDHLSL
        }
    }

}
