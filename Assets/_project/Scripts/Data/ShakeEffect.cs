using System.Collections;
using UnityEngine;

public class ShakeEffect : MonoBehaviour
{
    public static ShakeEffect Instance;
    
    private Vector3 _originalPosition;
    public float ShakeIntensity = 0.3f;
    public float Duration = 0.5f;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        
        _originalPosition = transform.position;
    }
    
    public void PlayHorrorEffect()
    {
        StartCoroutine(HorrorCoroutine());
    }
    
    private IEnumerator HorrorCoroutine()
    {
        float timer = 0;
        
        _originalPosition = transform.position;
        
        while (timer < Duration)
        {
            timer += Time.deltaTime;
            float t = timer / Duration;
            
            float shakeX = Random.Range(-ShakeIntensity, ShakeIntensity) * (1 - t);
            float shakeY = Random.Range(-ShakeIntensity, ShakeIntensity) * (1 - t);
            transform.localPosition = _originalPosition + new Vector3(shakeX, shakeY, 0);
            
            yield return null;
        }
        
        transform.position = _originalPosition;
    }
}