using UnityEngine;
using UnityEngine.UI;

public class TestSacrificeButton1 : MonoBehaviour
{
    public Button btn;
    
    private void Start()
    {
        btn.onClick.AddListener(() =>
        {
            G.Game.Totem.MakeSacrifice(SacrificePresets.Gold);
        });
    }
}