Shader "Unlit/GameTWS"
{               //https://roystan.net/articles/toon-water.html
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("NoiseTexture", 2D) = "white" {}

        _DistortionNoiseScroll("Distortion Noise Scroll Amount", Vector) = (0.03, 0.03, 0, 0)

        _DistortionTex ("DistortionTexture", 2D) = "white" {}
        _DistortionStrength("_DistortionStrength", Range(0,1)) = 0.5

        _EdgeFoamDistance("Foam Distance", Float) = 0.4

        _DepthGradientShallow("Depth Gradient Shallow", Color) = (0.1,0.5,0.9,0.7)//(0.325, 0.807, 0.971, 0.725) // LIGHTER
        _DepthGradientDeep("Depth Gradient Deep", Color) = (0, 0.3, 1,0.7)//(0.086, 0.407, 1, 0.749)             // DARKER
        _DepthMaxDistance("Depth Maximum Distance", Float) = 1

        _SurfaceNoiseCutoffFix("Surace Noise Texture Bright Fix", Range(0, 1)) = 1


        _Amp("Water Strength",Range(0,2)) = 1
    }
    SubShader
    {
    /*
        Tags 
        { 
            "RenderType"="Transparent" 
        }
        */
        Tags 
        { 
            "Queue"="Transparent" 
        }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            //https://wiki.unity3d.com/index.php/Shader_Code#Vertex_Shader_to_Fragment_Shader_Structure_.28v2f.29
            //https://docs.unity3d.com/Manual/SL-UnityShaderVariables.html
            //https://docs.unity3d.com/Manual/SL-VertexProgramInputs.html
            struct appdata
            {
                float2 uv : TEXCOORD0;
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 noiseUV : TEXCOORD0;
                float2 distortionUV : TEXCOORD1;
                float4 screenPosition : TEXCOORD2;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float4 color : COLOR;
            };

            //_ST = tiling and offset data

            sampler2D _MainTex;
            float4 _MainTexColor;
            float4 _MainTex_ST;

            sampler2D _NoiseTex;
            float4 _NoiseTexColor;
            float4 _NoiseTex_ST;
            float _SurfaceNoiseCutoffFix;

            sampler2D _DistortionTex;
            float4 _DistortionTex_ST;

            float _DistortionStrength;
            float2 _DistortionNoiseScroll;

            float _EdgeFoamDistance;

            float4 _Color;
            float _Amp;

            float4 _DepthGradientShallow;
            float4 _DepthGradientDeep;

            float _DepthMaxDistance;

            sampler2D _CameraDepthTexture;

            v2f vert (appdata v)
            {
                v2f o;

                //v.vertex.xyz += sin(v.normal.y + (_Time.y * 2));//v.normal * 1; 
                //v.vertex.y += sin(v.vertex.y + (_Time.y * 2));   //(sin() )* 5 
                //v.vertex.y += sin(v.vertex.x + (_Time.y * 2));
                /*
                float sWave1 = sin(v.vertex.y + (_Time.y * 2));
                float sWave2 = sin(v.vertex.x + (_Time.y * 2));
                float sWave3 = sin(v.vertex.z + (_Time.y * 2));
                v.vertex.y += (sWave1 * 0.5) + (sWave2 * 1) + (sWave3 * 1);
                */
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                //https://docs.unity3d.com/Manual/SL-BuiltinFunctions.html
                o.screenPosition = ComputeScreenPos(o.vertex);
                
                //Noise tex
                o.noiseUV = TRANSFORM_TEX(v.uv, _NoiseTex);

                //Distortion tex
                o.distortionUV = TRANSFORM_TEX(v.uv, _DistortionTex);


                /*
                float a = sin(v.vertex.xyz + (_Time.y * 0.5));
                
                float noiseValue = tex2Dlod(_NoiseTex, float4(v.vertex.xy,0,0) * a);          
                v.vertex.xyz += v.normal.xyz * noiseValue * _Amp;
                */

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
            //https://developer.download.nvidia.com/cg/tex2Dproj.html tex2Dproj > tex2D transform the coordinate from an orthographic to a perspective projection.
            //https://docs.unity3d.com/Manual/SL-BuiltinMacros.html UNITY_PROJ_COORD
            //https://docs.unity3d.com/Manual/SL-DepthTextures.html LinearEyeDepth
            //https://developer.download.nvidia.com/cg/lerp.html lerp
            //https://developer.download.nvidia.com/cg/saturate.html saturate
            //.r = rgba / xyzw channel
                //sample the depth texture using tex2dproj function & taking in the screen space pos & should return the depth of the surface of the water with a range of 0 - 1.
                float depthTexture = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPosition)).r;
                //now we have the depth texture the problem is that its not linear, so the camera has to be extremely close to the water to see any brightness so we make it linear using the linear eye depth function.
                //since the value above is non linear we change it to be linear now
                //depth in unity units
                float depthLinear = LinearEyeDepth(depthTexture);
                //now we need to know how deep this depth value is relative to our water surface & need to know the depth of the water surface.
                //the good thing is the depth of the water surface is just the w comp in i.screenpos so now we take the diffrence of the 2 depth values & return the result. 
                // Difference, in Unity units, between the water's surface and the object behind it.
                float depthDifference = depthLinear - i.screenPosition.w;

                //water coloring
                //lerp 2 values lerp the depth shallow & deep based on the 3rd val 0 - 1 which is water depth difference.
                //Now depth diff is in world unites so we need to know how deep it is compared to our max depth percentage wise so we divide depth difference by max depth
                //saturate will return a 0-1 val.
                float waterDepthDifference = saturate(depthDifference / _DepthMaxDistance);
                //feed the saturat clamped 0 - 1 num to lerp & to calc the gradient and return the new color
                float4 waterColor = lerp(_DepthGradientShallow, _DepthGradientDeep, waterDepthDifference);
                

                //random distortion texture
                float2 sampleDistortionTexture = (tex2D(_DistortionTex, i.distortionUV).xy * 2 - 1) * _DistortionStrength;

                // OLD scrolling the noise texture test
                //float2 distortionNoiseUV = float2(i.noiseUV.x + _Time.y * _DistortionNoiseScroll.x, i.noiseUV.y + _Time.y * _DistortionNoiseScroll.y);

                //Changed to start using the random distortion texture
                //Distort the noise UV based off the RG/XY channels of the distortion texture. the noffset the uv by time & scale it by scroll speed
                float2 distortionNoiseUV = float2(((i.noiseUV.x + _Time.y) * _DistortionNoiseScroll.x) + sampleDistortionTexture.x, ((i.noiseUV.y + _Time.y) * _DistortionNoiseScroll.y) + sampleDistortionTexture.y);
                
                // OLD scrolling sample nosie Texture
                //float sampleNoiseTexture = tex2D(_NoiseTex, i.noiseUV).r;

                //changed the UV to take the new distortion noise UV texture
                float sampleNoiseTexture = tex2D(_NoiseTex, distortionNoiseUV).r;

                //applying a cutoff threshhold to get a nicer look with the surface noise

                //
                // Add in the fragment shader, above the existing surfaceNoise declaration.
                float foamDepthDifference = saturate(depthDifference / _EdgeFoamDistance);
                float surfaceNoiseCutoff = foamDepthDifference * _SurfaceNoiseCutoffFix;
                //


                float surfaceNoise = sampleNoiseTexture > surfaceNoiseCutoff ? 1 : 0;
                //float surfaceNoise = sampleNoiseTexture > _SurfaceNoiseCutoffFix ? 1 : 0;
                

                return (waterColor * _Amp) + surfaceNoise;
            }
            ENDCG
        }
    }
}
