using System;

[Serializable]
public class SacrificeData : ITooltip<SacrificeData>
{
    public string Name;
    public ResourceType Type;
    public int BasePower;
    public int FavorChange;
    public int Amount;
    public int HealthCost;
}