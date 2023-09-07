void GetDirectionalShadow_float(float3 WorldPos, out float Attenuation) {
#if defined(SHADERGRAPH_PREVIEW)
    Attenuation = 1;
#else
    float4 sCoord = TransformWorldToShadowCoord(WorldPos);
    Light light = GetMainLight(sCoord);
    Attenuation = light.shadowAttenuation;
#endif
}