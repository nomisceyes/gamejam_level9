using System;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePanel : MonoBehaviour
{
    public Text FoodText;
    public Text GoldText;
    public Text BloodText;
    public Text GriefText;
    // public Text MemoryText;

    private void Start()
    {
        Debug.Log("Subsribe");
        G.ResourceManager.OnResourceChanged += UpdateResourceUI;

        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            UpdateResourceUI(type);
        }
    }

    private void UpdateResourceUI(ResourceType type)
    {
        int currentAmount = G.ResourceManager.GetResource(type);

        switch (type)
        {
            case ResourceType.Food:
                if (FoodText != null) FoodText.text = currentAmount.ToString();
                //if (FoodSlider != null) FoodSlider.value = amount;
                break;
            case ResourceType.Gold:
                if (GoldText != null) GoldText.text = currentAmount.ToString();
                break;
            case ResourceType.Blood:
                if (BloodText != null) BloodText.text = currentAmount.ToString();
                //if (bloodSlider != null) bloodSlider.value = amount;
                break;
            // case ResourceType.Grief:
            //     if (GriefText != null) GriefText.text = currentAmount.ToString();
            //     break;
            // case ResourceType.Memory:
            //     if (memoryText != null) memoryText.text = $"{amount}";
            //     break;
        }
    }
}