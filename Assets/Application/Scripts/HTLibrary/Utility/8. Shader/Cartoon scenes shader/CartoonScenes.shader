Shader "HTLibrary/CarttonScenes"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Diffuse("Color",Color) = (1,1,1,1)
		_BumpMap("Normal Map",2D ) = "white"{}
		_BumpScale("Bump Scale", float) = 1
		_Outline("Outline", Range(0,0.2)) = 0.1
		_OutlineColor("OutlineColor", Color) = (0,0,0,0)
		_Step("Step", Range(1,30)) = 1
		_ToonEffect("ToonEffect", Range(0,1)) = 0.5
		_Snow("Snow Level", Range(0,1)) = 0.5
		_SnowColor("SnowColor", Color) = (1,1,1,1)
		_SnowDir("SnowDir", Vector) = (0,1,0)

		_AmbientLightingTrashold("Ambient Lighting Trashold",float)=1
    }

    SubShader
    {
        Tags {"LightMode"="ForwardBase" }
        LOD 100
		 
		//使用019 的Outline的Pass通道
		UsePass "HTLibrary/CartoonShader/Outline"

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			//宏的定义 代表可以通过这个 开关雪的特效
		    //#pragma multi_compile_fwdbase
			#pragma multi_compile __ SNOW_ON
			 //要想有正确的衰减内置变量等，必须要有这句
            //#pragma multi_compile_fwdbase
            #include "UnityCG.cginc"
			#include "Lighting.cginc"
		   // #include "autolight.cginc"
			
            struct v2f
            {
                float4 pos : SV_POSITION;
				float4 uv :TEXCOORD0;
				float4 TtoW0 : TEXCOORD1;
				float4 TtoW1 :TEXCOORD2;
				float4 TtoW2 : TEXCOORD3;
				
			    //SHADOW_COORDS(4)    //宏表示为定义一个float4的采样坐标，放到编号为1的寄存器中
				//V2F_SHADOW_CASTER;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed4 _Diffuse;
			float _Step;
			float _ToonEffect;
			sampler2D _BumpMap;
			float4 _BumpMap_ST;
			float _BumpScale;
			//积雪
			//雪的大小
			float _Snow; 
			//雪的颜色
			float4 _SnowColor;
			//雪的方向
			float4 _SnowDir;
			float _AmbientLightingTrashold;
            v2f vert (appdata_tan v)
            {
                v2f o;

                o.pos= UnityObjectToClipPos(v.vertex);

                o.uv.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
							 
				o.uv.zw = TRANSFORM_TEX(v.texcoord, _BumpMap);

				fixed3 worldPos = mul(unity_ObjectToWorld, v.vertex);
				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
				fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
				fixed3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w;

				o.TtoW0 = float4(worldTangent.x,worldBinormal.x,worldNormal.x, worldPos.x);
				o.TtoW1 = float4(worldTangent.y,worldBinormal.y,worldNormal.y, worldPos.y);
				o.TtoW2 = float4(worldTangent.z,worldBinormal.z,worldNormal.z, worldPos.z);
			    
			    //TRANSFER_SHADOW(o);								
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz*_AmbientLightingTrashold+_AmbientLightingTrashold;

                fixed4 albedo = tex2D(_MainTex, i.uv);
				//fixed shadow = SHADOW_ATTENUATION(i);

				float3 worldPos = float3(i.TtoW0.w,i.TtoW1.w,i.TtoW2.w);

				fixed3 lightDir = UnityWorldSpaceLightDir(worldPos);
				fixed3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));

				//求法线
				fixed4 packedNormal = tex2D(_BumpMap,i.uv.zw);
				fixed3 tangentNormal = UnpackNormal(packedNormal);
				tangentNormal.xy *= _BumpScale;
				fixed3 worldNormal = normalize(float3(dot(i.TtoW0.xyz, tangentNormal), dot(i.TtoW1.xyz, tangentNormal),dot(i.TtoW2.xyz,tangentNormal)));

				float difLight = dot(lightDir,worldNormal)*0.5+0.5;
				//loat difLight= saturate(dot(worldNormal, normalize(lightDir)));		  
				difLight = smoothstep(0,1,difLight);
				float toon = floor(difLight * _Step)/_Step;
				difLight = lerp(difLight, toon, _ToonEffect);
				
				fixed3 diffuse = _LightColor0.rgb * albedo * _Diffuse.rgb* difLight;
				
				fixed4 color = fixed4(ambient + diffuse,1);
															
				//当满足条件 进行对应的片元改变颜色
				if(dot(worldNormal, _SnowDir.xyz) > lerp(1,-1,_Snow))
				{
					color.rgb = _SnowColor.rgb;
					//color.rgb=color.rgb;
				}
				else 
				{
					color.rgb = color.rgb;
				}
				
                return color;
            }
            ENDCG
        }
    }
	FallBack "Diffuse"
}
