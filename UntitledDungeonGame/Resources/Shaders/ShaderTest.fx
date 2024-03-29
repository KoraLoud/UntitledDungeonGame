﻿#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);
	color.gb = color.r;
	return color;
}

technique BasicColorDrawing
{
	pass P0
	{
		//PixelShader = compile PS_SHADERMODEL MainPS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};