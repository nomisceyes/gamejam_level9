using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SacrificeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ResourceType ResourceType;
    public Button btn;
    public SacrificeData Data;

    private void Start()
    {
        Data = ResourceType switch
        {
            ResourceType.Food => SacrificePresets.Food,
            ResourceType.Gold => SacrificePresets.Gold,
            ResourceType.Blood => SacrificePresets.Blood,
            ResourceType.Health => SacrificePresets.HealthSacrifice,
            _ => Data
        };

        btn.onClick.AddListener(() => { G.Game.Totem.MakeSacrifice(Data); });
        G.TooltipManager.SacrificeTooltip.UpdateTooltip(Data);
    }
    
    private void Update()
    {
        if (ResourceType == ResourceType.Health)
        {
            bool isThirstActive = G.CurseManager.IsCurseActive("thirst");

            if (isThirstActive)
                btn.targetGraphic.color = Color.red;
            else
                btn.targetGraphic.color = Color.white;
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        G.TooltipManager.SacrificeTooltip.UpdateTooltip(Data);
        G.TooltipManager.SacrificeTooltip.ShowTooltip(transform.position);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        G.TooltipManager.SacrificeTooltip.HideTooltip();
    }
}