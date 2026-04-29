using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogSystem : MonoBehaviour
{
    public static LogSystem Instance { get; private set; }
    
    [Header("UI")]
    public Transform LOGContainer;
    public GameObject LOGEntryPrefab;
    public ScrollRect ScrollRect;
    public int MaxLogEntries = 50;
    
    private Queue<GameObject> logEntries = new Queue<GameObject>();
    
    private readonly Color _positiveColor = new Color(0.2f, 0.8f, 0.2f);
    private readonly Color _negativeColor = new Color(0.9f, 0.2f, 0.2f);
    private readonly Color _neutralColor = new Color(0.8f, 0.8f, 0.6f);
    private readonly Color _curseColor = new Color(0.8f, 0.4f, 0.8f);
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    private void Start()
    {
        AddLog("Добро пожаловать. Господин ждёт твоих жертв...", _neutralColor);
    }
    
    public void AddLog(string message, Color color)
    {
        GameObject newEntry = Instantiate(LOGEntryPrefab, LOGContainer);
        Text textComponent = newEntry.GetComponent<Text>();
        
        string timestamp = System.DateTime.Now.ToString("HH:mm:ss");
        textComponent.text = $"[{timestamp}] {message}";
        textComponent.color = color;
        
        logEntries.Enqueue(newEntry);

        if (logEntries.Count > MaxLogEntries)
        {
            GameObject oldest = logEntries.Dequeue();
            Destroy(oldest);
        }
        
       StartCoroutine(AutoScroll());
    }
    
    public void LogSacrifice(string resourceName, float favorChange)
    {
        if (favorChange > 0)
        {
            AddLog($"Ты принёс в жертву {resourceName}. Господин доволен! +{favorChange} благосклонности", 
                   _positiveColor);
        }
        else
        {
            AddLog($"Ты принёс в жертву {resourceName}. Господин разочарован. {favorChange} благосклонности", 
                   _negativeColor);
        }
    }
    
    public void LogTrialResult(bool success, int power, int enemyPower)
    {
        if (success)
        {
            AddLog($"Испытание пройдено! Ваше пожертвование достаточно: {power} > {enemyPower}. Господин вознаграждает тебя!", 
                   _positiveColor);
        }
        else
        {
            AddLog($"Испытание провалено! Ценность вашего пожертвования слишком мала: {power} ≤ {enemyPower}. Господин насылает проклятие!", 
                   _negativeColor);
        }
    }
    
    public void LogCurseApplied(string curseName, string description, float duration)
    {
        AddLog($"ПРОКЛЯТИЕ: {curseName} - {description} (на {duration} сек)", 
               _curseColor);
    }
    
    public void LogCurseExpired(string curseName)
    {
        AddLog($"Проклятие {curseName} исчезло.", _neutralColor);
    }
    
    public void LogFavorChange(float oldFavor, float newFavor)
    {
        float delta = newFavor - oldFavor;
        if (delta > 0)
            AddLog($"Благосклонность Господина повысилась до {newFavor} (+{delta})", _positiveColor);
        else if (delta < 0)
            AddLog($"Благосклонность Господина упала до {newFavor} ({delta})", _negativeColor);
    }
    
    public void LogGameEnd(bool isWin)
    {
        if (Health.Instance.CurrentHealth <= 0)
        {
            AddLog("Вы погибли, не успев призвать своего господина.", _negativeColor);
            return;
        }

        if (isWin)
            AddLog("Господин ликует! Ты достиг просветления. ПОБЕДА!", _positiveColor);
        else
            AddLog("Господин уничтожил деревню. Ты не смог его умилостивить. ПОРАЖЕНИЕ!", _negativeColor);
    }
    
    private IEnumerator AutoScroll()
    {
        yield return new WaitForEndOfFrame();
        if (ScrollRect != null)
        {
            ScrollRect.verticalNormalizedPosition = 0f;
        }
    }
}