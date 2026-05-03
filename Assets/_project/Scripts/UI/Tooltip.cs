using UnityEngine;

public class Tooltip<TTooltip> : MonoBehaviour where TTooltip : ITooltip<TTooltip>
{
    protected GameObject TooltipPanel;
    protected RectTransform _tooltipRectTransform;
    
    protected float offsetX;
    protected float offsetY;

    public virtual void UpdateTooltip(TTooltip obj) {}
    
    public void ShowTooltip(Vector2 targetIcon)
    {
        TooltipPanel.gameObject.SetActive(true);
        PositionTooltip(targetIcon);
    }

    public void HideTooltip() =>
        TooltipPanel.SetActive(false);
    
    private void PositionTooltip(Vector2 targetIcon)
    {
        TooltipPanel.transform.position = targetIcon;

        _tooltipRectTransform.anchoredPosition = new Vector2(
            _tooltipRectTransform.anchoredPosition.x + offsetX,
            _tooltipRectTransform.anchoredPosition.y + offsetY);
    }
}

