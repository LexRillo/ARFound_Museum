// original from https://forum.unity.com/threads/clipping-objects-with-a-plane.537058/
Shader "Custom/plane clipping" {
	Properties{
		_Color("Color", Color) = (178,154,86,255)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	  _cutting_plane_norm("cutting plane normal",Vector) = (1,0,0,0)

	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200
			Cull OFF


			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0



			sampler2D _MainTex;
		  float4 _cutting_plane_norm;

			struct Input {
				float2 uv_MainTex;
				float3 worldPos;
				float3 worldNormal;
				float3 viewDir;
			 fixed facing : VFACE;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)


			void surf(Input IN, inout SurfaceOutputStandard o)
			{

			float3 localPos = IN.worldPos - mul(unity_ObjectToWorld, float4(0,0,0,1)).xyz;

			float norm_calc = _cutting_plane_norm.x *localPos.x +
								_cutting_plane_norm.y *localPos.y +
								_cutting_plane_norm.z *localPos.z +
								_cutting_plane_norm.w;

				clip(norm_calc > 0 ? -1 : 1);


				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

				//            if (dot(IN.worldNormal, IN.viewDir) > _e)
				//                o.Albedo = c.rgb;
				//            else
				//                o.Albedo = _Color;

						 if (IN.facing > 0)
								o.Albedo = _Color;
						 else
						 {
								o.Albedo = float3(0,0,0);
							o.Emission = _Color;
						 }

						 // Metallic and smoothness come from slider variables
						 o.Metallic = _Metallic;
						 o.Smoothness = _Glossiness;
						 o.Alpha = c.a;
					 }
					 ENDCG
		}
			FallBack "Diffuse"
}