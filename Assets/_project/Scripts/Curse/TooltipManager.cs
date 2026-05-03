using UnityEngine;

public class TooltipManager : MonoBehaviour, IService
{
    public CurseTooltip CurseTooltip;
    public SacrificeTooltip SacrificeTooltip;

    public void Init()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        
        GameObject sacrificeObj = new GameObject("SacrificeTooltip") { transform = { parent = transform } };
        sacrificeObj.AddComponent<SacrificeTooltip>();
        GameObject tooltipSacrifice = Instantiate(Resources.Load<GameObject>("Tooltip/SacrificeTooltip"), canvas.transform);
        
        GameObject curseObj = new GameObject("CurseTooltip") { transform = { parent = transform } };
        curseObj.AddComponent<CurseTooltip>();
        
        CurseTooltip = curseObj.GetComponent<CurseTooltip>();
        SacrificeTooltip = sacrificeObj.GetComponent<SacrificeTooltip>();
        GameObject tooltipCurse = Instantiate(Resources.Load<GameObject>("Tooltip/CurseTooltip"), canvas.transform);
        
        SacrificeTooltip.Init(tooltipSacrifice);
        CurseTooltip.Init(tooltipCurse);
    }
}