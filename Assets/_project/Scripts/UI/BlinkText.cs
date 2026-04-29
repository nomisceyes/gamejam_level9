using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlinkText : MonoBehaviour
{
    [Header("Настройки мигания")]
    public float BlinkSpeed = 1f;
    public float MinAlpha = 0.3f;
    public float MaxAlpha = 1f;
    
    private Text _textComponent;
    private Color _originalColor;
    
    private void Start()
    {
        _textComponent = GetComponent<Text>();
        if (_textComponent != null)
        {
            _originalColor = _textComponent.color;
            StartCoroutine(BlinkCoroutine());
        }
    }
    
    private IEnumerator BlinkCoroutine()
    {
        while (true)
        {
            // Плавное изменение прозрачности
            float alpha = MinAlpha + (Mathf.Sin(Time.time * BlinkSpeed) + 1f) / 2f * (MaxAlpha - MinAlpha);
            Color newColor = _originalColor;
            newColor.a = alpha;
            _textComponent.color = newColor;
            
            yield return null;
        }
    }
}
