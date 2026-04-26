using System;
using System.Collections.Generic;
using UnityEngine;

public class CurseManager : MonoBehaviour, IService
{
    private List<Curse> _activeCurses = new();

    public event Action<Curse> OnCurseAdded;
    public event Action<Curse> OnCurseExpired;
    public event Action<List<Curse>> OnAllCursesChanged;

    private Dictionary<string, float> multipliers = new();

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
        if (existing != null && existing.IsStrong)
        {
            Debug.Log($"Curse {curseId} is already active in an independent form");
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
        ApplyCurseEffect(newCurse);
        OnCurseAdded?.Invoke(newCurse);
        OnAllCursesChanged?.Invoke(_activeCurses);

        Debug.Log($"Curse {curseId} has been applied");
    }

    public void RemoveCurse(Curse curse)
    {
        _activeCurses.Remove(curse);
        RemoveCurseEffect(curse);
        OnCurseExpired?.Invoke(curse);
        OnAllCursesChanged?.Invoke(_activeCurses);

        Debug.Log($"Curse {curse.DisplayName} has been removed");
    }

    public float GetModifier(string modifierType)
    {
        if (multipliers.ContainsKey(modifierType))
            return multipliers[modifierType];
        return 1f;
    }

    public bool IsCurseActive(string curseId) =>
        _activeCurses.Exists(c => c.CurseId == curseId);

    public List<Curse> GetActiveCurses() =>
        new List<Curse>(_activeCurses);

    private void ApplyCurseEffect(Curse curse)
    {
        foreach (var effect in curse.Effects)
        {
            if (multipliers.ContainsKey(effect.ModifierType))
                multipliers[effect.ModifierType] *= effect.Multiplier;
            else
                multipliers[effect.ModifierType] = effect.Multiplier;
        }
    }

    private void RemoveCurseEffect(Curse curse)
    {
        foreach (var effect in curse.Effects)
        {
            if (multipliers.ContainsKey(effect.ModifierType))
            {
                multipliers[effect.ModifierType] /= effect.Multiplier;

                if (Mathf.Approximately(multipliers[effect.ModifierType], 1f))
                    multipliers.Remove(effect.ModifierType);
            }
        }
    }
    
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
        switch (curseId)
        {
            case "rot":
                return new Curse
                {
                    CurseId = "rot",
                    DisplayName = "Rot",
                    Icon = "🌿",
                    Description = "Resource collection has been slowed down by 30%.",
                    Effects = new List<CurseEffect>
                    {
                        new CurseEffect { ModifierType = "sacrifice_power", Multiplier = 0.5f }
                    }
                };

            case "eye":
                return new Curse
                {
                    CurseId = "eye",
                    DisplayName = "TotemEye",
                    Icon = "👁️",
                    Description = "The next victim is 50% weaker.",
                    Effects = new List<CurseEffect>
                    {
                        new CurseEffect { ModifierType = "sacrifice_power", Multiplier = 0.5f }
                    }
                };

            case "thirst":
                return new Curse
                {
                    CurseId = "thirst",
                    DisplayName = "Bloodlust",
                    Icon = "🩸",
                    Description = "You need to bring blood, otherwise there will be a fine.",
                    Effects = new List<CurseEffect>(),
                    OnApplied = (curse) => { Debug.Log("Тотем требует кровь!"); }
                };
            
            case "silence":
                return new Curse
                {
                    CurseId = "silence",
                    DisplayName = "Silence",
                    Icon = "🔇",
                    Description = "Favorability grows 40% slower",
                    Effects = new List<CurseEffect>
                    {
                        new CurseEffect { ModifierType = "favor_gain", Multiplier = 0.6f }
                    }
                };
            
            case "curse_of_greed": // Проклятие жадности
                return new Curse
                {
                    CurseId = "greed",
                    DisplayName = "Жадность",
                    Icon = "💰",
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