Shader "Custom/TestCopyShader"
{
    Properties
    {
        [IntRange] _StencilID ("Stencil ID", Range(0,255)) = 0
        [NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
        _PulseSpeed("PulseSpeed", Float) = 1
        _MinMaxHighlight("MinMaxHighlight", Vector) = (1, 2, 0, 0)
        _WaveSize("WaveSize", Range(0, 0.6)) = 0.5
        _Color("Color", Color) = (0.9118466, 0.9103774, 1, 1)
        _SmoothEdge("SmoothEdge", Range(0, 0.2)) = 0.05
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
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
        
        // Render State
        Cull Off
        //Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        Blend Zero One
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        Stencil
            {
                Ref [_StencilID]
                Comp Always
                Pass Replace
            }
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX_NORMAL_OUTPUT
        #define FEATURES_GRAPH_VERTEX_TANGENT_OUTPUT
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITEUNLIT
        #define _FOG_FRAGMENT 1
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/DebugMipmapStreamingMacros.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(ATTRIBUTES_NEED_INSTANCEID)
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float2 NDCPosition;
             float2 PixelPosition;
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
             float4 uv0;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 color : INTERP1;
             float3 positionWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.color.xyzw = input.color;
            output.positionWS.xyz = input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.color = input.color.xyzw;
            output.positionWS = input.positionWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float _PulseSpeed;
        float2 _MinMaxHighlight;
        float _WaveSize;
        float4 _Color;
        float _SmoothEdge;
        UNITY_TEXTURE_STREAMING_DEBUG_VARS;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_CameraSortingLayerTexture);
        SAMPLER(sampler_CameraSortingLayerTexture);
        float4 _CameraSortingLayerTexture_TexelSize;
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        float Unity_SimpleNoise_ValueNoise_Deterministic_float (float2 uv)
        {
            float2 i = floor(uv);
            float2 f = frac(uv);
            f = f * f * (3.0 - 2.0 * f);
            uv = abs(frac(uv) - 0.5);
            float2 c0 = i + float2(0.0, 0.0);
            float2 c1 = i + float2(1.0, 0.0);
            float2 c2 = i + float2(0.0, 1.0);
            float2 c3 = i + float2(1.0, 1.0);
            float r0; Hash_Tchou_2_1_float(c0, r0);
            float r1; Hash_Tchou_2_1_float(c1, r1);
            float r2; Hash_Tchou_2_1_float(c2, r2);
            float r3; Hash_Tchou_2_1_float(c3, r3);
            float bottomOfGrid = lerp(r0, r1, f.x);
            float topOfGrid = lerp(r2, r3, f.x);
            float t = lerp(bottomOfGrid, topOfGrid, f.y);
            return t;
        }
        
        void Unity_SimpleNoise_Deterministic_float(float2 UV, float Scale, out float Out)
        {
            float freq, amp;
            Out = 0.0f;
            freq = pow(2.0, float(0));
            amp = pow(0.5, float(3-0));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
            freq = pow(2.0, float(1));
            amp = pow(0.5, float(3-1));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
            freq = pow(2.0, float(2));
            amp = pow(0.5, float(3-2));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A + B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Distance_float2(float2 A, float2 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            UnityTexture2D _Property_43c38452057644c7befab43c635f6778_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float _TextureSize_6296f00037a549648168c3e259bbf177_Width_0_Float = _Property_43c38452057644c7befab43c635f6778_Out_0_Texture2D.texelSize.z;
            float _TextureSize_6296f00037a549648168c3e259bbf177_Height_2_Float = _Property_43c38452057644c7befab43c635f6778_Out_0_Texture2D.texelSize.w;
            float _TextureSize_6296f00037a549648168c3e259bbf177_TexelWidth_3_Float = _Property_43c38452057644c7befab43c635f6778_Out_0_Texture2D.texelSize.x;
            float _TextureSize_6296f00037a549648168c3e259bbf177_TexelHeight_4_Float = _Property_43c38452057644c7befab43c635f6778_Out_0_Texture2D.texelSize.y;
            float3 _Vector3_d3b74d1441ca41799a392e0b16b7cf2a_Out_0_Vector3 = float3(_TextureSize_6296f00037a549648168c3e259bbf177_Width_0_Float, _TextureSize_6296f00037a549648168c3e259bbf177_Height_2_Float, float(1));
            float3 _Multiply_f0f4dc336c734fc2a49491b9a87ef62f_Out_2_Vector3;
            Unity_Multiply_float3_float3(float3(0.0008, 0.0008, 0.0008), _Vector3_d3b74d1441ca41799a392e0b16b7cf2a_Out_0_Vector3, _Multiply_f0f4dc336c734fc2a49491b9a87ef62f_Out_2_Vector3);
            float _SimpleNoise_348a3914a33843a994913a5d7f184aae_Out_2_Float;
            Unity_SimpleNoise_Deterministic_float(IN.uv0.xy, float(20), _SimpleNoise_348a3914a33843a994913a5d7f184aae_Out_2_Float);
            float _Remap_3e5cc8bd01c643c49ef76e62ecea9b38_Out_3_Float;
            Unity_Remap_float(_SimpleNoise_348a3914a33843a994913a5d7f184aae_Out_2_Float, float2 (-1, 1), float2 (-1, 1), _Remap_3e5cc8bd01c643c49ef76e62ecea9b38_Out_3_Float);
            float3 _Multiply_83ae6eebe550408cabe6124d4be7871b_Out_2_Vector3;
            Unity_Multiply_float3_float3(_Multiply_f0f4dc336c734fc2a49491b9a87ef62f_Out_2_Vector3, (_Remap_3e5cc8bd01c643c49ef76e62ecea9b38_Out_3_Float.xxx), _Multiply_83ae6eebe550408cabe6124d4be7871b_Out_2_Vector3);
            float3 _Add_cb1e2b9d56d442578cb6b0a385eba233_Out_2_Vector3;
            Unity_Add_float3(IN.ObjectSpacePosition, _Multiply_83ae6eebe550408cabe6124d4be7871b_Out_2_Vector3, _Add_cb1e2b9d56d442578cb6b0a385eba233_Out_2_Vector3);
            description.Position = _Add_cb1e2b9d56d442578cb6b0a385eba233_Out_2_Vector3;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_8dae12f47bf34cf893202e2b3827d1e8_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_CameraSortingLayerTexture);
            float4 _ScreenPosition_8812d76f920c48178bfa5ad28781258d_Out_0_Vector4 = float4(IN.NDCPosition.xy, 0, 0);
            float4 _SampleTexture2D_9842fdb51ef849fe80e396e9aec03e50_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_8dae12f47bf34cf893202e2b3827d1e8_Out_0_Texture2D.tex, _Property_8dae12f47bf34cf893202e2b3827d1e8_Out_0_Texture2D.samplerstate, _Property_8dae12f47bf34cf893202e2b3827d1e8_Out_0_Texture2D.GetTransformedUV((_ScreenPosition_8812d76f920c48178bfa5ad28781258d_Out_0_Vector4.xy)) );
            float _SampleTexture2D_9842fdb51ef849fe80e396e9aec03e50_R_4_Float = _SampleTexture2D_9842fdb51ef849fe80e396e9aec03e50_RGBA_0_Vector4.r;
            float _SampleTexture2D_9842fdb51ef849fe80e396e9aec03e50_G_5_Float = _SampleTexture2D_9842fdb51ef849fe80e396e9aec03e50_RGBA_0_Vector4.g;
            float _SampleTexture2D_9842fdb51ef849fe80e396e9aec03e50_B_6_Float = _SampleTexture2D_9842fdb51ef849fe80e396e9aec03e50_RGBA_0_Vector4.b;
            float _SampleTexture2D_9842fdb51ef849fe80e396e9aec03e50_A_7_Float = _SampleTexture2D_9842fdb51ef849fe80e396e9aec03e50_RGBA_0_Vector4.a;
            float4 _Property_c1e1471ab3344e36a423f48911e89fb9_Out_0_Vector4 = _Color;
            float _Property_40f877571ff4439a9a92ea0a3b5d9823_Out_0_Float = _PulseSpeed;
            float _Multiply_539f0df73ad440f0a3f8747551f70836_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_40f877571ff4439a9a92ea0a3b5d9823_Out_0_Float, _Multiply_539f0df73ad440f0a3f8747551f70836_Out_2_Float);
            float2 _TilingAndOffset_91338700237145828bf3b7fb6c2490c4_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_Multiply_539f0df73ad440f0a3f8747551f70836_Out_2_Float.xx), _TilingAndOffset_91338700237145828bf3b7fb6c2490c4_Out_3_Vector2);
            float _SimpleNoise_bb64167a340641da8d9c4794d228b332_Out_2_Float;
            Unity_SimpleNoise_Deterministic_float(_TilingAndOffset_91338700237145828bf3b7fb6c2490c4_Out_3_Vector2, float(10), _SimpleNoise_bb64167a340641da8d9c4794d228b332_Out_2_Float);
            float2 _Property_b3062eb0c49b41179a8f46ea657adcc0_Out_0_Vector2 = _MinMaxHighlight;
            float _Remap_306e090b0d67498daf29e483a63b65c9_Out_3_Float;
            Unity_Remap_float(_SimpleNoise_bb64167a340641da8d9c4794d228b332_Out_2_Float, float2 (0, 1), _Property_b3062eb0c49b41179a8f46ea657adcc0_Out_0_Vector2, _Remap_306e090b0d67498daf29e483a63b65c9_Out_3_Float);
            UnityTexture2D _Property_147bf67514134240a8978048642218bd_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_3b790c1044a34a5fa441a26b85e03f0d_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_147bf67514134240a8978048642218bd_Out_0_Texture2D.tex, _Property_147bf67514134240a8978048642218bd_Out_0_Texture2D.samplerstate, _Property_147bf67514134240a8978048642218bd_Out_0_Texture2D.GetTransformedUV(IN.uv0.xy) );
            float _SampleTexture2D_3b790c1044a34a5fa441a26b85e03f0d_R_4_Float = _SampleTexture2D_3b790c1044a34a5fa441a26b85e03f0d_RGBA_0_Vector4.r;
            float _SampleTexture2D_3b790c1044a34a5fa441a26b85e03f0d_G_5_Float = _SampleTexture2D_3b790c1044a34a5fa441a26b85e03f0d_RGBA_0_Vector4.g;
            float _SampleTexture2D_3b790c1044a34a5fa441a26b85e03f0d_B_6_Float = _SampleTexture2D_3b790c1044a34a5fa441a26b85e03f0d_RGBA_0_Vector4.b;
            float _SampleTexture2D_3b790c1044a34a5fa441a26b85e03f0d_A_7_Float = _SampleTexture2D_3b790c1044a34a5fa441a26b85e03f0d_RGBA_0_Vector4.a;
            float4 _Multiply_37d247c67df446e881c32a458bb98bad_Out_2_Vector4;
            Unity_Multiply_float4_float4((_Remap_306e090b0d67498daf29e483a63b65c9_Out_3_Float.xxxx), _SampleTexture2D_3b790c1044a34a5fa441a26b85e03f0d_RGBA_0_Vector4, _Multiply_37d247c67df446e881c32a458bb98bad_Out_2_Vector4);
            float4 _Multiply_068eab5a8cab4e3ebd34c63ba5a2d9c6_Out_2_Vector4;
            Unity_Multiply_float4_float4(_Property_c1e1471ab3344e36a423f48911e89fb9_Out_0_Vector4, _Multiply_37d247c67df446e881c32a458bb98bad_Out_2_Vector4, _Multiply_068eab5a8cab4e3ebd34c63ba5a2d9c6_Out_2_Vector4);
            float4 _Multiply_0475505356d24598aeec07482d0acc22_Out_2_Vector4;
            Unity_Multiply_float4_float4(_SampleTexture2D_9842fdb51ef849fe80e396e9aec03e50_RGBA_0_Vector4, _Multiply_068eab5a8cab4e3ebd34c63ba5a2d9c6_Out_2_Vector4, _Multiply_0475505356d24598aeec07482d0acc22_Out_2_Vector4);
            float _Property_5c08bf143c5244eba2331f381682df5f_Out_0_Float = _WaveSize;
            float _Subtract_5410d776f36f413e8cfcf6fe7cca590b_Out_2_Float;
            Unity_Subtract_float(float(0.6), _Property_5c08bf143c5244eba2331f381682df5f_Out_0_Float, _Subtract_5410d776f36f413e8cfcf6fe7cca590b_Out_2_Float);
            float _Property_74cea885cd8140b8aa931342595c6f8b_Out_0_Float = _SmoothEdge;
            float _Subtract_9bc253d679574df3af14884d8a1da1ac_Out_2_Float;
            Unity_Subtract_float(_Subtract_5410d776f36f413e8cfcf6fe7cca590b_Out_2_Float, _Property_74cea885cd8140b8aa931342595c6f8b_Out_0_Float, _Subtract_9bc253d679574df3af14884d8a1da1ac_Out_2_Float);
            float4 _UV_00bd6497fdad45788ff584f8a36c35c6_Out_0_Vector4 = IN.uv0;
            float2 _Vector2_1a977afd960540eeb69aa68b56dae506_Out_0_Vector2 = float2(float(0.5), float(0.5));
            float _Distance_3e7a23d4689c447daa57611afba70214_Out_2_Float;
            Unity_Distance_float2((_UV_00bd6497fdad45788ff584f8a36c35c6_Out_0_Vector4.xy), _Vector2_1a977afd960540eeb69aa68b56dae506_Out_0_Vector2, _Distance_3e7a23d4689c447daa57611afba70214_Out_2_Float);
            float _Smoothstep_968f3951c8f34bc19c268b73fbc85026_Out_3_Float;
            Unity_Smoothstep_float(_Subtract_9bc253d679574df3af14884d8a1da1ac_Out_2_Float, _Subtract_5410d776f36f413e8cfcf6fe7cca590b_Out_2_Float, _Distance_3e7a23d4689c447daa57611afba70214_Out_2_Float, _Smoothstep_968f3951c8f34bc19c268b73fbc85026_Out_3_Float);
            float _OneMinus_441adcef5d194f9d8c7e9e02ea2a0507_Out_1_Float;
            Unity_OneMinus_float(_Smoothstep_968f3951c8f34bc19c268b73fbc85026_Out_3_Float, _OneMinus_441adcef5d194f9d8c7e9e02ea2a0507_Out_1_Float);
            surface.BaseColor = (_Multiply_0475505356d24598aeec07482d0acc22_Out_2_Vector4.xyz);
            surface.Alpha = _OneMinus_441adcef5d194f9d8c7e9e02ea2a0507_Out_1_Float;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
            output.uv0 =                                        input.uv0;
        #if UNITY_ANY_INSTANCING_ENABLED
        #else // TODO: XR support for procedural instancing because in this case UNITY_ANY_INSTANCING_ENABLED is not defined and instanceID is incorrect.
        #endif
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x < 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
            #else
            output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x > 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
            #endif
        
            output.NDCPosition = output.PixelPosition.xy / _ScaledScreenParams.xy;
            output.NDCPosition.y = 1.0f - output.NDCPosition.y;
        
            output.uv0 = input.texCoord0;
        #if UNITY_ANY_INSTANCING_ENABLED
        #else // TODO: XR support for procedural instancing because in this case UNITY_ANY_INSTANCING_ENABLED is not defined and instanceID is incorrect.
        #endif
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteUnlitPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "SceneSelectionPass"
            Tags
            {
                "LightMode" = "SceneSelectionPass"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX_NORMAL_OUTPUT
        #define FEATURES_GRAPH_VERTEX_TANGENT_OUTPUT
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENESELECTIONPASS 1
        
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/DebugMipmapStreamingMacros.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(ATTRIBUTES_NEED_INSTANCEID)
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
             float4 uv0;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float _PulseSpeed;
        float2 _MinMaxHighlight;
        float _WaveSize;
        float4 _Color;
        float _SmoothEdge;
        UNITY_TEXTURE_STREAMING_DEBUG_VARS;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_CameraSortingLayerTexture);
        SAMPLER(sampler_CameraSortingLayerTexture);
        float4 _CameraSortingLayerTexture_TexelSize;
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        float Unity_SimpleNoise_ValueNoise_Deterministic_float (float2 uv)
        {
            float2 i = floor(uv);
            float2 f = frac(uv);
            f = f * f * (3.0 - 2.0 * f);
            uv = abs(frac(uv) - 0.5);
            float2 c0 = i + float2(0.0, 0.0);
            float2 c1 = i + float2(1.0, 0.0);
            float2 c2 = i + float2(0.0, 1.0);
            float2 c3 = i + float2(1.0, 1.0);
            float r0; Hash_Tchou_2_1_float(c0, r0);
            float r1; Hash_Tchou_2_1_float(c1, r1);
            float r2; Hash_Tchou_2_1_float(c2, r2);
            float r3; Hash_Tchou_2_1_float(c3, r3);
            float bottomOfGrid = lerp(r0, r1, f.x);
            float topOfGrid = lerp(r2, r3, f.x);
            float t = lerp(bottomOfGrid, topOfGrid, f.y);
            return t;
        }
        
        void Unity_SimpleNoise_Deterministic_float(float2 UV, float Scale, out float Out)
        {
            float freq, amp;
            Out = 0.0f;
            freq = pow(2.0, float(0));
            amp = pow(0.5, float(3-0));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
            freq = pow(2.0, float(1));
            amp = pow(0.5, float(3-1));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
            freq = pow(2.0, float(2));
            amp = pow(0.5, float(3-2));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A + B;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Distance_float2(float2 A, float2 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            UnityTexture2D _Property_43c38452057644c7befab43c635f6778_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float _TextureSize_6296f00037a549648168c3e259bbf177_Width_0_Float = _Property_43c38452057644c7befab43c635f6778_Out_0_Texture2D.texelSize.z;
            float _TextureSize_6296f00037a549648168c3e259bbf177_Height_2_Float = _Property_43c38452057644c7befab43c635f6778_Out_0_Texture2D.texelSize.w;
            float _TextureSize_6296f00037a549648168c3e259bbf177_TexelWidth_3_Float = _Property_43c38452057644c7befab43c635f6778_Out_0_Texture2D.texelSize.x;
            float _TextureSize_6296f00037a549648168c3e259bbf177_TexelHeight_4_Float = _Property_43c38452057644c7befab43c635f6778_Out_0_Texture2D.texelSize.y;
            float3 _Vector3_d3b74d1441ca41799a392e0b16b7cf2a_Out_0_Vector3 = float3(_TextureSize_6296f00037a549648168c3e259bbf177_Width_0_Float, _TextureSize_6296f00037a549648168c3e259bbf177_Height_2_Float, float(1));
            float3 _Multiply_f0f4dc336c734fc2a49491b9a87ef62f_Out_2_Vector3;
            Unity_Multiply_float3_float3(float3(0.0008, 0.0008, 0.0008), _Vector3_d3b74d1441ca41799a392e0b16b7cf2a_Out_0_Vector3, _Multiply_f0f4dc336c734fc2a49491b9a87ef62f_Out_2_Vector3);
            float _SimpleNoise_348a3914a33843a994913a5d7f184aae_Out_2_Float;
            Unity_SimpleNoise_Deterministic_float(IN.uv0.xy, float(20), _SimpleNoise_348a3914a33843a994913a5d7f184aae_Out_2_Float);
            float _Remap_3e5cc8bd01c643c49ef76e62ecea9b38_Out_3_Float;
            Unity_Remap_float(_SimpleNoise_348a3914a33843a994913a5d7f184aae_Out_2_Float, float2 (-1, 1), float2 (-1, 1), _Remap_3e5cc8bd01c643c49ef76e62ecea9b38_Out_3_Float);
            float3 _Multiply_83ae6eebe550408cabe6124d4be7871b_Out_2_Vector3;
            Unity_Multiply_float3_float3(_Multiply_f0f4dc336c734fc2a49491b9a87ef62f_Out_2_Vector3, (_Remap_3e5cc8bd01c643c49ef76e62ecea9b38_Out_3_Float.xxx), _Multiply_83ae6eebe550408cabe6124d4be7871b_Out_2_Vector3);
            float3 _Add_cb1e2b9d56d442578cb6b0a385eba233_Out_2_Vector3;
            Unity_Add_float3(IN.ObjectSpacePosition, _Multiply_83ae6eebe550408cabe6124d4be7871b_Out_2_Vector3, _Add_cb1e2b9d56d442578cb6b0a385eba233_Out_2_Vector3);
            description.Position = _Add_cb1e2b9d56d442578cb6b0a385eba233_Out_2_Vector3;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Property_5c08bf143c5244eba2331f381682df5f_Out_0_Float = _WaveSize;
            float _Subtract_5410d776f36f413e8cfcf6fe7cca590b_Out_2_Float;
            Unity_Subtract_float(float(0.6), _Property_5c08bf143c5244eba2331f381682df5f_Out_0_Float, _Subtract_5410d776f36f413e8cfcf6fe7cca590b_Out_2_Float);
            float _Property_74cea885cd8140b8aa931342595c6f8b_Out_0_Float = _SmoothEdge;
            float _Subtract_9bc253d679574df3af14884d8a1da1ac_Out_2_Float;
            Unity_Subtract_float(_Subtract_5410d776f36f413e8cfcf6fe7cca590b_Out_2_Float, _Property_74cea885cd8140b8aa931342595c6f8b_Out_0_Float, _Subtract_9bc253d679574df3af14884d8a1da1ac_Out_2_Float);
            float4 _UV_00bd6497fdad45788ff584f8a36c35c6_Out_0_Vector4 = IN.uv0;
            float2 _Vector2_1a977afd960540eeb69aa68b56dae506_Out_0_Vector2 = float2(float(0.5), float(0.5));
            float _Distance_3e7a23d4689c447daa57611afba70214_Out_2_Float;
            Unity_Distance_float2((_UV_00bd6497fdad45788ff584f8a36c35c6_Out_0_Vector4.xy), _Vector2_1a977afd960540eeb69aa68b56dae506_Out_0_Vector2, _Distance_3e7a23d4689c447daa57611afba70214_Out_2_Float);
            float _Smoothstep_968f3951c8f34bc19c268b73fbc85026_Out_3_Float;
            Unity_Smoothstep_float(_Subtract_9bc253d679574df3af14884d8a1da1ac_Out_2_Float, _Subtract_5410d776f36f413e8cfcf6fe7cca590b_Out_2_Float, _Distance_3e7a23d4689c447daa57611afba70214_Out_2_Float, _Smoothstep_968f3951c8f34bc19c268b73fbc85026_Out_3_Float);
            float _OneMinus_441adcef5d194f9d8c7e9e02ea2a0507_Out_1_Float;
            Unity_OneMinus_float(_Smoothstep_968f3951c8f34bc19c268b73fbc85026_Out_3_Float, _OneMinus_441adcef5d194f9d8c7e9e02ea2a0507_Out_1_Float);
            surface.Alpha = _OneMinus_441adcef5d194f9d8c7e9e02ea2a0507_Out_1_Float;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
            output.uv0 =                                        input.uv0;
        #if UNITY_ANY_INSTANCING_ENABLED
        #else // TODO: XR support for procedural instancing because in this case UNITY_ANY_INSTANCING_ENABLED is not defined and instanceID is incorrect.
        #endif
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
        #if UNITY_ANY_INSTANCING_ENABLED
        #else // TODO: XR support for procedural instancing because in this case UNITY_ANY_INSTANCING_ENABLED is not defined and instanceID is incorrect.
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ScenePickingPass"
            Tags
            {
                "LightMode" = "Picking"
            }
        
        // Render State
        Cull Back
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX_NORMAL_OUTPUT
        #define FEATURES_GRAPH_VERTEX_TANGENT_OUTPUT
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENEPICKINGPASS 1
        
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/DebugMipmapStreamingMacros.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(ATTRIBUTES_NEED_INSTANCEID)
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
             float4 uv0;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float _PulseSpeed;
        float2 _MinMaxHighlight;
        float _WaveSize;
        float4 _Color;
        float _SmoothEdge;
        UNITY_TEXTURE_STREAMING_DEBUG_VARS;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_CameraSortingLayerTexture);
        SAMPLER(sampler_CameraSortingLayerTexture);
        float4 _CameraSortingLayerTexture_TexelSize;
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        float Unity_SimpleNoise_ValueNoise_Deterministic_float (float2 uv)
        {
            float2 i = floor(uv);
            float2 f = frac(uv);
            f = f * f * (3.0 - 2.0 * f);
            uv = abs(frac(uv) - 0.5);
            float2 c0 = i + float2(0.0, 0.0);
            float2 c1 = i + float2(1.0, 0.0);
            float2 c2 = i + float2(0.0, 1.0);
            float2 c3 = i + float2(1.0, 1.0);
            float r0; Hash_Tchou_2_1_float(c0, r0);
            float r1; Hash_Tchou_2_1_float(c1, r1);
            float r2; Hash_Tchou_2_1_float(c2, r2);
            float r3; Hash_Tchou_2_1_float(c3, r3);
            float bottomOfGrid = lerp(r0, r1, f.x);
            float topOfGrid = lerp(r2, r3, f.x);
            float t = lerp(bottomOfGrid, topOfGrid, f.y);
            return t;
        }
        
        void Unity_SimpleNoise_Deterministic_float(float2 UV, float Scale, out float Out)
        {
            float freq, amp;
            Out = 0.0f;
            freq = pow(2.0, float(0));
            amp = pow(0.5, float(3-0));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
            freq = pow(2.0, float(1));
            amp = pow(0.5, float(3-1));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
            freq = pow(2.0, float(2));
            amp = pow(0.5, float(3-2));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A + B;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Distance_float2(float2 A, float2 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            UnityTexture2D _Property_43c38452057644c7befab43c635f6778_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float _TextureSize_6296f00037a549648168c3e259bbf177_Width_0_Float = _Property_43c38452057644c7befab43c635f6778_Out_0_Texture2D.texelSize.z;
            float _TextureSize_6296f00037a549648168c3e259bbf177_Height_2_Float = _Property_43c38452057644c7befab43c635f6778_Out_0_Texture2D.texelSize.w;
            float _TextureSize_6296f00037a549648168c3e259bbf177_TexelWidth_3_Float = _Property_43c38452057644c7befab43c635f6778_Out_0_Texture2D.texelSize.x;
            float _TextureSize_6296f00037a549648168c3e259bbf177_TexelHeight_4_Float = _Property_43c38452057644c7befab43c635f6778_Out_0_Texture2D.texelSize.y;
            float3 _Vector3_d3b74d1441ca41799a392e0b16b7cf2a_Out_0_Vector3 = float3(_TextureSize_6296f00037a549648168c3e259bbf177_Width_0_Float, _TextureSize_6296f00037a549648168c3e259bbf177_Height_2_Float, float(1));
            float3 _Multiply_f0f4dc336c734fc2a49491b9a87ef62f_Out_2_Vector3;
            Unity_Multiply_float3_float3(float3(0.0008, 0.0008, 0.0008), _Vector3_d3b74d1441ca41799a392e0b16b7cf2a_Out_0_Vector3, _Multiply_f0f4dc336c734fc2a49491b9a87ef62f_Out_2_Vector3);
            float _SimpleNoise_348a3914a33843a994913a5d7f184aae_Out_2_Float;
            Unity_SimpleNoise_Deterministic_float(IN.uv0.xy, float(20), _SimpleNoise_348a3914a33843a994913a5d7f184aae_Out_2_Float);
            float _Remap_3e5cc8bd01c643c49ef76e62ecea9b38_Out_3_Float;
            Unity_Remap_float(_SimpleNoise_348a3914a33843a994913a5d7f184aae_Out_2_Float, float2 (-1, 1), float2 (-1, 1), _Remap_3e5cc8bd01c643c49ef76e62ecea9b38_Out_3_Float);
            float3 _Multiply_83ae6eebe550408cabe6124d4be7871b_Out_2_Vector3;
            Unity_Multiply_float3_float3(_Multiply_f0f4dc336c734fc2a49491b9a87ef62f_Out_2_Vector3, (_Remap_3e5cc8bd01c643c49ef76e62ecea9b38_Out_3_Float.xxx), _Multiply_83ae6eebe550408cabe6124d4be7871b_Out_2_Vector3);
            float3 _Add_cb1e2b9d56d442578cb6b0a385eba233_Out_2_Vector3;
            Unity_Add_float3(IN.ObjectSpacePosition, _Multiply_83ae6eebe550408cabe6124d4be7871b_Out_2_Vector3, _Add_cb1e2b9d56d442578cb6b0a385eba233_Out_2_Vector3);
            description.Position = _Add_cb1e2b9d56d442578cb6b0a385eba233_Out_2_Vector3;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Property_5c08bf143c5244eba2331f381682df5f_Out_0_Float = _WaveSize;
            float _Subtract_5410d776f36f413e8cfcf6fe7cca590b_Out_2_Float;
            Unity_Subtract_float(float(0.6), _Property_5c08bf143c5244eba2331f381682df5f_Out_0_Float, _Subtract_5410d776f36f413e8cfcf6fe7cca590b_Out_2_Float);
            float _Property_74cea885cd8140b8aa931342595c6f8b_Out_0_Float = _SmoothEdge;
            float _Subtract_9bc253d679574df3af14884d8a1da1ac_Out_2_Float;
            Unity_Subtract_float(_Subtract_5410d776f36f413e8cfcf6fe7cca590b_Out_2_Float, _Property_74cea885cd8140b8aa931342595c6f8b_Out_0_Float, _Subtract_9bc253d679574df3af14884d8a1da1ac_Out_2_Float);
            float4 _UV_00bd6497fdad45788ff584f8a36c35c6_Out_0_Vector4 = IN.uv0;
            float2 _Vector2_1a977afd960540eeb69aa68b56dae506_Out_0_Vector2 = float2(float(0.5), float(0.5));
            float _Distance_3e7a23d4689c447daa57611afba70214_Out_2_Float;
            Unity_Distance_float2((_UV_00bd6497fdad45788ff584f8a36c35c6_Out_0_Vector4.xy), _Vector2_1a977afd960540eeb69aa68b56dae506_Out_0_Vector2, _Distance_3e7a23d4689c447daa57611afba70214_Out_2_Float);
            float _Smoothstep_968f3951c8f34bc19c268b73fbc85026_Out_3_Float;
            Unity_Smoothstep_float(_Subtract_9bc253d679574df3af14884d8a1da1ac_Out_2_Float, _Subtract_5410d776f36f413e8cfcf6fe7cca590b_Out_2_Float, _Distance_3e7a23d4689c447daa57611afba70214_Out_2_Float, _Smoothstep_968f3951c8f34bc19c268b73fbc85026_Out_3_Float);
            float _OneMinus_441adcef5d194f9d8c7e9e02ea2a0507_Out_1_Float;
            Unity_OneMinus_float(_Smoothstep_968f3951c8f34bc19c268b73fbc85026_Out_3_Float, _OneMinus_441adcef5d194f9d8c7e9e02ea2a0507_Out_1_Float);
            surface.Alpha = _OneMinus_441adcef5d194f9d8c7e9e02ea2a0507_Out_1_Float;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
            output.uv0 =                                        input.uv0;
        #if UNITY_ANY_INSTANCING_ENABLED
        #else // TODO: XR support for procedural instancing because in this case UNITY_ANY_INSTANCING_ENABLED is not defined and instanceID is incorrect.
        #endif
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
        #if UNITY_ANY_INSTANCING_ENABLED
        #else // TODO: XR support for procedural instancing because in this case UNITY_ANY_INSTANCING_ENABLED is not defined and instanceID is incorrect.
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "Sprite Unlit"
            Tags
            {
                "LightMode" = "UniversalForward"
            }
        
        // Render State
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZTest LEqual
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX_NORMAL_OUTPUT
        #define FEATURES_GRAPH_VERTEX_TANGENT_OUTPUT
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITEFORWARD
        #define _FOG_FRAGMENT 1
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/DebugMipmapStreamingMacros.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(ATTRIBUTES_NEED_INSTANCEID)
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float2 NDCPosition;
             float2 PixelPosition;
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
             float4 uv0;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 color : INTERP1;
             float3 positionWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.color.xyzw = input.color;
            output.positionWS.xyz = input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.color = input.color.xyzw;
            output.positionWS = input.positionWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED || defined(VARYINGS_NEED_INSTANCEID)
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float _PulseSpeed;
        float2 _MinMaxHighlight;
        float _WaveSize;
        float4 _Color;
        float _SmoothEdge;
        UNITY_TEXTURE_STREAMING_DEBUG_VARS;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_CameraSortingLayerTexture);
        SAMPLER(sampler_CameraSortingLayerTexture);
        float4 _CameraSortingLayerTexture_TexelSize;
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        float Unity_SimpleNoise_ValueNoise_Deterministic_float (float2 uv)
        {
            float2 i = floor(uv);
            float2 f = frac(uv);
            f = f * f * (3.0 - 2.0 * f);
            uv = abs(frac(uv) - 0.5);
            float2 c0 = i + float2(0.0, 0.0);
            float2 c1 = i + float2(1.0, 0.0);
            float2 c2 = i + float2(0.0, 1.0);
            float2 c3 = i + float2(1.0, 1.0);
            float r0; Hash_Tchou_2_1_float(c0, r0);
            float r1; Hash_Tchou_2_1_float(c1, r1);
            float r2; Hash_Tchou_2_1_float(c2, r2);
            float r3; Hash_Tchou_2_1_float(c3, r3);
            float bottomOfGrid = lerp(r0, r1, f.x);
            float topOfGrid = lerp(r2, r3, f.x);
            float t = lerp(bottomOfGrid, topOfGrid, f.y);
            return t;
        }
        
        void Unity_SimpleNoise_Deterministic_float(float2 UV, float Scale, out float Out)
        {
            float freq, amp;
            Out = 0.0f;
            freq = pow(2.0, float(0));
            amp = pow(0.5, float(3-0));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
            freq = pow(2.0, float(1));
            amp = pow(0.5, float(3-1));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
            freq = pow(2.0, float(2));
            amp = pow(0.5, float(3-2));
            Out += Unity_SimpleNoise_ValueNoise_Deterministic_float(float2(UV.xy*(Scale/freq)))*amp;
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A + B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Distance_float2(float2 A, float2 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            UnityTexture2D _Property_43c38452057644c7befab43c635f6778_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float _TextureSize_6296f00037a549648168c3e259bbf177_Width_0_Float = _Property_43c38452057644c7befab43c635f6778_Out_0_Texture2D.texelSize.z;
            float _TextureSize_6296f00037a549648168c3e259bbf177_Height_2_Float = _Property_43c38452057644c7befab43c635f6778_Out_0_Texture2D.texelSize.w;
            float _TextureSize_6296f00037a549648168c3e259bbf177_TexelWidth_3_Float = _Property_43c38452057644c7befab43c635f6778_Out_0_Texture2D.texelSize.x;
            float _TextureSize_6296f00037a549648168c3e259bbf177_TexelHeight_4_Float = _Property_43c38452057644c7befab43c635f6778_Out_0_Texture2D.texelSize.y;
            float3 _Vector3_d3b74d1441ca41799a392e0b16b7cf2a_Out_0_Vector3 = float3(_TextureSize_6296f00037a549648168c3e259bbf177_Width_0_Float, _TextureSize_6296f00037a549648168c3e259bbf177_Height_2_Float, float(1));
            float3 _Multiply_f0f4dc336c734fc2a49491b9a87ef62f_Out_2_Vector3;
            Unity_Multiply_float3_float3(float3(0.0008, 0.0008, 0.0008), _Vector3_d3b74d1441ca41799a392e0b16b7cf2a_Out_0_Vector3, _Multiply_f0f4dc336c734fc2a49491b9a87ef62f_Out_2_Vector3);
            float _SimpleNoise_348a3914a33843a994913a5d7f184aae_Out_2_Float;
            Unity_SimpleNoise_Deterministic_float(IN.uv0.xy, float(20), _SimpleNoise_348a3914a33843a994913a5d7f184aae_Out_2_Float);
            float _Remap_3e5cc8bd01c643c49ef76e62ecea9b38_Out_3_Float;
            Unity_Remap_float(_SimpleNoise_348a3914a33843a994913a5d7f184aae_Out_2_Float, float2 (-1, 1), float2 (-1, 1), _Remap_3e5cc8bd01c643c49ef76e62ecea9b38_Out_3_Float);
            float3 _Multiply_83ae6eebe550408cabe6124d4be7871b_Out_2_Vector3;
            Unity_Multiply_float3_float3(_Multiply_f0f4dc336c734fc2a49491b9a87ef62f_Out_2_Vector3, (_Remap_3e5cc8bd01c643c49ef76e62ecea9b38_Out_3_Float.xxx), _Multiply_83ae6eebe550408cabe6124d4be7871b_Out_2_Vector3);
            float3 _Add_cb1e2b9d56d442578cb6b0a385eba233_Out_2_Vector3;
            Unity_Add_float3(IN.ObjectSpacePosition, _Multiply_83ae6eebe550408cabe6124d4be7871b_Out_2_Vector3, _Add_cb1e2b9d56d442578cb6b0a385eba233_Out_2_Vector3);
            description.Position = _Add_cb1e2b9d56d442578cb6b0a385eba233_Out_2_Vector3;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_8dae12f47bf34cf893202e2b3827d1e8_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_CameraSortingLayerTexture);
            float4 _ScreenPosition_8812d76f920c48178bfa5ad28781258d_Out_0_Vector4 = float4(IN.NDCPosition.xy, 0, 0);
            float4 _SampleTexture2D_9842fdb51ef849fe80e396e9aec03e50_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_8dae12f47bf34cf893202e2b3827d1e8_Out_0_Texture2D.tex, _Property_8dae12f47bf34cf893202e2b3827d1e8_Out_0_Texture2D.samplerstate, _Property_8dae12f47bf34cf893202e2b3827d1e8_Out_0_Texture2D.GetTransformedUV((_ScreenPosition_8812d76f920c48178bfa5ad28781258d_Out_0_Vector4.xy)) );
            float _SampleTexture2D_9842fdb51ef849fe80e396e9aec03e50_R_4_Float = _SampleTexture2D_9842fdb51ef849fe80e396e9aec03e50_RGBA_0_Vector4.r;
            float _SampleTexture2D_9842fdb51ef849fe80e396e9aec03e50_G_5_Float = _SampleTexture2D_9842fdb51ef849fe80e396e9aec03e50_RGBA_0_Vector4.g;
            float _SampleTexture2D_9842fdb51ef849fe80e396e9aec03e50_B_6_Float = _SampleTexture2D_9842fdb51ef849fe80e396e9aec03e50_RGBA_0_Vector4.b;
            float _SampleTexture2D_9842fdb51ef849fe80e396e9aec03e50_A_7_Float = _SampleTexture2D_9842fdb51ef849fe80e396e9aec03e50_RGBA_0_Vector4.a;
            float4 _Property_c1e1471ab3344e36a423f48911e89fb9_Out_0_Vector4 = _Color;
            float _Property_40f877571ff4439a9a92ea0a3b5d9823_Out_0_Float = _PulseSpeed;
            float _Multiply_539f0df73ad440f0a3f8747551f70836_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_40f877571ff4439a9a92ea0a3b5d9823_Out_0_Float, _Multiply_539f0df73ad440f0a3f8747551f70836_Out_2_Float);
            float2 _TilingAndOffset_91338700237145828bf3b7fb6c2490c4_Out_3_Vector2;
            Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_Multiply_539f0df73ad440f0a3f8747551f70836_Out_2_Float.xx), _TilingAndOffset_91338700237145828bf3b7fb6c2490c4_Out_3_Vector2);
            float _SimpleNoise_bb64167a340641da8d9c4794d228b332_Out_2_Float;
            Unity_SimpleNoise_Deterministic_float(_TilingAndOffset_91338700237145828bf3b7fb6c2490c4_Out_3_Vector2, float(10), _SimpleNoise_bb64167a340641da8d9c4794d228b332_Out_2_Float);
            float2 _Property_b3062eb0c49b41179a8f46ea657adcc0_Out_0_Vector2 = _MinMaxHighlight;
            float _Remap_306e090b0d67498daf29e483a63b65c9_Out_3_Float;
            Unity_Remap_float(_SimpleNoise_bb64167a340641da8d9c4794d228b332_Out_2_Float, float2 (0, 1), _Property_b3062eb0c49b41179a8f46ea657adcc0_Out_0_Vector2, _Remap_306e090b0d67498daf29e483a63b65c9_Out_3_Float);
            UnityTexture2D _Property_147bf67514134240a8978048642218bd_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_3b790c1044a34a5fa441a26b85e03f0d_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_147bf67514134240a8978048642218bd_Out_0_Texture2D.tex, _Property_147bf67514134240a8978048642218bd_Out_0_Texture2D.samplerstate, _Property_147bf67514134240a8978048642218bd_Out_0_Texture2D.GetTransformedUV(IN.uv0.xy) );
            float _SampleTexture2D_3b790c1044a34a5fa441a26b85e03f0d_R_4_Float = _SampleTexture2D_3b790c1044a34a5fa441a26b85e03f0d_RGBA_0_Vector4.r;
            float _SampleTexture2D_3b790c1044a34a5fa441a26b85e03f0d_G_5_Float = _SampleTexture2D_3b790c1044a34a5fa441a26b85e03f0d_RGBA_0_Vector4.g;
            float _SampleTexture2D_3b790c1044a34a5fa441a26b85e03f0d_B_6_Float = _SampleTexture2D_3b790c1044a34a5fa441a26b85e03f0d_RGBA_0_Vector4.b;
            float _SampleTexture2D_3b790c1044a34a5fa441a26b85e03f0d_A_7_Float = _SampleTexture2D_3b790c1044a34a5fa441a26b85e03f0d_RGBA_0_Vector4.a;
            float4 _Multiply_37d247c67df446e881c32a458bb98bad_Out_2_Vector4;
            Unity_Multiply_float4_float4((_Remap_306e090b0d67498daf29e483a63b65c9_Out_3_Float.xxxx), _SampleTexture2D_3b790c1044a34a5fa441a26b85e03f0d_RGBA_0_Vector4, _Multiply_37d247c67df446e881c32a458bb98bad_Out_2_Vector4);
            float4 _Multiply_068eab5a8cab4e3ebd34c63ba5a2d9c6_Out_2_Vector4;
            Unity_Multiply_float4_float4(_Property_c1e1471ab3344e36a423f48911e89fb9_Out_0_Vector4, _Multiply_37d247c67df446e881c32a458bb98bad_Out_2_Vector4, _Multiply_068eab5a8cab4e3ebd34c63ba5a2d9c6_Out_2_Vector4);
            float4 _Multiply_0475505356d24598aeec07482d0acc22_Out_2_Vector4;
            Unity_Multiply_float4_float4(_SampleTexture2D_9842fdb51ef849fe80e396e9aec03e50_RGBA_0_Vector4, _Multiply_068eab5a8cab4e3ebd34c63ba5a2d9c6_Out_2_Vector4, _Multiply_0475505356d24598aeec07482d0acc22_Out_2_Vector4);
            float _Property_5c08bf143c5244eba2331f381682df5f_Out_0_Float = _WaveSize;
            float _Subtract_5410d776f36f413e8cfcf6fe7cca590b_Out_2_Float;
            Unity_Subtract_float(float(0.6), _Property_5c08bf143c5244eba2331f381682df5f_Out_0_Float, _Subtract_5410d776f36f413e8cfcf6fe7cca590b_Out_2_Float);
            float _Property_74cea885cd8140b8aa931342595c6f8b_Out_0_Float = _SmoothEdge;
            float _Subtract_9bc253d679574df3af14884d8a1da1ac_Out_2_Float;
            Unity_Subtract_float(_Subtract_5410d776f36f413e8cfcf6fe7cca590b_Out_2_Float, _Property_74cea885cd8140b8aa931342595c6f8b_Out_0_Float, _Subtract_9bc253d679574df3af14884d8a1da1ac_Out_2_Float);
            float4 _UV_00bd6497fdad45788ff584f8a36c35c6_Out_0_Vector4 = IN.uv0;
            float2 _Vector2_1a977afd960540eeb69aa68b56dae506_Out_0_Vector2 = float2(float(0.5), float(0.5));
            float _Distance_3e7a23d4689c447daa57611afba70214_Out_2_Float;
            Unity_Distance_float2((_UV_00bd6497fdad45788ff584f8a36c35c6_Out_0_Vector4.xy), _Vector2_1a977afd960540eeb69aa68b56dae506_Out_0_Vector2, _Distance_3e7a23d4689c447daa57611afba70214_Out_2_Float);
            float _Smoothstep_968f3951c8f34bc19c268b73fbc85026_Out_3_Float;
            Unity_Smoothstep_float(_Subtract_9bc253d679574df3af14884d8a1da1ac_Out_2_Float, _Subtract_5410d776f36f413e8cfcf6fe7cca590b_Out_2_Float, _Distance_3e7a23d4689c447daa57611afba70214_Out_2_Float, _Smoothstep_968f3951c8f34bc19c268b73fbc85026_Out_3_Float);
            float _OneMinus_441adcef5d194f9d8c7e9e02ea2a0507_Out_1_Float;
            Unity_OneMinus_float(_Smoothstep_968f3951c8f34bc19c268b73fbc85026_Out_3_Float, _OneMinus_441adcef5d194f9d8c7e9e02ea2a0507_Out_1_Float);
            surface.BaseColor = (_Multiply_0475505356d24598aeec07482d0acc22_Out_2_Vector4.xyz);
            surface.Alpha = _OneMinus_441adcef5d194f9d8c7e9e02ea2a0507_Out_1_Float;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
            output.uv0 =                                        input.uv0;
        #if UNITY_ANY_INSTANCING_ENABLED
        #else // TODO: XR support for procedural instancing because in this case UNITY_ANY_INSTANCING_ENABLED is not defined and instanceID is incorrect.
        #endif
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x < 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
            #else
            output.PixelPosition = float2(input.positionCS.x, (_ProjectionParams.x > 0) ? (_ScaledScreenParams.y - input.positionCS.y) : input.positionCS.y);
            #endif
        
            output.NDCPosition = output.PixelPosition.xy / _ScaledScreenParams.xy;
            output.NDCPosition.y = 1.0f - output.NDCPosition.y;
        
            output.uv0 = input.texCoord0;
        #if UNITY_ANY_INSTANCING_ENABLED
        #else // TODO: XR support for procedural instancing because in this case UNITY_ANY_INSTANCING_ENABLED is not defined and instanceID is incorrect.
        #endif
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteUnlitPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    CustomEditorForRenderPipeline "UnityEditor.ShaderGraphSpriteGUI" "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset"
    FallBack "Hidden/Shader Graph/FallbackError"
}