#include <Packages/com.blendernodesgraph.core/Editor/Includes/Importers.hlsl>

void BlenderMat_float(float3 _POS, float3 _PVS, float3 _PWS, float3 _NOS, float3 _NVS, float3 _NWS, float3 _NTS, float3 _TWS, float3 _BTWS, float3 _UV, float3 _SP, float3 _VVS, float3 _VWS, Texture2D gradient_3618, Texture2D gradient_29980, out float4 Color, out float3 Normal, out float Smoothness, out float4 Emission, out float AmbientOcculusion, out float Metallic, out float4 Specular)
{
	
	float4 _ColorRamp_3618 = color_ramp(gradient_3618, 0,5);
	float4 _ColorRamp_29980 = color_ramp(gradient_29980, 0,5);
	float4 _MixRGB_19664 = mix_blend(0,5, _ColorRamp_3618, _ColorRamp_29980);

	Color = _MixRGB_19664;
	Normal = float3(0.0, 0.0, 0.0);
	Smoothness = 0.0;
	Emission = float4(0.0, 0.0, 0.0, 0.0);
	AmbientOcculusion = 0.0;
	Metallic = 0.0;
	Specular = float4(0.0, 0.0, 0.0, 0.0);
}