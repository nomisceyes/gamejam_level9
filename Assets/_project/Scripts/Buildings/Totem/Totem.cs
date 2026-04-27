using System;
using UnityEngine;

public class Totem : MonoBehaviour
{
    [Header("Balance Settings")] 
    public float CurrentFavor = 50;
    public int MaxFavor = 100;
    public int MinFavorForWin = 85;
    public int MinFavorForLose = 10;
    public float FavorDecayRate = 0.5f;

    public int TotalSacrifices = 0;
    private float _decayTimer = 0f;

    public event Action OnFavorChange;

    private void Update()
    {
        _decayTimer += Time.deltaTime;
        if (_decayTimer >= 1f)
        {
            _decayTimer = 0f;
            float decayModifier = G.CurseManager.GetModifier("favor_decay");
            int decayAmount = Mathf.RoundToInt(FavorDecayRate * decayModifier);
            
            CurrentFavor = Mathf.Max(0, CurrentFavor - decayAmount);
            OnFavorChange?.Invoke();
            
            if(CurrentFavor == 20)
                LogSystem.Instance.AddLog("⚠️ Благосклонность падает! Тотем гневается!", Color.red);
        }
    }
    
    public void MakeSacrifice(SacrificeData sacrifice)
    {
        float oldFavor = CurrentFavor;
        
        bool enoughResources = G.ResourceManager.SpendResource(sacrifice.Cost);
        if (enoughResources == false)
        {
            Debug.Log("You don't have enough resources");
            return;
        }

        if (G.CurseManager.IsCurseActive("thirst") && sacrifice.Type != ResourceType.Blood)
        {
            Debug.Log("Curse of Thirst! The totem demands blood.");
            CurrentFavor -= 15;
            OnFavorChange?.Invoke();

            G.CurseManager.RemoveCurseByName("thirst");
            return;
        }

        float powerModifier = G.CurseManager.GetModifier("sacrifice_power");
        int finalPower = Mathf.FloorToInt(sacrifice.BasePower * powerModifier);

        if (G.CurseManager.IsCurseActive("greed"))
        {
            G.ResourceManager.RemoveResource(ResourceType.Gold, 5);
        }

        float favorModifier = G.CurseManager.GetModifier("favor_gain");
        int finalFavorChange = Mathf.FloorToInt(sacrifice.FavorChange * favorModifier);
        CurrentFavor = Mathf.Clamp(CurrentFavor + finalFavorChange, 0, MaxFavor);
        OnFavorChange?.Invoke();

        TotalSacrifices++;
        G.TrialSystem.StartTrial(finalPower, sacrifice.Type);
        LogSystem.Instance.LogSacrifice(sacrifice.Name, CurrentFavor - oldFavor);
        LogSystem.Instance.LogFavorChange(oldFavor, CurrentFavor);
        //CheckRitualUnlock();
    }

    public void UpdateFavor() =>
        OnFavorChange?.Invoke();

    public float GetGatheringModifier()
    {
        if (CurrentFavor <= 10) return 0.5f;
        if (CurrentFavor <= 30) return 1f;
        if (CurrentFavor <= 60) return 1.2f;
        if (CurrentFavor <= 84) return 1.5f;
        return 1f;
    }

    public void FavorDecrease(int amount)
    {
        CurrentFavor -= amount;
        OnFavorChange?.Invoke();
    }
        

    // private void CheckRitualUnlock()
    // {
    //     if (TotalSacrifices == 3) G.RitualManager.UnlockRitual("Eclipse");
    //     if (TotalSacrifices == 5) G.RitualManager.UnlockRitual("BloodMoon");
    //     if (TotalSacrifices == 7) G.RitualManager.UnlockRitual("Whisper");
    // }
}