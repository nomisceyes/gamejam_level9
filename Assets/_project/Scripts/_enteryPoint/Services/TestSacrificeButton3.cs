using UnityEngine;
using UnityEngine.UI;

public class TestSacrificeButton3 : MonoBehaviour
{
    public Button btn;

    private void Start()
    {
        btn.onClick.AddListener( () =>
        {
            G.Game.Totem.MakeSacrifice(SacrificePresets.HealthSacrifice);
        });
    }
    
    private void SacrificeHealth(int amount)
    {
        if (Health.Instance.CurrentHealth > amount)
        {
            Health.Instance.TakeDamage(amount);
        }
        else
        {
            Health.Instance.TakeDamage(5);
        }

        G.Game.Totem.CurrentFavor += amount / 2;
    }
    
    private void Update()
    {
        bool isThirstActive = G.CurseManager.IsCurseActive("thirst");
        
        if (isThirstActive)
        {
            btn.targetGraphic.color = Color.red;
        }
        else
        {
            btn.targetGraphic.color = Color.white;
        }
    }
}