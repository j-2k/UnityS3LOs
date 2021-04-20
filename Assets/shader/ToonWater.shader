Shader "Unlit/ToonWater"
{
    Properties
    {
        //_MainTex("Surface Noise", 2D) = "white" {}
        _SurfaceNoise("Surface Noise", 2D) = "white" {}
        _SurfaceNoiseCutoff("Surface Noise Cutoff", Range(0, 1)) = 0.777
        _FoamDistance("Foam Distance", Float) = 0.4
        _DepthGradientShallow("Depth Gradient Shallow", Color) = (0.325, 0.807, 0.971, 0.725)
        _DepthGradientDeep("Depth Gradient Deep", Color) = (0.086, 0.407, 1, 0.749)
        _DepthMaxDistance("Depth Maximum Distance", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        //Tags { "Queue"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                //float2 uv : TEXCOORD0;
                float4 uv : TEXCOORD0;
            };

            struct v2f
            {
                //float2 uv : TEXCOORD0;
                float2 noiseUV : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 screenPosition : TEXCOORD2;
            };

            //sampler2D _MainTex;
            //float4 _MainTex_ST;

            float4 _DepthGradientShallow;
            float4 _DepthGradientDeep;

            float _DepthMaxDistance;

            sampler2D _CameraDepthTexture;

            sampler2D _SurfaceNoise;
            float4 _SurfaceNoise_ST;

            float _SurfaceNoiseCutoff;

            float _FoamDistance;

            v2f vert (appdata v)
            {
                v2f o;
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPosition = ComputeScreenPos(o.vertex);
                o.noiseUV = TRANSFORM_TEX(v.uv, _SurfaceNoise);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //divide by the depth/projection
                //float2 uv = i.screenPosition.xy / i.screenPosition.w;

                //fixed4 col = tex2D(_MainTex, i.uv); //_CameraDepthTexture, uv/i.screenpos

                float existingDepth01 = tex2D(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPosition.xy / i.screenPosition.w)).r;
                //float existingDepth01 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPosition)).r;
                // it is not linear so we make it linear, when its not linear assuming you have 0 - 1 the closer you are the more presice the numbers will be in deciamls 
                //compared to when your far away it wont be as precise when you are close. So now we change it to linear so the distance would be constant based on how close/far we are.
                float existingDepthLinear = LinearEyeDepth(existingDepth01);

                float depthDifference = existingDepthLinear - i.screenPosition.w;

                float waterDepthDifference01 = saturate(depthDifference / _DepthMaxDistance);
                float4 waterColor = lerp(_DepthGradientShallow, _DepthGradientDeep, waterDepthDifference01);
                float surfaceNoiseSample = tex2D(_SurfaceNoise, i.noiseUV).r;

                float foamDepthDifference01 = saturate(depthDifference / _FoamDistance);
                float surfaceNoiseCutoff = foamDepthDifference01 * _SurfaceNoiseCutoff;

                float surfaceNoise = surfaceNoiseSample > surfaceNoiseCutoff ? 1 : 0;

                return waterColor + surfaceNoise;

                /*
                // sample the texture
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
                */
            }
            ENDCG
        }
    }
}
