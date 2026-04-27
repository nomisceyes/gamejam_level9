using System;
using UnityEngine;

public class Totem : MonoBehaviour
{
    [Header("Balance Settings")] 
    public int CurrentFavor = 50;
    public int MaxFavor = 100;
    public int MinFavorForWin = 85;
    public int MinFavorForLose = 10;
    
    public event Action OnFavorChange;
    public event Action OnTotemWin;
    public event Action OnTotemLose;

    public int TotalSacrifices = 0;
    
    private void Start() =>
        CheckWinLoseCondition();

    public void MakeSacrifice(SacrificeData sacrifice)
    {
        if (CurrentFavor <= MinFavorForLose || CurrentFavor >= MinFavorForWin)
        {
            Debug.Log("Game is end");
            return;
        }

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
            int goldToLose = 5;
            G.ResourceManager.AddResource(ResourceType.Gold, -goldToLose);
            Debug.Log($"Greed: You lost {goldToLose} gold");
        }
        
        float favorModifier = G.CurseManager.GetModifier("favor_gain");     
        int finalFavorChange = Mathf.FloorToInt(sacrifice.FavorChange * favorModifier);
        CurrentFavor = Mathf.Clamp(CurrentFavor + finalFavorChange, 0, MaxFavor);
        OnFavorChange?.Invoke();
        
        //int sacrificePower = sacrifice.BasePower;
        
        TotalSacrifices++;
        G.TrialSystem.StartTrial(finalPower, sacrifice.Type);
        //CheckRitualUnlock();
        CheckWinLoseCondition();
    }

    public float GetGatheringModifier()
    {
        if (CurrentFavor <= 10) return 0.5f;
        if (CurrentFavor <= 30) return 1f;
        if(CurrentFavor <= 60) return 1.2f;
        if (CurrentFavor <= 84) return 1.5f;
        return 1f;
    }
    
    // private void CheckRitualUnlock()
    // {
    //     if (TotalSacrifices == 3) G.RitualManager.UnlockRitual("Eclipse");
    //     if (TotalSacrifices == 5) G.RitualManager.UnlockRitual("BloodMoon");
    //     if (TotalSacrifices == 7) G.RitualManager.UnlockRitual("Whisper");
    // }

    private void CheckWinLoseCondition()
    {
        if (CurrentFavor >= MinFavorForWin)
        {
            OnTotemWin?.Invoke();
            Debug.Log("Win!");
        }
        else if (CurrentFavor <= MinFavorForLose)
        {
            OnTotemLose?.Invoke();
            Debug.Log("Lose!");
        }
    }
}