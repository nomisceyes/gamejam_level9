using System;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour
{
    [Header("Balance Settings")] public float CurrentFavor = 50;
    public int MaxFavor = 100;
    public int MinFavorForWin = 100;
    public int MinFavorForLose = 0;
    public float FavorDecayRate = 0.5f;

    public int TotalSacrifices = 0;
    private int _maxSacrificesHistory = 3;
    private float _repetitionPenalty = 0.7f;
    private float _maxPenalty = 0.3f;
    private float _decayTimer = 0f;

    private Queue<ResourceType> _recentSacrifices = new();
    private Dictionary<ResourceType, int> _repetitionCount = new();

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

            if (Mathf.Approximately(CurrentFavor, 20))
                LogSystem.Instance.AddLog("⚠️ Благосклонность падает! Тотем гневается!", Color.red);
        }
    }

    public void MakeSacrifice(SacrificeData sacrifice)
    {
        float oldFavor = CurrentFavor;

        if (sacrifice.Type == ResourceType.Health)
        {
            Health.Instance.TakeDamage(sacrifice.HealthCost);
        }
        else
        {
            bool enoughResources = G.ResourceManager.SpendResource(sacrifice.Cost);
            if (enoughResources == false)
            {
                Debug.Log("You don't have enough resources");
                return;
            }
        }

        float repetitionModifier = CalculateRepetitionModifier(sacrifice.Type);

        if (repetitionModifier < 1f)
        {
            string message = repetitionModifier < 0.5f
                ? "Господин устал от однообразных жертв! Благосклонность снижена!"
                : "Господин скучает... Попробуй другую жертву";

            LogSystem.Instance.AddLog(message, Color.yellow);
        }
        
        if (G.CurseManager.IsCurseActive("thirst") && sacrifice.Type == ResourceType.Health)
        {
            int healthCost = 10;

            if (Health.Instance.CurrentHealth >= healthCost)
            {
                Health.Instance.TakeDamage(healthCost);
                LogSystem.Instance.AddLog($"Ты утолил жажду Господина своей кровью! -{healthCost} HP", Color.green);
            }
            else
            {
                CurrentFavor -= 30;
                LogSystem.Instance.AddLog($"У тебя недостаточно здоровья для жертвы! ", Color.red);
            }

            OnFavorChange?.Invoke();
            G.CurseManager.RemoveCurseByName("thirst");

            // if (Health.Instance.CurrentHealth >= healthCost)
            // {
            //     Health.Instance.TakeDamage(healthCost);
            //     LogSystem.Instance.AddLog($"Ты утолил жажду Господина своей кровью! -{healthCost} HP", Color.green);
            // }
            // else
            // {
            //     CurrentFavor -= 15;
            //     LogSystem.Instance.AddLog($"Жажда Господина не утолена! Тебе не хватило здоровья! -15 благосклонности", Color.red);
            //     OnFavorChange?.Invoke();
            // }
            //
            // G.CurseManager.RemoveCurseByName("thirst");
            // return;
        }
        else if(G.CurseManager.IsCurseActive("thirst") && sacrifice.Type != ResourceType.Health)
        {
            CurrentFavor -= 15;
            OnFavorChange?.Invoke();
            G.CurseManager.RemoveCurseByName("thirst");
        }


        float powerModifier = G.CurseManager.GetModifier("sacrifice_power");
        int finalPower = Mathf.FloorToInt(sacrifice.BasePower * powerModifier);

        if (G.CurseManager.IsCurseActive("greed"))
        {
            G.ResourceManager.RemoveResource(ResourceType.Gold, 15);
        }

        float favorModifier = G.CurseManager.GetModifier("favor_gain") * repetitionModifier;

        if (G.CurseManager.IsCurseActive("eye"))
            favorModifier *= 0.5f;

        int finalFavorChange = Mathf.FloorToInt(sacrifice.FavorChange * favorModifier);
        CurrentFavor = Mathf.Clamp(CurrentFavor + finalFavorChange, 0, MaxFavor);
        OnFavorChange?.Invoke();

        AddSacrificeToHistory(sacrifice.Type);
        TotalSacrifices++;
        G.TrialSystem.StartTrial(finalPower, sacrifice.Type);
        LogSystem.Instance.LogSacrifice(sacrifice.Name, CurrentFavor - oldFavor);
        LogSystem.Instance.LogFavorChange(oldFavor, CurrentFavor);
    }

    private float CalculateRepetitionModifier(ResourceType sacrificeType)
    {
        if (_repetitionCount.ContainsKey(sacrificeType) == false) return 1f;

        int count = _repetitionCount[sacrificeType];

        if (count == 1) return 0.9f;
        if (count == 2) return 0.7f;
        if (count >= 3) return 0.5f;

        return 1f;
    }

    private void AddSacrificeToHistory(ResourceType sacrificeType)
    {
        _recentSacrifices.Enqueue(sacrificeType);

        if (!_repetitionCount.ContainsKey(sacrificeType))
            _repetitionCount[sacrificeType] = 0;

        _repetitionCount[sacrificeType]++;

        if (_recentSacrifices.Count > _maxSacrificesHistory)
        {
            ResourceType oldest = _recentSacrifices.Dequeue();
            _repetitionCount[oldest]--;

            if (_repetitionCount[oldest] <= 0)
                _repetitionCount.Remove(oldest);
        }
    }

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
    
    public void UpdateUI() =>
        OnFavorChange?.Invoke();
}