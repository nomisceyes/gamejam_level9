using UnityEngine;
using UnityEngine.UI;

public class TestSacrificeButton : MonoBehaviour
{
    private void Start()
    {
        GameObject button = new GameObject("TestSacrificeButton");
        button.transform.SetParent(transform);
        Button btn = button.AddComponent<Button>();
        Image img = button.AddComponent<Image>();
        img.color = new Color(0.3f, 0.6f, 0.9f, 1f);

        Debug.Log("Create");
        
        btn.onClick.AddListener(() =>
        {
            Debug.Log("Sacrifice grain");
            G.Game.Totem.MakeSacrifice(SacrificePresets.Food);
        });
        
        Text text = button.AddComponent<Text>();
        text.text = "Sacrifice grain";
    }
}