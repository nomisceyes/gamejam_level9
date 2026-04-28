using UnityEngine;
using UnityEngine.UI;

public class ResoultHandler : MonoBehaviour
{
    public static ResoultHandler Instance;
    
    [SerializeField] private GameObject _resoultsPanel;
    [SerializeField] private Text _loseText;
    [SerializeField] private Text _winText;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        
        _resoultsPanel.SetActive(false);
        _loseText.gameObject.SetActive(false);
        _winText.gameObject.SetActive(false);
    }
    
    public void ShowResoult(bool isWin)
    {
        _resoultsPanel.SetActive(true);
        
        if(isWin)
            _winText.gameObject.SetActive(true);
        else
            _loseText.gameObject.SetActive(true);
    }
}