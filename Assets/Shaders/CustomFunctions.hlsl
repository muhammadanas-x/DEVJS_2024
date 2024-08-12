//UNITY_SHADER_NO_UPGRADE
#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED


float Hash(float2 UV) {
    return frac(
        sin(
            dot(
                UV,
                float2(12.9898,78.233)
            )
        ) * 43758.5453123
    );
}

float HashNoSine(float2 UV) {
    float3 p3 = frac(float3(UV.xyx) * .1031);
    p3 += dot(p3, p3.yzx + 33.33);
    return frac((p3.x + p3.y) * p3.z);
}

float Noise(float2 UV) {
    float2 i = floor(UV);
    float2 f = frac(UV);

    float a = HashNoSine(i);
    float b = HashNoSine(i + float2(1, 0));
    float c = HashNoSine(i + float2(0, 1));
    float d = HashNoSine(i + float2(1, 1));

    float2 u = f * f * (3 - 2 * f);

    return lerp(a, b, u.x) +
        (c - a)* u.y * (1.0 - u.x) +
        (d - b) * u.x * u.y;
}

void FractalBrownianMotion_float(float2 UV, out float Out) {
    float value = 0;
    float amplitude = .5;
    float frequency = 0;

    for (int i = 0; i < _Octaves; i++) {
        value += amplitude * Noise(UV);
        UV *= 2;
        amplitude *= .5;
    }
    
    Out = value;
}
#endif //MYHLSLINCLUDE_INCLUDED