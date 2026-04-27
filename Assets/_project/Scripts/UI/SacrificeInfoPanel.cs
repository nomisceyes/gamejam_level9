using UnityEngine;
using UnityEngine.UI;

public class SacrificeInfoPanel : MonoBehaviour
{
    public Text InfoText;

    public void UpdateInfo(SacrificeData sacrifice)
    {
        int enemyPower = G.TrialSystem.GetEnemyPower(G.Game.Totem.TotalSacrifices);
        float winChance = (float)sacrifice.BasePower / enemyPower;
        
        InfoText.text = $"⚔️ Испытание: сила врага {enemyPower}\n" +
                        $"📊 Твой шанс победы: {Mathf.RoundToInt(winChance * 100)}%\n" +
                        (winChance >= 0.5f ? "✅ Хорошие шансы" : "⚠️ Рискованно");
    }
}