using UnityEngine;
using Random = UnityEngine.Random;

public class TrialSystem : MonoBehaviour, IService
{
    public float[] TrialChances = { 0.3f, 0.5f, 0.7f, 0.9f };

    public void Init() {}
    
    public void StartTrial(int sacrificePower, ResourceType resourceType)
    {
        int sacrificeCount = GetSacrificeCount();
        int index = Mathf.Clamp(sacrificeCount / 3, 0, TrialChances.Length - 1);
        float chance = TrialChances[index];

        if (Random.value > chance)
        {
            LogSystem.Instance.AddLog("Тотем принял жертву без испытания.", Color.lightSeaGreen);
            return;
        }

        int enemyPower = GetEnemyPower(sacrificeCount);
        
        TrialChoicePopup.Instance.Show(sacrificePower, enemyPower, resourceType, OnTrialChoice);
    }

    private void OnTrialChoice(bool accepted)
    {
        if (accepted)
        {
            LogSystem.Instance.AddLog("Ты принял вызов тотема!", Color.white);
        }
        else
        {
            LogSystem.Instance.AddLog("Ты отказался от испытания. Тотем разгневан!", Color.red);
        }
    }

    public int GetEnemyPower(int sacrificeCount)
    {
        if (sacrificeCount < 3) return 15;
        if (sacrificeCount < 6) return 30;
        if (sacrificeCount < 9) return 50;
        return 70;
    }

    private int GetSacrificeCount() =>
        G.Game.Totem.TotalSacrifices;
}