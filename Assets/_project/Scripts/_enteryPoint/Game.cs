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
}