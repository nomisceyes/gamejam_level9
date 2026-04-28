using System;
using System.Collections.Generic;
using UnityEngine;

public class CurseManager : MonoBehaviour, IService
{
    private List<Curse> _activeCurses = new();

    public event Action<Curse> OnCurseAdded;
    public event Action<Curse> OnCurseExpired;

    public void Init()
    {
    }

    private void Update()
    {
        if (_activeCurses.Count != 0)
        {
            for (int i = _activeCurses.Count - 1; i >= 0; i--)
            {
                Curse curse = _activeCurses[i];
                curse.RemainingTime -= Time.deltaTime;

                if (curse.RemainingTime <= 0)
                    RemoveCurse(curse);
            }
        }
    }

    public void ApplyCurse(string curseId, float duration, string displayName = null)
    {
        Curse existing = _activeCurses.Find(c => c.CurseId == curseId);
        if (existing != null)
        {
            existing.RemainingTime = Mathf.Max(existing.RemainingTime, duration);

            LogSystem.Instance.AddLog($"Проклятие {existing.DisplayName} продлено до {duration} сек",
                Color.yellow, "⏰");
            
            return;
        }

        Curse newCurse = GetCurseTemplate(curseId);
        if (newCurse == null)
        {
            Debug.LogWarning($"Unable to find curse {curseId}");
            return;
        }

        newCurse.RemainingTime = duration;
        if (string.IsNullOrEmpty(displayName) == false)
            newCurse.DisplayName = displayName;

        _activeCurses.Add(newCurse);
        OnCurseAdded?.Invoke(newCurse);

        LogSystem.Instance.LogCurseApplied(newCurse.DisplayName, newCurse.Description, duration);
    }

    public void RemoveCurse(Curse curse)
    {
        TooltipManager.Instance.HideTooltip();
        _activeCurses.Remove(curse);
        OnCurseExpired?.Invoke(curse);

        LogSystem.Instance.LogCurseExpired(curse.DisplayName);
    }

    public float GetModifier(string modifierType)
    {
        float totalMultiplier = 1f;

        foreach (Curse curse in _activeCurses)
        {
            foreach (CurseEffect effect in curse.Effects)
            {
                if (effect.ModifierType == modifierType)
                {
                    totalMultiplier *= effect.Multiplier;
                }
            }
        }
        
        return totalMultiplier;
    }

    public bool IsCurseActive(string curseId) =>
        _activeCurses.Exists(c => c.CurseId == curseId);

    public List<Curse> GetActiveCurses() =>
        new (_activeCurses);
    
    public void RemoveCurseByName(string curseId)
    {
        Curse curse = _activeCurses.Find(c => c.CurseId == curseId);
        if (curse != null)
        {
            RemoveCurse(curse);
        }
    }
    
    private Curse GetCurseTemplate(string curseId)
    {
        Sprite icon = Resources.Load<Sprite>($"CurseIcons/{curseId}");

        if (icon == null)
            Debug.LogWarning($"Unable to find curse icon {curseId}");

        switch (curseId)
        {
            case "rot":
                return new Curse
                {
                    CurseId = "rot",
                    DisplayName = "Порча",
                    Icon = icon,
                    Description = "Сбор ресурсов замедлился на 50%.",
                    Effects = new List<CurseEffect>
                    {
                        new CurseEffect { ModifierType = "gather_speed", Multiplier = 0.5f }
                    }
                };

            case "eye":
                return new Curse
                {
                    CurseId = "eye",
                    DisplayName = "Сглаз",
                    Icon = icon,
                    Description = "Следующая жертва на 50% слабее.",
                    Effects = new List<CurseEffect>
                    {
                        new CurseEffect { ModifierType = "sacrifice_power", Multiplier = 0.5f }
                    }
                };

            case "thirst":
                return new Curse
                {
                    CurseId = "thirst",
                    DisplayName = "Жажда крови",
                    Icon = icon,
                    Description = "Необходимо пожертвовать кровь, иначе будет наложен штраф.",
                    Effects = new List<CurseEffect>(),
                };

            case "time_slow":
                return new Curse
                {
                    CurseId = "time_slow",
                    DisplayName = "Замедление времени",
                    Description = "Благосклонность падает в 2 раза быстрее",
                    Icon = icon,
                    Effects = new List<CurseEffect>
                    {
                        new CurseEffect { ModifierType = "favor_decay", Multiplier = 2f }
                    }
                };
            
            case "greed": 
                return new Curse
                {
                    CurseId = "greed",
                    DisplayName = "Жадность",
                    Icon = icon,
                    Description = "Теряешь золото при каждой жертве",
                    Effects = new List<CurseEffect>
                    {
                        new CurseEffect { ModifierType = "gold_loss", Multiplier = 1f }
                    }
                };

            default:
                return null;
        }
    }
}