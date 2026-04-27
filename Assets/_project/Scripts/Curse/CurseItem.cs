using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CurseItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Curse CurseData;
    
    public Image IconImage;
    public Text NameText;
    public Text DescriptionText;
    
    private RectTransform _rectTransform;

    public void Init(Curse curse)
    {
        CurseData = curse;

        // NameText.text = curse.DisplayName;
        // DescriptionText.text = curse.Description;
        Debug.Log("Init");
        
        IconImage = GetComponent<Image>();
        IconImage.sprite = CurseData.Icon;
        
        if (NameText != null)
            NameText.text = $"{curse.Icon} {curse.DisplayName}";
        
        if(DescriptionText != null)
            DescriptionText.text = $"{curse.Description}";
        
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
        
        if (CurseData != null)
        {
            TooltipManager.Instance.ShowTooltip(CurseData, _rectTransform);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }
}