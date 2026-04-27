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
    
    [Header("Настройки")]
    public float AutoScrollDelay = 0.1f;
    
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
        AddLog("Добро пожаловать. Тотем ждёт твоих жертв...", _neutralColor);
    }
    
    public void AddLog(string message, Color color, string icon = "📜")
    {
        GameObject newEntry = Instantiate(LOGEntryPrefab, LOGContainer);
        Text textComponent = newEntry.GetComponent<Text>();
        
        string timestamp = System.DateTime.Now.ToString("HH:mm:ss");
        textComponent.text = $"[{timestamp}] {icon} {message}";
        textComponent.color = color;
        
        logEntries.Enqueue(newEntry);

        if (logEntries.Count > MaxLogEntries)
        {
            GameObject oldest = logEntries.Dequeue();
            Destroy(oldest);
        }
        
        StartCoroutine(AutoScroll());
    }
    
    public void LogSacrifice(string resourceName, int favorChange)
    {
        if (favorChange > 0)
        {
            AddLog($"Ты принёс в жертву {resourceName}. Тотем доволен! +{favorChange} благосклонности", 
                   _positiveColor, "🩸");
        }
        else
        {
            AddLog($"Ты принёс в жертву {resourceName}. Тотем разочарован. {favorChange} благосклонности", 
                   _negativeColor, "😞");
        }
    }
    
    public void LogTrialResult(bool success, int power, int enemyPower)
    {
        if (success)
        {
            AddLog($"Испытание пройдено! Ваше пожертвование достаточно: {power} > {enemyPower}. Тотем вознаграждает тебя!", 
                   _positiveColor, "⚔️");
        }
        else
        {
            AddLog($"Испытание провалено! Ценность вашего пожертвования слишком мала: {power} ≤ {enemyPower}. Тотем насылает проклятие!", 
                   _negativeColor, "💀");
        }
    }
    
    public void LogCurseApplied(string curseName, string description, float duration)
    {
        AddLog($"ПРОКЛЯТИЕ: {curseName} - {description} (на {duration} сек)", 
               _curseColor, "🌿");
    }
    
    public void LogCurseExpired(string curseName)
    {
        AddLog($"Проклятие {curseName} исчезло.", _neutralColor, "✨");
    }
    
    // public void LogResourceGain(ResourceType type, int amount)
    // {
        // string icon = type switch
        // {
            // ResourceType.Food => "🌾",
            // ResourceType.Gold => "💰",
            // ResourceType.Blood => "🩸",
            // _ => "📦"
        // };
        // AddLog($"Получено +{amount} {type}", _positiveColor, icon);
    // }
    
    public void LogFavorChange(int oldFavor, int newFavor)
    {
        int delta = newFavor - oldFavor;
        if (delta > 0)
            AddLog($"Благосклонность тотема повысилась до {newFavor} (+{delta})", _positiveColor, "📈");
        else if (delta < 0)
            AddLog($"Благосклонность тотема упала до {newFavor} ({delta})", _negativeColor, "📉");
    }
    
    public void LogGameEnd(bool isWin)
    {
        if (isWin)
            AddLog("Тотем ликует! Ты достиг просветления. ПОБЕДА!", _positiveColor, "🏆");
        else
            AddLog("Тотем уничтожил деревню. Ты не смог его умилостивить. ПОРАЖЕНИЕ!", _negativeColor, "💀");
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