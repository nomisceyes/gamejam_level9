using Unity.VisualScripting;
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
        int sacrificeCount = GetSacrificeCount();
        int index = Mathf.Clamp(sacrificeCount / 3, 0, TrialChances.Length - 1);
        float chance = TrialChances[index];

        if (Random.value > chance)
        {
            LogSystem.Instance.AddLog("Тотем принял жертву без испытания.", Color.gray);
            return;
        }

        int enemyPower = GetEnemyPower(sacrificeCount);
        
        TrialChoicePopup.Instance.Show(sacrificePower, enemyPower, resourceType, OnTrialChoice);
        // TrialPopup.Instance.ShowTrial(sacrificePower, enemyPower);
        //
        // float winChance = CalculateWinChance(sacrificePower, enemyPower);
        // bool success = sacrificePower > enemyPower;
        //
        // if (success)
        // {
        //     ApplyTrialSuccess(resourceType);
        // }
        // else
        // {
        //     ApplyTrialFailure(resourceType);
        // }
        //
        // LogSystem.Instance.LogTrialResult(success, sacrificePower, enemyPower);
    }

    private void OnTrialChoice(bool accepted)
    {
        if (accepted)
        {
            // Игрок принял испытание — результат уже обработан в TrialChoicePopup
            LogSystem.Instance.AddLog("Ты принял вызов тотема!", Color.white, "⚔️");
        }
        else
        {
            // Игрок отказался — штраф уже применён
            LogSystem.Instance.AddLog("Ты отказался от испытания. Тотем разгневан!", Color.red, "😨");
        }
    }
    
    private void ApplyTrialSuccess(ResourceType type)
    {
        // Награда
        int foodGain = 15;
        int goldGain = 10;

        G.ResourceManager.AddResource(ResourceType.Food, foodGain);
        G.ResourceManager.AddResource(ResourceType.Gold, goldGain);
        G.Game.Totem.CurrentFavor += 8;

        LogSystem.Instance.AddLog($"Тотем вознаграждает за победу! +{foodGain} еды, +{goldGain} золота",
            Color.green, "🎁");
    }

    private void ApplyTrialFailure(ResourceType type)
    {
        // Штраф
        int loss = 10;
        G.ResourceManager.AddResource(type, -loss);

        // Проклятие
        ApplyRandomCurse();
        
        LogSystem.Instance.AddLog($"Поражение! -{loss} {type.DisplayName()} и проклятие!",
            Color.red, "💀");
    }

    private float CalculateWinChance(int sacrificePower, int enemyPower)
    {
        float rawChance = (float)sacrificePower / enemyPower;
        return Mathf.Clamp(rawChance, 0.1f, 0.9f);
    }

    public int GetEnemyPower(int sacrificeCount)
    {
        if (sacrificeCount < 3) return 15;
        if (sacrificeCount < 6) return 30;
        if (sacrificeCount < 9) return 50;
        return 70;
    }

    private void ApplyRandomCurse()
    {
        string[] possibleCurses = { "rot", "eye", "thirst" };
        string curseId = possibleCurses[Random.Range(0, possibleCurses.Length)];
        float duration = Random.Range(30f, 90f);

        G.CurseManager.ApplyCurse(curseId, duration);
        Debug.Log($"Тотем насылает проклятие: {curseId} на {duration} сек");
    }

    private int GetSacrificeCount() =>
        G.Game.Totem.TotalSacrifices;
}