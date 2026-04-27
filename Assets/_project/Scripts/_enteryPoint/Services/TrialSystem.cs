using UnityEngine;
using Random = UnityEngine.Random;

public class TrialSystem : MonoBehaviour, IService
{
    public float[] TrialChances = { 0.3f, 0.5f, 0.7f, 0.9f };

    public void Init()
    {
        
    }

    public void StartTrial(int sacrificePower, ResourceType resourceType)
    {
        int sacrificeCount = GetSicrificeCount();
        int index = Mathf.Clamp(sacrificeCount / 3, 0 , TrialChances.Length - 1);
        // float chance = TrialChances[index];

        // if (Random.value > chance)
        // {
        //     Debug.Log("Fail");
        //     return;
        // }

        int enemyPower = GetEnemyPower(sacrificeCount);
        sacrificePower = 0;
        bool success = sacrificePower > enemyPower;

        if (success)
        {
            G.ResourceManager.AddResource(ResourceType.Food, 10 + sacrificeCount * 5);
            G.ResourceManager.AddResource(ResourceType.Gold, 5 + sacrificeCount * 3);
            G.Game.Totem.CurrentFavor += 8;
            Debug.Log("Win in trial");
        }
        else
        {
            G.ResourceManager.AddResource(ResourceType.Food, 5);
            ApplyRandomCurse();
            Debug.Log("Lose in trial");
        }
    }

    private int GetEnemyPower(int sacrificeCount)
    {
        if (sacrificeCount < 3) return 15;
        if (sacrificeCount < 6) return 30;
        if (sacrificeCount < 9) return 50;
        return 70;
    }

    private void ApplyRandomCurse()
    {
        string[] possibleCurses = { "rot", "eye", "silence", "greed" };
        string curseId = possibleCurses[Random.Range(0, possibleCurses.Length)];
        float duration = Random.Range(30f, 90f);
    
        G.CurseManager.ApplyCurse(curseId, duration);
    
        Debug.Log($"Тотем насылает проклятие: {curseId} на {duration} сек");
    }

    private int GetSicrificeCount() => G.Game.Totem.TotalSacrifices;
}