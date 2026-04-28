using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public static Health Instance { get; private set; }
    
    public int MaxHealth;
    public int CurrentHealth;
    
    public Image HealthSlider;
    public Text HealthText;
    //public GameObject LowHealthWarning;
    
    public System.Action<int, int> OnHealthChanged;
    public System.Action OnPlayerDeath;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    private void Start()
    {
        CurrentHealth = MaxHealth;
        UpdateUI();
    }
    
    public void TakeDamage(int amount)
    {
        if (CurrentHealth <= 0) return;
        
        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        UpdateUI(); 
        
        Debug.Log($"❤️ Получено урона: {amount}. Здоровье: {CurrentHealth}/{MaxHealth}");
        
        if (CurrentHealth <= 0)
        {
            OnPlayerDeath?.Invoke();
           // Die();
        }
    }
    
    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);
        UpdateUI();
        Debug.Log($"💚 Восстановлено: {amount}. Здоровье: {CurrentHealth}/{MaxHealth}");
    }
    
    public bool HasEnoughHealth(int requiredAmount)
    {
        return CurrentHealth >= requiredAmount;
    }
    
    private void UpdateUI()
    {
        if (HealthSlider != null)
            HealthSlider.fillAmount = (float)CurrentHealth / MaxHealth;
        
        if (HealthText != null)
            HealthText.text = $"{CurrentHealth}/{MaxHealth}";
        
        // if (LowHealthWarning != null)
        //     LowHealthWarning.SetActive(CurrentHealth <= MaxHealth * 0.3f);
        
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    // private void Die()
    // {
    //     Debug.Log("💀 Игрок умер! Деревня пала...");
    //     // Здесь можно вызвать поражение
    //     G.Game.WinLoseCondition();
    // }
}