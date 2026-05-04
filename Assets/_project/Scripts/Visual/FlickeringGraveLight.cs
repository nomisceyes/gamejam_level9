using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickeringGraveLight : MonoBehaviour
{
    [SerializeField] private Color _lightColor = new(0.2f, 0.9f, 0.2f);
    [SerializeField] private float _baseIntensity = 0.5f;
    [SerializeField] private float _flickerAmount = 0.3f;
    [SerializeField] private float _flickerSpeed = 5f;
    [SerializeField] private float _radius = 2.5f;

    private Light2D _light2D;
    private float _randomOffset;

    private void Start()
    {
        _light2D = GetComponent<Light2D>();
        _light2D.lightType = Light2D.LightType.Point;
        _light2D.color = _lightColor;
        _light2D.pointLightOuterRadius = _radius;
        _light2D.falloffIntensity = 1.5f;

        _randomOffset = Random.Range(0f, 100f);

        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        WaitForSeconds wait = new(0.05f);

        while (enabled)
        {
            float noise = Mathf.PerlinNoise(Time.time * _flickerSpeed, _randomOffset);
            float flicker = (noise - 0.5f) * _flickerAmount;
            
            _light2D.intensity = Mathf.Max(0.1f, _baseIntensity + flicker);
            yield return wait;
        }
    }
}