using UnityEngine;

public class Game : MonoBehaviour
{
    public Totem Totem;

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
        WinLoseCondition();
    }

    public void WinLoseCondition()
    {
        if (Health.Instance.CurrentHealth <= 0 || Totem.CurrentFavor <= Totem.MinFavorForLose)
        {
            Debug.Log("Вы пребали");
        }
        else if (Totem.CurrentFavor >= Totem.MinFavorForWin && Health.Instance.CurrentHealth > 0)
        {
            Debug.Log("Вы победили");
        }
    }
}