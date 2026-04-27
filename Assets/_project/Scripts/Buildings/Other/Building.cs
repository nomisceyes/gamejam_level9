using UnityEngine;

public class Building : MonoBehaviour
{
    public Totem Totem;

    public ResourceType ResourceType;
    public float BaseGatherTime = 2f;
    public float BaseGatherPerSecond = 1f;
    public bool AutoGather = true;

    private float _timer = 0f;
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

    public void GatherResource()
    {
        float curseModifier = G.CurseManager.GetModifier("gather_speed");
        float totemModifier = Totem.GetGatheringModifier();
        int amount = Mathf.FloorToInt(BaseGatherPerSecond * totemModifier * curseModifier);

        if (amount > 0)
        {
            G.ResourceManager.AddResource(ResourceType, amount);
            Debug.Log($"[{ResourceType}] Собрано {amount}. Модификаторы: тотем={totemModifier}, проклятия={curseModifier}");
        }
    }

    public void ManualGather(int multiplier = 1)
    {
        float modifier = Totem.GetGatheringModifier();
        int amount = Mathf.FloorToInt(BaseGatherPerSecond * modifier * multiplier);
        G.ResourceManager.AddResource(ResourceType, amount);
    }

    private void ShowCurseEffect()
    {
        GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.8f, 0.7f);
        Invoke(nameof(ResetColor), 0.5f);
    }

    private void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}