public static class SacrificePresets
{
    public static readonly SacrificeData Food = new()
    {
        Name = "Еда",
        Type = ResourceType.Food,
        BasePower = 15,
        FavorChange = 3,
        Amount = 8,
    };

    public static readonly SacrificeData Gold = new()
    {
        Name = "Золото",
        Type = ResourceType.Gold,
        BasePower = 23,
        FavorChange = 5,
        Amount = 10,
    };
    
    public static readonly SacrificeData Blood = new()
    {
        Name = "Кровь послушников",
        Type = ResourceType.Blood,
        BasePower = 25,
        FavorChange = 6,
        Amount = 10,
    };
    
    public static readonly SacrificeData HealthSacrifice = new()
    {
        Name = "Собственная кровь",
        Type = ResourceType.Health,
        BasePower = 35,
        FavorChange = 10,
        Amount = 0,
        HealthCost = 10,
    };
}