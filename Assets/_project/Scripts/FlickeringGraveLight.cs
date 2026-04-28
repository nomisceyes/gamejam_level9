using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickeringGraveLight : MonoBehaviour
{
    public Color LightColor = new (0.2f, 0.9f, 0.2f);
    public float BaseIntensity = 0.5f;
    public float FlickerAmount = 0.3f;
    public float FlickerSpeed = 5f;
    public float Radius = 2.5f;
    
    private Light2D _light2D;
    private float _randomOffset;
    
    private void Start()
    {
        _light2D = GetComponent<Light2D>();
        _light2D.lightType = Light2D.LightType.Point;
        _light2D.color = LightColor;
        _light2D.pointLightOuterRadius = Radius;
        _light2D.falloffIntensity = 1.5f;
        
        _randomOffset = Random.Range(0f, 100f);
    }
    
    private void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * FlickerSpeed, _randomOffset);
        float flicker = (noise - 0.5f) * FlickerAmount;
        _light2D.intensity = Mathf.Max(0.1f, BaseIntensity + flicker);
    }
}
