using UnityEngine;

public class Building : MonoBehaviour
{
    public Totem Totem;

    public ResourceType ResourceType;
    public float BaseGatherPerSecond = 1f;
    public bool AutoGather = true;

    private float _timer = 0f;

    private void Start()
    {
        if (Totem == null)
            Totem = G.Game.Totem;
    }

    private void Update()
    {
        if (AutoGather == false)
            return;

        _timer += Time.deltaTime;
        if (_timer >= 1f)
        {
            _timer = 0f;
            GatherResource();
        }
    }

    public void GatherResource()
    {
        float curseModifier = G.CurseManager.GetModifier("gather_speed");
        float totemModifier = Totem.GetGatheringModifier();
        int amount = Mathf.FloorToInt(BaseGatherPerSecond * totemModifier * curseModifier);

        if (amount > 0)
            G.ResourceManager.AddResource(ResourceType, amount);

        if (curseModifier < 0.99f)
            ShowCurseEffect();
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