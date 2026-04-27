using System;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePanel : MonoBehaviour
{
    public Text FoodText;
    public Text GoldText;
    public Text BloodText;

    private void Start()
    {
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
                break;
            case ResourceType.Gold:
                if (GoldText != null) GoldText.text = currentAmount.ToString();
                break;
            case ResourceType.Blood:
                if (BloodText != null) BloodText.text = currentAmount.ToString();
                break;
        }
    }
}