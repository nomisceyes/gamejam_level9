using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Curse
{
    public string CurseId;
    public string DisplayName;
    public Sprite Icon;
    [TextArea] public string Description;
    public float RemainingTime;
    public bool IsStrong = false;
    public List<CurseEffect> Effects = new List<CurseEffect>();
    public Action<Curse> OnApplied;
    public Action<Curse> OnRemoved;
}