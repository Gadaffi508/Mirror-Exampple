#include <Packages/com.blendernodesgraph.core/Editor/Includes/Importers.hlsl>

void MagicTextureGraph_float(float3 _POS, float3 _PVS, float3 _PWS, float3 _NOS, float3 _NVS, float3 _NWS, float3 _NTS, float3 _TWS, float3 _BTWS, float3 _UV, float3 _SP, float3 _VVS, float3 _VWS, out float4 ColorOut)
{
	
	float _Math_39410 = math_multiply(_Time, 2, 0.5);
	float4 _CombineXYZ_39412 = float4(combine_xyz(0, _Math_39410, 0), 0);
	float _Math_39404 = math_multiply(_Time, 60, 0.5);
	float4 _CombineXYZ_39414 = float4(combine_xyz(0, _Math_39404, 0), 0);
	float4 _Mapping_39406 = float4(mapping_point(float4(_POS, 0), _CombineXYZ_39412, _CombineXYZ_39414, float3(1, 1, 1)), 0);
	float _MagicTexture_39416_fac; float4 _MagicTexture_39416_col; node_tex_magic(_Mapping_39406, 2, 1, 8, _MagicTexture_39416_fac, _MagicTexture_39416_col);

	ColorOut = _MagicTexture_39416_col;
}