float4 ApplyAdditionalLights_float(float4 color, float3 worldPos, float3 normal, out float4 finalColor) {
#if defined(SHADERGRAPH_PREVIEW)
    finalColor = color; 
    return finalColor;
#else
    finalColor = color;
    int numAdditionalLights = GetAdditionalLightsCount();

    for (int i = 0; i < numAdditionalLights; i++) {
        Light additionalLight = GetAdditionalLight(i, worldPos);

        float3 lightDirection = normalize(additionalLight.direction);
        float NDotL = saturate(dot(normal, lightDirection));
        float3 diffuse = NDotL * additionalLight.color;

        finalColor.rgb += diffuse * color.rgb;
    }
    return finalColor;
#endif
}