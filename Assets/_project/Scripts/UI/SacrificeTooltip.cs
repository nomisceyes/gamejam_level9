using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SacrificeTooltip : MonoBehaviour
{
    public static SacrificeTooltip Instance;

    [Header("UI тултипа")] public GameObject TooltipPanel;
    public Text TitleText;
    public Text PowerText;
    public Text FavorText;
    public Text CostText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        TooltipPanel.SetActive(false);
    }

    public void UpdateTooltip(SacrificeData data)
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

    public void PositionTooltip(Vector3 position)
    {
        TooltipPanel.GetComponent<RectTransform>().position = new Vector3(position.x, position.y + 300, 0);
    }

    public void ShowTooltip()
    {
        TooltipPanel.SetActive(true);
    }

    public void HideTooltip()
    {
        TooltipPanel.SetActive(false);
    }
}