using UnityEngine;
using UnityEngine.UI;

public class TestSacrificeButton2 : MonoBehaviour
{
    public Button btn;

    private void Start()
    {
        btn.onClick.AddListener(() => { G.Game.Totem.MakeSacrifice(SacrificePresets.Blood); });
    }
}