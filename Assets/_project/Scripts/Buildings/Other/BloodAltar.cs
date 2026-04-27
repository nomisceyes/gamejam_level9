using UnityEngine;
using UnityEngine.UI;

public class BloodAltar : Building
{
    public int BloodPerSacrifice = 5;  
    public int HealthCostPerUse = 10;
    
    public Button collectBloodButton;
    
    private void Start()
    {
        ResourceType = ResourceType.Blood;
        BaseGatherPerSecond = 0f; 
        AutoGather = false;
        
        if (collectBloodButton != null)
            collectBloodButton.onClick.AddListener(CollectBloodManually);
    }

    public void CollectBloodManually()
    {
        if (Health.Instance.HasEnoughHealth(HealthCostPerUse) == false)
        {
            Debug.Log($"❌ Не хватает здоровья! Нужно {HealthCostPerUse} HP");
            return;
        }

        Health.Instance.TakeDamage(HealthCostPerUse);
        G.ResourceManager.AddResource(ResourceType.Blood, BloodPerSacrifice);
        G.Game.Totem.MakeSacrifice(SacrificePresets.Blood);
        
        Debug.Log($"🩸 Пожертвовано {HealthCostPerUse} HP → получено {BloodPerSacrifice} крови");
        
        ShowBloodCollectionEffect();
    }
    
    private void ShowBloodCollectionEffect()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke(nameof(ResetColor), 0.3f);
    }
    
    private void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}