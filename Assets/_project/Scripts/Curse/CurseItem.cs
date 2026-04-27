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
        
        IconImage = GetComponent<Image>();
        IconImage.sprite = CurseData.Icon;
        
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