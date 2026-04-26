using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurseUI : MonoBehaviour
{
    public Transform CurseContainer;
    public GameObject CursePrefab;

    private Dictionary<string, GameObject> CurseIcons = new();

    private void Start()
    {
        G.CurseManager.OnCurseAdded += AddCurseIcon;
        G.CurseManager.OnCurseExpired += RemoveCurseIcon;
    }

    private void Update()
    {
        foreach (var curse in G.CurseManager.GetActiveCurses())
        {
            if (CurseIcons.ContainsKey(curse.CurseId))
            {
                Text text = CurseIcons[curse.CurseId].GetComponentInChildren<Text>();
                text.text = $"{curse.Icon} {curse.DisplayName}\n{curse.RemainingTime:F0}c";
            }
        }
    }
    
    private void AddCurseIcon(Curse curse)
    {
        GameObject icon = Instantiate(CursePrefab, CurseContainer);
        icon.GetComponentInChildren<Text>().text = $"{curse.Icon} {curse.DisplayName}\n{curse.RemainingTime:F0}c";
        CurseIcons[curse.CurseId] = icon;
    }

    private void RemoveCurseIcon(Curse curse)
    {
        if (CurseIcons.ContainsKey(curse.CurseId))
        {
            Destroy(CurseIcons[curse.CurseId]);
            CurseIcons.Remove(curse.CurseId);
        }
    }
}