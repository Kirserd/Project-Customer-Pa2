void ComputeLights_float(float3 worldPos, float3 normal, out float3 finalColor, out float Attenuation) {
#if defined(SHADERGRAPH_PREVIEW)
    Attenuation = 1;
    finalColor = half3(0, 0, 0);
#else
    finalColor = half3(0, 0, 0);
    
    float4 sCoord = TransformWorldToShadowCoord(worldPos);
    Light mainLight = GetMainLight(sCoord);
    finalColor += mainLight.color;
    Attenuation = mainLight.shadowAttenuation;

    int numAdditionalLights = GetAdditionalLightsCount();

    for (int i = 0; i < numAdditionalLights; i++) {
        Light light = GetAdditionalLight(i, worldPos);
        float3 radiance = light.color * (light.distanceAttenuation * light.shadowAttenuation);
        float diffuse = saturate(dot(normal, light.direction));
        Attenuation += light.distanceAttenuation * light.shadowAttenuation;
        finalColor += radiance * diffuse;
    }
#endif
}