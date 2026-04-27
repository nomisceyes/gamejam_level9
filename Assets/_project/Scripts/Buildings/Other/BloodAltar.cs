using UnityEngine;

public class BloodAltar : Building
{
    public int BloodAmount = 3;
    
    private void Start()
    {
        ResourceType = ResourceType.Blood;
        BaseGatherPerSecond = 0f;
        AutoGather = false;
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicked on blood altar");
        SacrificeHealth(BloodAmount);
    }
    
    public void SacrificeHealth(int hpToConvert)
    {
        G.ResourceManager.AddResource(ResourceType.Blood, hpToConvert);
        
        //PlayerHealth.TakeDamage(hpToConvert);
    }
}