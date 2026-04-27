using UnityEngine;
using UnityEngine.UI;

public class TestSacrificeButton : MonoBehaviour
{
    public Button btn;
    public Text btnText;
    
    private void Start()
    {
        btn.onClick.AddListener(() =>
        {
            Debug.Log("Food");
            G.Game.Totem.MakeSacrifice(SacrificePresets.Food);
        });
        
        btnText.text = "Sacrifice grain";
    }
}