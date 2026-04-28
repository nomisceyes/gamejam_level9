public static class SacrificePresets
{
    public static readonly SacrificeData Food = new()
    {
        Name = "Food",
        Type = ResourceType.Food,
        BasePower = 10,
        FavorChange = 5,
        Cost = new[] { new ResourceCost {Type = ResourceType.Food, Amount = 8}}
    };

    public static readonly SacrificeData Gold = new()
    {
        Name = "Gold",
        Type = ResourceType.Gold,
        BasePower = 15,
        FavorChange = 7,
        Cost = new[] { new ResourceCost { Type = ResourceType.Gold, Amount = 10 } }
    };
    
    public static readonly SacrificeData Blood = new()
    {
        Name = "Blood",
        Type = ResourceType.Blood,
        BasePower = 35,
        FavorChange = 10,
        Cost = new[] { new ResourceCost {Type = ResourceType.Blood, Amount = 10} }
    };
}