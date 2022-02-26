Shader "HTLibrary/CartoonShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Diffuse("Color", Color) = (1,1,1,1)
		_Outline("Outline", Range(0,0.2)) = 0.1
		_OutlineColor("OutlineColor", Color) =(0,0,0,0)
		_Steps("Steps",Range(1,30)) = 1
		_ToonEffect("ToonEffect", Range(0,1)) = 0.5
		//边缘光
		_RimColor("RimColor", Color) =(1,1,1,1)
		_RimPower("RimPower", Range(0.0001,3)) = 1
		//Xray
		_XRayColor("XRayColor", Color) =(1,1,1,1)
		_XRayPower("XRayPower", Range(0.0001,3)) = 1
    }

    SubShader
    {
		//+ 1000 可以 让程序先执行 不透明物体 在执行透明物体的渲染  
        Tags {"Queue"= "Geometry+1000" "RenderType"="Opaque" }
        LOD 100

		Pass
		{
			Name "XRay"
			//不需要阴影这个Pass 通道
			Tags{ "ForceNoShadowCasting" = "true" }
			Blend SrcAlpha One

			//关闭深度写入
			ZWrite Off 

			//自定义深度测试
			ZTest Greater
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			//控制Xray 颜色
			fixed4 _XRayColor;
			//控制Xray 范围
			float _XRayPower;

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 viewDir : TEXCOORD0;
				float3 normal : TEXCOORD1;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = v.normal;
				o.viewDir = ObjSpaceViewDir(v.vertex);
				return o;
			}

			fixed4 frag(v2f i):SV_Target
			{
				float3 normal = normalize(i.normal);
				float3 viewDir = normalize(i.viewDir);
				//将值控制在 0-1 1 就是切线方向 也就是边缘
				float rim = 1 - dot(normal,viewDir); 
				//控制强度 0-1
				return _XRayColor * pow(rim, 1/_XRayPower);
				//return _XRayColor * rim * _XRayPower;
			}
			ENDCG
		}

		Pass
		{
			Name "Outline"
			Cull Front
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float _Outline;
			fixed4 _OutlineColor;

			struct v2f
			{
				float4 vertex :SV_POSITION;
			};

			v2f vert (appdata_base v)
			{
				v2f o;
				//物体空间法线外拓
				v.vertex.xyz += v.normal * _Outline;
				o.vertex = UnityObjectToClipPos(v.vertex);

				return o;
			}

			float4 frag(v2f i):SV_Target
			{
				return _OutlineColor;
			}
			ENDCG
		}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
			#include "Lighting.cginc"

            struct v2f
            {
				float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed3 worldNormal:TEXCOORD1;
				float3 worldPos: TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _Diffuse;
			float _Steps;
			float _ToonEffect;
			sampler2D _RampTex;
			fixed4 _RimColor;
			float _RimPower;

            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

                fixed4 albedo = tex2D(_MainTex, i.uv);


				fixed3 worldLightDir = UnityWorldSpaceLightDir(i.worldPos);
				fixed3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));

				//卡通颜色
				float difLight = dot(worldLightDir,i.worldNormal)*0.5+0.5;
				//渐进纹理颜色值
				//fixed4 rampColor = tex2D(_RampTex, fixed2(difLight,difLight));

				////颜色平滑在【0，1】之间
				difLight = smoothstep(0,1,difLight);
				//颜色离散化
				float toon = floor(difLight * _Steps) /_Steps;
				difLight = lerp(difLight, toon, _ToonEffect);

				fixed3 diffuse = _LightColor0.rgb * albedo * _Diffuse.rgb * difLight;// * rampColor;

				float rim = 1 - dot(i.worldNormal, viewDir);
				fixed3 rimColor = _RimColor * pow(rim, 1/_RimPower);

                return float4( ambient + diffuse + rimColor,1);
            }
            ENDCG
        }
    }
	FallBack "Diffuse"
}
