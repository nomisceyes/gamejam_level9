using UnityEngine;

public class Building : MonoBehaviour
{
    public Totem Totem;

    public ResourceType ResourceType;
    public float BaseGatherTime = 2f;
    public bool AutoGather = true;
    
    private float _currentProgress = 0f;

    private void Start()
    {
        if (Totem == null)
            Totem = G.Game.Totem;
    }

    private void Update()
    {
        if (AutoGather == false)
            return;

        float curseModifier = G.CurseManager.GetModifier("gather_speed");
        float totemModifier = Totem.GetGatheringModifier();
        
        float gatherSpeed = 1f / BaseGatherTime * totemModifier * curseModifier;
        
        _currentProgress += gatherSpeed * Time.deltaTime;
        
        if (_currentProgress >= 1f)
        {
            int amount = Mathf.FloorToInt(_currentProgress);
            _currentProgress -= amount;
            G.ResourceManager.AddResource(ResourceType, amount);
        }
    }
}