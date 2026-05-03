using UnityEngine;
using UnityEngine.UI;

public class CurseTooltip : Tooltip<Curse>
{
    private Text _tooltipTitle;
    private Text _tooltipDescription;

    public void Init(GameObject tooltip)
    {
        TooltipPanel = tooltip;
        TooltipPanel.SetActive(false);
        _tooltipRectTransform = TooltipPanel.GetComponent<RectTransform>();
        
        _tooltipTitle = TooltipPanel.transform.Find("Title").GetComponent<Text>();
        _tooltipDescription = TooltipPanel.transform.Find("Description").GetComponent<Text>();

        offsetX = 200f;
        offsetY = -180f;
    }
    
    public override void UpdateTooltip(Curse curse)
    {
        if (TooltipPanel == null)
        {
            Debug.LogWarning("Tooltip Panel не назначен!");
            return;
        }

        if (_tooltipTitle != null)
            _tooltipTitle.text = $"{curse.DisplayName}";

        if (_tooltipDescription != null)
            _tooltipDescription.text = curse.Description;
    }
}