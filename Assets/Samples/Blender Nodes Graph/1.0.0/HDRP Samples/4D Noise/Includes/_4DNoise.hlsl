#include <Packages/com.blendernodesgraph.core/Editor/Includes/Importers.hlsl>

void _4DNoise_float(float3 _POS, float3 _PVS, float3 _PWS, float3 _NOS, float3 _NVS, float3 _NWS, float3 _NTS, float3 _TWS, float3 _BTWS, float3 _UV, float3 _SP, float3 _VVS, float3 _VWS, out float4 ColorOut)
{
	
	float _Math_21032 = math_multiply(_Time, 3, 0.5);
	float4 _Mapping_21028 = float4(mapping_point(float4(_POS, 0), float3(0, 0, 0), float3(0, 0, 0), float3(1, 1, 1)), 0);
	float _SimpleNoiseTexture_21034_fac; float4 _SimpleNoiseTexture_21034_col; node_simple_noise_texture_full(_Mapping_21028, _Math_21032, 5, 2, 0,5, 0, 2, _SimpleNoiseTexture_21034_fac, _SimpleNoiseTexture_21034_col);

	ColorOut = _SimpleNoiseTexture_21034_col;
}