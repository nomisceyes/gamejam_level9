using UnityEngine;

public class Game : MonoBehaviour
{
    public Totem Totem;
    private bool _isStop = false;

    private void Awake()
    {
        G.Game = this;
    }

    private void Start()
    {
        if (Totem == null)
            Totem = FindFirstObjectByType<Totem>();
    }

    private void Update()
    {
        if (_isStop == false)
            WinLoseCondition();
    }

    public void WinLoseCondition()
    {
        if (Health.Instance.CurrentHealth <= 0 || Totem.CurrentFavor <= Totem.MinFavorForLose)
        {
            LogSystem.Instance.LogGameEnd(false);
            Debug.Log("Вы пребали");
        }
        else if (Totem.CurrentFavor >= Totem.MinFavorForWin && Health.Instance.CurrentHealth > 0)
        {
            LogSystem.Instance.LogGameEnd(true);
            Debug.Log("Вы победили");
        }
        
        _isStop = true;
    }
}