Shader "Unlit/My First Lighting Shader"
{
	Properties{
		 _Tint("Tint", Color) = (1,1,1,1)
		 _MainTex("Albedo", 2D) = "white" {}
	_Smoothness("Smoothness", Range(0,1)) = 0.5
		[Gamma] _Metallic("Metallic", Range(0,1)) = 0
	}

		SubShader
	{
		Pass {
			Tags {"LightMode" = "ForwardBase"}
			CGPROGRAM

			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram
			#pragma target 3.0
			#pragma multi_compile_VERTEXLIGHT_ON


			#define FORWARD_BASE_PASS
			#include "MyLighting.cginc"

			ENDCG
		}

		Pass {
			Tags {"LightMode" = "ForwardAdd"}

			Blend One One
			ZWrite Off

			CGPROGRAM

			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram
			#pragma target 3.0
			#pragma multi_compile_fwdadd
		//#pragma multi_compile DIRECTIONAL DIRECTIONAL_COOKIE POINT POINT_COOKIE SPOT
		#include "MyLighting.cginc"

		ENDCG
	}
	}
}
