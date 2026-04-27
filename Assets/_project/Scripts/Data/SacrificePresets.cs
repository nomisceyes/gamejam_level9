using UnityEngine;

public static class SacrificePresets
{
    public static SacrificeData Food = new SacrificeData
    {
        Name = "Food",
        Type = ResourceType.Food,
        BasePower = 10,
        FavorChange = 5,
        Cost = new[] { new ResourceCost {Type = ResourceType.Food, Amount = 20}}
    };

    public static SacrificeData Gold = new SacrificeData
    {
        Name = "Gold",
        Type = ResourceType.Gold,
        BasePower = 15,
        FavorChange = 8,
        Cost = new[] { new ResourceCost { Type = ResourceType.Gold, Amount = 25 } }
    };
    
    public static SacrificeData Blood = new SacrificeData
    {
        Name = "Blood",
        Type = ResourceType.Blood,
        BasePower = 35,
        FavorChange = 15,
        Cost = new[]
        {
            new ResourceCost {Type = ResourceType.Blood, Amount = 30}
        },
        
        Action = () =>
        {
            Health.Instance.TakeDamage(10);
            Debug.Log("Вы пожертвовали здоровьем");
        }
    };
}