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
        if (CurseIcons.Count > 0)
        {
            foreach (var curse in G.CurseManager.GetActiveCurses())
            {
                if (CurseIcons.ContainsKey(curse.CurseId))
                {
                    Text text = CurseIcons[curse.CurseId].GetComponentInChildren<Text>();
                    text.text = $"{curse.DisplayName}\n{curse.RemainingTime:F0}c";
                }
            }
        }
    }

    private void AddCurseIcon(Curse curse)
    {
        if (CursePrefab == null || CurseContainer == null)
            return;

        GameObject curseItem = Instantiate(CursePrefab, CurseContainer);

        CurseItem item = curseItem.GetComponent<CurseItem>();
        if (item == null)
            item = curseItem.AddComponent<CurseItem>();
        
        item.Init(curse);
        
        CurseIcons[curse.CurseId] = curseItem;
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