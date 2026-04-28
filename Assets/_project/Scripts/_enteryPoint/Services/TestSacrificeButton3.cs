using UnityEngine;
using UnityEngine.UI;

public class TestSacrificeButton3 : MonoBehaviour
{
    public Button btn;

    private void Start()
    {
        btn.onClick.AddListener(() => { SacrificeHealth(10); });
    }

    private void SacrificeHealth(int amount)
    {
        Health.Instance.TakeDamage(amount);
        G.Game.Totem.CurrentFavor += amount / 2;
    }
}