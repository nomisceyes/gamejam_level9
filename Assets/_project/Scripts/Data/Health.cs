using System;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public static Health Instance { get; private set; }
    
    public int MaxHealth;
    public int CurrentHealth;
    
    public Image HealthSlider;
    public Text HealthText;
    
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
    }
    
    private void UpdateUI()
    {
        if (HealthSlider != null)
            HealthSlider.fillAmount = (float)CurrentHealth / MaxHealth;
        
        if (HealthText != null)
            HealthText.text = $"{CurrentHealth}/{MaxHealth}";
    }
}