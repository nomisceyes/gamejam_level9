using UnityEngine;
using UnityEngine.UI;

public class TestSacrificeButton1 : MonoBehaviour
{
    public Button btn;
    public Text btnText;
    
    private void Start()
    {
        btn.onClick.AddListener(() =>
        {
            Debug.Log("Sacrifice grain");
            G.Game.Totem.MakeSacrifice(SacrificePresets.Gold);
        });
        
        btnText.text = "Sacrifice grain";
    }
}