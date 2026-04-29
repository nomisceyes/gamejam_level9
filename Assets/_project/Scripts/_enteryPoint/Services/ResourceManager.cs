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

        Resources[ResourceType.Food] = 20;
        Resources[ResourceType.Gold] = 20;
        Resources[ResourceType.Blood] = 0;
    }

    public void AddResource(ResourceType type, int amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException("Отрицалово");

        Resources[type] += amount;
        OnResourceChanged?.Invoke(type);
    }

    public void RemoveResource(ResourceType type, int amount)
    {
        Resources[type] = Mathf.Max(0, Resources[type] - amount);
        OnResourceChanged?.Invoke(type);
    }

    public bool SpendResource(SacrificeData data)
    {
        if (GetResource(data.Type) < data.Amount)
            return false;
        
        Resources[data.Type] = Mathf.Max(0, Resources[data.Type] - data.Amount);
        OnResourceChanged?.Invoke(data.Type);

        return true;
    }

    public int GetResource(ResourceType type) =>
        Resources[type];

    public void ResetResource()
    {
        Resources[ResourceType.Food] = 20;
        Resources[ResourceType.Gold] = 20;
        Resources[ResourceType.Blood] = 0;

        foreach (var type in Enum.GetValues(typeof(ResourceType)))
            OnResourceChanged?.Invoke((ResourceType)type);
    }
}