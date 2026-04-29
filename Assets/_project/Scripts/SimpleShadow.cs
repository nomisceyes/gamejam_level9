using UnityEngine;

public class SimpleShadow : MonoBehaviour
{
    public Vector2 ShadowOffset = new Vector2(0.5f, -0.5f);
    public Color ShadowColor = new Color(0, 0, 0, 0.5f);
    public float ShadowScale = 0.8f;
    
    private SpriteRenderer _shadowRenderer;
    private SpriteRenderer _parentRenderer;
    
    private void Start()
    {
        CreateShadow();
    }
    
    private void CreateShadow()
    {
        GameObject shadowObj = new("Shadow");
        shadowObj.transform.SetParent(transform);
        shadowObj.transform.localPosition = ShadowOffset;
        shadowObj.transform.localScale = Vector3.one * ShadowScale;
        
        _shadowRenderer = shadowObj.AddComponent<SpriteRenderer>();
        _parentRenderer = GetComponent<SpriteRenderer>();
        
        if (_parentRenderer != null)
        {
            _shadowRenderer.sprite = _parentRenderer.sprite;
            _shadowRenderer.color = ShadowColor;
            _shadowRenderer.sortingOrder = _parentRenderer.sortingOrder - 1;
        }
    }
    
    private void Update()
    {
        if (_parentRenderer != null && _shadowRenderer != null)
        {
            if (_shadowRenderer.sprite != _parentRenderer.sprite)
                _shadowRenderer.sprite = _parentRenderer.sprite;
        }
    }
}