void GetLight_float(float3 WorldPos, out float3 Direction, out float3 Color, out float Attenuation) {
#if defined(SHADERGRAPH_PREVIEW)
    Direction = half3(0.5, 0.5, 0);
    Color = 1;
    Attenuation = 1;
#else
    float4 sCoord = TransformWorldToShadowCoord(WorldPos);
    Light light = GetMainLight(sCoord);
    Direction = light.direction;
    Color = light.color;
    Attenuation = light.shadowAttenuation;
#endif
}