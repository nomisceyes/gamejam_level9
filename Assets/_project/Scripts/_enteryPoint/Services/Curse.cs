using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Curse : ITooltip<Curse>
{
    public string CurseId;
    public string DisplayName;
    public Sprite Icon;
    [TextArea] public string Description;
    public float RemainingTime;
    public List<CurseEffect> Effects = new ();
}