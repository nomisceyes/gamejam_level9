using UnityEngine;
using UnityEngine.UI;

public class SacrificeTooltip : Tooltip<SacrificeData>
{
    public Text TitleText;
    public Text PowerText;
    public Text FavorText;
    public Text CostText;

    public void Init(GameObject tooltip)
    {
        TooltipPanel = tooltip;
        TooltipPanel.SetActive(false);
        _tooltipRectTransform = TooltipPanel.GetComponent<RectTransform>();
        
        TitleText = TooltipPanel.transform.Find("Title").GetComponent<Text>();
        PowerText = TooltipPanel.transform.GetChild(1).GetComponent<Text>();
        FavorText = TooltipPanel.transform.GetChild(2).GetComponent<Text>();
        CostText = TooltipPanel.transform.GetChild(3).GetComponent<Text>();

        offsetX = 0f;
        offsetY = 230f;
    }

    public override void UpdateTooltip(SacrificeData data)
    {
        if (data == null) return;

        int currentPower = CalculateCurrentPower(data);
        int currentFavor = CalculateCurrentFavor(data);

        if (currentPower == 0 || currentFavor == 0)
        {
            currentPower = data.BasePower;
            currentFavor = data.FavorChange;
        }

        TitleText.text = data.Name;
        PowerText.text = $"Сила: {currentPower}";
        FavorText.text = $"Благосклонность: {currentFavor}";

        if (data.Amount == 0)
            CostText.gameObject.SetActive(false);
        else
            CostText.gameObject.SetActive(true);

        CostText.text = $"Стоимость: {data.Amount}";
    }

    private int CalculateCurrentPower(SacrificeData data)
    {
        float powerModifier = G.Game.Totem.PowerModifier;
        return Mathf.FloorToInt(data.BasePower * powerModifier);
    }

    private int CalculateCurrentFavor(SacrificeData data)
    {
        float favorModifier = G.Game.Totem.FavorModifier;
        return Mathf.FloorToInt(data.FavorChange * favorModifier);
    }
}