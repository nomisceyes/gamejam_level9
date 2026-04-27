using System;

[System.Serializable]
public class SacrificeData
{
    public string Name;
    public ResourceType Type;
    public int BasePower;
    public int FavorChange;
    public ResourceCost[] Cost;
    public Action Action;
}