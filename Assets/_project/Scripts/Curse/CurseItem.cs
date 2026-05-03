using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CurseItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Curse CurseData;
    public Image IconImage;

    public void Init(Curse curse)
    {
        CurseData = curse;

        IconImage = GetComponent<Image>();
        IconImage.sprite = CurseData.Icon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        G.TooltipManager.CurseTooltip.UpdateTooltip(CurseData);
        G.TooltipManager.CurseTooltip.ShowTooltip(transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        G.TooltipManager.CurseTooltip.HideTooltip();
    }
}