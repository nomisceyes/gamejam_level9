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
        if (ResourceType == ResourceType.Food)
            Data = SacrificePresets.Food;
        else if (ResourceType == ResourceType.Gold)
            Data = SacrificePresets.Gold;
        else if(ResourceType == ResourceType.Blood)
            Data = SacrificePresets.Blood;
        else if(ResourceType == ResourceType.Health)
            Data = SacrificePresets.HealthSacrifice;
        
        btn.onClick.AddListener(() => { G.Game.Totem.MakeSacrifice(Data); });
        SacrificeTooltip.Instance.UpdateTooltip(Data);
    }
    
    private void Update()
    {
        if (ResourceType == ResourceType.Health)
        {
            bool isThirstActive = G.CurseManager.IsCurseActive("thirst");

            if (isThirstActive)
            {
                btn.targetGraphic.color = Color.red;
            }
            else
            {
                btn.targetGraphic.color = Color.white;
            }
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        SacrificeTooltip.Instance.UpdateTooltip(Data);
        SacrificeTooltip.Instance.ShowTooltip();
        
        SacrificeTooltip.Instance.PositionTooltip(eventData.position);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        SacrificeTooltip.Instance.HideTooltip();
    }
}