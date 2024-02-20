#ifndef LIGHTING_CEL_SHADED_INCLUDED
#define LIGHTING_CEL_SHADED_INCLUDED

#if !defined (SHADERGRAPH_PREVIEW)
struct EdgeConstants
{

    float diffuse;
    float specular;
    float distanceAttenuation;
    float shadowAttenuation;

};

struct SurfaceVariables
{

    float smoothness;
    float shininess;
    float3 normal;
    float3 view;

    EdgeConstants ec;

};

float3 CalculateCelShading(Light l, SurfaceVariables s)
{
    
    float diffuse = saturate(dot(s.normal, l.direction));

    float attenuation = l.distanceAttenuation * l.shadowAttenuation;
    
    diffuse *= attenuation;
   
    return l.color * diffuse;
}

float AttenuationCalculation(Light l, SurfaceVariables s)
{
    float diffuse = saturate(dot(s.normal, l.direction));
    float attenuation = l.distanceAttenuation * l.shadowAttenuation;
    
    diffuse *= attenuation;
    
    return diffuse;
}

#endif

void LightingCelShaded_float(
       
      float3 Position, float3 Normal, float3 View, out float3 Color, out float Attenuation)
{

#if defined(SHADERGRAPH_PREVIEW)
    Color = half3(0.5f, 0.5f, 0.5f);
    Attenuation = half(0.5f);
   
#else
    SurfaceVariables s;
    
    s.normal = normalize(Normal);
    s.view = SafeNormalize(View);



#if SHADOWS_SCREEN
   float4 clipPos = TransformWorldToHClip(Position);
   float4 shadowCoord = ComputeScreenPos(clipPos);
#else
    float4 shadowCoord = TransformWorldToShadowCoord(Position);
#endif

    Light light = GetMainLight(shadowCoord);
    Color = CalculateCelShading(light, s);
    
    Attenuation = AttenuationCalculation(light, s);
    float h = Attenuation * 0.5f + 0.5f;
    DotValue = h;
   
    
    int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; i++)
    {
        light = GetAdditionalLight(i, Position, 1);
        Color += CalculateCelShading(light, s);
        
        Attenuation += AttenuationCalculation(light, s);
        float h = Attenuation * 0.5f + 0.5f;
        DotValue = h;

    }
   
#endif
}

#endif