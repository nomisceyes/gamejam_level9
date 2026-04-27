using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour, IService
{
    public static TooltipManager Instance;

    public GameObject TooltipPanel;
    public Text TooltipTitle;
    public Text TooltipDescription;
    //public Text TooltipTimer;

    public Vector2 Offset = new Vector2(20f, -20f);
    public float DelayBeforeShow = 0.2f;

    private Curse _pendingCurse;
    private RectTransform _targetIcon;
    private float _currentDelay = 0f;
    private bool _isHovering = false;
    private bool _isVisible = false;

    public void Init()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);


        //TooltipPanel = Resources.Load<GameObject>("Prefabs/TooltipPanel");

        if (TooltipPanel != null)
            TooltipPanel.SetActive(false);
    }

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (TooltipPanel != null)
            TooltipPanel.SetActive(false);
    }

    private void Update()
    {
        if (_isHovering && _pendingCurse != null)
        {
            _currentDelay += Time.deltaTime;
            if (_currentDelay >= DelayBeforeShow)
            {
                ShowTooltipImmediately(_pendingCurse);
                _isVisible = true;
                _isHovering = false;
            }
        }
    }

    public void ShowTooltip(Curse curse, RectTransform targetIcon)
    {
        _pendingCurse = curse;
        _targetIcon = targetIcon;
        _isHovering = true;
        _currentDelay = 0f;
    }

    private void ShowTooltipImmediately(Curse curse)
    {
        if (TooltipPanel == null)
        {
            Debug.LogWarning("Tooltip Panel не назначен!");
            return;
        }

        if (TooltipTitle != null)
            TooltipTitle.text = $"{curse.Icon} {curse.DisplayName}";

        if (TooltipDescription != null)
            TooltipDescription.text = curse.Description;

        PositionTooltipNearIcon(_targetIcon);
        
        TooltipPanel.SetActive(true);
        // if (TooltipTimer != null)
        //     TooltipTimer.text = $"Осталось: {Mathf.CeilToInt(curse.RemainingTime)}с";
    }

    private void PositionTooltipNearIcon(RectTransform targetIcon)
    {
        if (targetIcon == null || TooltipPanel == null) return;
        
        Vector3 iconWorldPos = targetIcon.position;
        Vector2 iconScreenPos = RectTransformUtility.WorldToScreenPoint(null, iconWorldPos);
        
        TooltipPanel.transform.position = iconScreenPos + new Vector2(targetIcon.rect.width + 120f, 180f);
    }

    public void HideTooltip()
    {
        _isHovering = false;
        _pendingCurse = null;
        _targetIcon = null;
        _currentDelay = 0f;
        _isVisible = false;

        if (TooltipPanel != null)
            TooltipPanel.SetActive(false);
    }
}