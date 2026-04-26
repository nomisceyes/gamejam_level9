[System.Serializable]
public class CurseEffect
{
    public string ModifierType; // "gather_speed", "sacrifice_power", "favor_gain" и т.д.
    public float Multiplier;     // 0.7 = -30%, 1.5 = +50% (если проклятие негативное, то <1)
}