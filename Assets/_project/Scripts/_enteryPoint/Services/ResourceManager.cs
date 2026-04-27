using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour, IService
{
    public Dictionary<ResourceType, int> Resources = new();

    public event Action<ResourceType> OnResourceChanged;

    public void Init()
    {
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
            Resources[type] = 0;

        Resources[ResourceType.Food] = 3000;
        Resources[ResourceType.Gold] = 5000;
        Resources[ResourceType.Blood] = 3000;
    }

    public void AddResource(ResourceType type, int amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException("Отрицалово");

        Resources[type] += amount;
        OnResourceChanged?.Invoke(type);
    }

    public void RemoveResource(ResourceType type, int amount)
    {
        if (amount > 0)
        {
            Resources[type] -= amount;
            OnResourceChanged?.Invoke(type);
        }
    }

    public bool SpendResource(ResourceCost[] costs)
    {
        foreach (var cost in costs)
        {
            if (GetResource(cost.Type) < cost.Amount)
                return false;
        }

        foreach (var cost in costs)
        {
            Resources[cost.Type] -= cost.Amount;
            OnResourceChanged?.Invoke(cost.Type);
        }

        return true;
    }

    public int GetResource(ResourceType type) =>
        Resources[type];
}