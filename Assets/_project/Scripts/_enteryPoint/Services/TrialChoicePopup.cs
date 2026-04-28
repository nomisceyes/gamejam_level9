using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TrialChoicePopup : MonoBehaviour
{
   public static TrialChoicePopup Instance;
    
    [Header("UI элементы")]
    public GameObject Panel;
    public Text TitleText;
    public Text SacrificePowerText;
    public Text EnemyPowerText;
    public Text WinChanceText;
    public Text RiskRewardText;
    public Text RefusalPenaltyText;
    public Button AcceptButton;
    public Button RefuseButton;
    
    private int _currentSacrificePower;
    private int _currentEnemyPower;
    private ResourceType _currentSacrificeType;
    private Action<bool> _onChoiceMade; 
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        Panel.SetActive(false);
        
        if (AcceptButton != null)
            AcceptButton.onClick.AddListener(AcceptTrial);
        
        if (RefuseButton != null)
            RefuseButton.onClick.AddListener(RefuseTrial);
    }
    
    public void Show(int sacrificePower, int enemyPower, ResourceType type, System.Action<bool> callback)
    {
        G.Game.PausedGame();
        
        _currentSacrificePower = sacrificePower;
        _currentEnemyPower = enemyPower;
        _currentSacrificeType = type;
        _onChoiceMade = callback;
        
        float winChance = CalculateWinChance(sacrificePower, enemyPower);
        int winChancePercent = Mathf.RoundToInt(winChance * 100);
        
        TitleText.text = "⚔️ИСПЫТАНИЕ ГОСПОДИНА ⚔️";
        SacrificePowerText.text = $"Твоя сила: {sacrificePower}";
        EnemyPowerText.text = $"Сила врага: {enemyPower}";
        
        WinChanceText.text = $"Шанс победы: {winChancePercent}%";
        WinChanceText.color = winChance >= 0.5f ? Color.green : Color.red;
        
        string rewardText = GetRewardText(type);
        string penaltyText = GetPenaltyText(type);
        RiskRewardText.text = $"✅ Победа: {rewardText}\n❌ Поражение: {penaltyText} + проклятие";
        
        RefusalPenaltyText.text = $"⚠Отказ: -{GetRefusalCost(type)} {GetResourceName(type)} + проклятие";
        RefusalPenaltyText.color = Color.red;
        
        Panel.SetActive(true);
    }
    
    private void AcceptTrial()
    {
        Panel.SetActive(false);
        
        float winChance = CalculateWinChance(_currentSacrificePower, _currentEnemyPower);
        bool success = Random.value < winChance;
        
        _onChoiceMade?.Invoke(true);
        ShowTrialResult(success);
    }
    
    private void RefuseTrial()
    {
        Panel.SetActive(false);
        G.Game.UnPausedGame();
        
        ApplyRefusalPenalty();
        
        _onChoiceMade?.Invoke(false);
        
        LogSystem.Instance.AddLog("Ты отказался от испытания! Господин гневается и желает твоей крови!", Color.red);
        G.ResourceManager.RemoveResource(ResourceType.Blood, 15);
        Health.Instance.TakeDamage(5);
    }
    
    private void ShowTrialResult(bool success)
    {
        G.Game.UnPausedGame();
        
        if (success)
        {
            ApplyVictoryReward();
            LogSystem.Instance.AddLog($"ПОБЕДА в испытании! +{GetRewardText(_currentSacrificeType)} {GetResourceName(_currentSacrificeType)}", 
                                       Color.green);
        }
        else
        {
            string curseId = GetRandomCurse();
            ApplyDefeatPenalty(curseId);
            LogSystem.Instance.AddLog($"ПОРАЖЕНИЕ в испытании! {GetDefeatPenaltyText(_currentSacrificeType, curseId)}", 
                                       Color.red);
        }
    }
    
    private float CalculateWinChance(int playerPower, int enemyPower)
    {
        return Mathf.Clamp((float)playerPower / enemyPower, 0.1f, 0.9f);
    }
    
    private void ApplyVictoryReward()
    {
        switch (_currentSacrificeType)
        {
            case ResourceType.Food:
                G.ResourceManager.AddResource(ResourceType.Food, 12);
                G.ResourceManager.AddResource(ResourceType.Gold, 8);
                break;
            case ResourceType.Gold:
                G.ResourceManager.AddResource(ResourceType.Food, 15);
                G.ResourceManager.AddResource(ResourceType.Gold, 15);
                break;
            case ResourceType.Blood:
                G.ResourceManager.AddResource(ResourceType.Food, 25);
                G.ResourceManager.AddResource(ResourceType.Gold, 20);
                break;
        }

        G.Game.Totem.CurrentFavor += 5;
    }
    
    private void ApplyDefeatPenalty(string curseId)
    {
        switch (_currentSacrificeType)
        {
            case ResourceType.Food:
                G.ResourceManager.RemoveResource(ResourceType.Food, 15);
                break;
            case ResourceType.Gold:
                G.ResourceManager.RemoveResource(ResourceType.Gold, 20);
                break;
            case ResourceType.Blood:
                G.ResourceManager.RemoveResource(ResourceType.Food, 10);
                break;
        }
       
        G.CurseManager.ApplyCurse(curseId, 40f);
        G.Game.Totem.FavorDecrease(10);
    }
    
    private void ApplyRefusalPenalty()
    {
        switch (_currentSacrificeType)
        {
            case ResourceType.Food:
                G.ResourceManager.RemoveResource(ResourceType.Food, 20);
                break;
            case ResourceType.Gold:
                G.ResourceManager.RemoveResource(ResourceType.Gold, 25);
                break;
            case ResourceType.Blood:
                G.ResourceManager.RemoveResource(ResourceType.Food, 15);
                break;
        }
        
        G.CurseManager.ApplyCurse(GetRandomCurse(), 30f);
        G.Game.Totem.FavorDecrease(10);
    }
    
    private string GetResourceName(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Food: return "еды";
            case ResourceType.Gold: return "золота";
            case ResourceType.Blood: return "крови";
            default: return "ресурсов";
        }
    }
    
    private string GetRewardText(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Food: return "+12 еды, +8 золота";
            case ResourceType.Gold: return "+15 еды, +15 золота";
            case ResourceType.Blood: return "+25 еды, +20 золота";
            default: return "+ ресурсы";
        }
    }
    
    private string GetPenaltyText(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Food: return "-15 еды";
            case ResourceType.Gold: return "-20 золота";
            case ResourceType.Blood: return "-10 еды";
            default: return "- ресурсы";
        }
    }
    
    private int GetRefusalCost(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Food: return 20;
            case ResourceType.Gold: return 25;
            case ResourceType.Blood: return 30;
            default: return 15;
        }
    }
    
    private string GetDefeatPenaltyText(ResourceType type, string curse)
    {
        switch (type)
        {
            case ResourceType.Food: return $"-15 еды + проклятие {curse}";
            case ResourceType.Gold: return $"-20 золота + проклятие {curse}";
            case ResourceType.Blood: return $"-10 еды + проклятие {curse}";
            default: return "- ресурсы + проклятие";
        }
    }

    // private string GetRandomCurse()
    // {
    //     string[] curces = { "rot" ,"eye","thirst","time_slow","greed"};
    //     int randomCurse = Random.Range(0, curces.Length);
    //     
    //     return curces[randomCurse];
    // }
    
    private string GetRandomCurse()
    {
        string[] curces = { "thirst"};
        int randomCurse = Random.Range(0, curces.Length);
        
        return curces[randomCurse];
    }
}