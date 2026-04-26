public class BloodAltar : Building
{
    private void Start()
    {
        ResourceType = ResourceType.Blood;
        BaseGatherPerSecond = 0f;
        AutoGather = false;
    }

    public void SacrificeHealth(int hpToConvert)
    {
        G.ResourceManager.AddResource(ResourceType.Blood, hpToConvert);
        
        //PlayerHealth.TakeDamage(hpToConvert);
    }
}