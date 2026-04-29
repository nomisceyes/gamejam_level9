public static class SacrificePresets
{
    public static readonly SacrificeData Food = new()
    {
        Name = "Еда",
        Type = ResourceType.Food,
        BasePower = 10,
        FavorChange = 5,
        Amount = 8,
    };

    public static readonly SacrificeData Gold = new()
    {
        Name = "Золото",
        Type = ResourceType.Gold,
        BasePower = 15,
        FavorChange = 7,
        Amount = 10,
    };
    
    public static readonly SacrificeData Blood = new()
    {
        Name = "Кровь послушников",
        Type = ResourceType.Blood,
        BasePower = 35,
        FavorChange = 8,
        Amount = 10,
    };
    
    public static readonly SacrificeData HealthSacrifice = new()
    {
        Name = "Собственная кровь",
        Type = ResourceType.Health,
        BasePower = 40,
        FavorChange = 12,
        Amount = 0,
        HealthCost = 10,
    };
}