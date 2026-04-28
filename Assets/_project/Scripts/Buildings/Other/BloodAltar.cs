using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BloodAltar : Building
{
    public ParticleSystem BloodParticles;
    public Light AltarLight;
    public AudioSource SacrificeSound;
    
    public int BloodPerSacrifice = 5;  
    public int HealthCostPerUse = 10;
    
    public Button collectBloodButton;
    
    private void Start()
    {
        ResourceType = ResourceType.Blood;
        //BaseGatherPerSecond = 0f; 
        AutoGather = false;
        
        if (collectBloodButton != null)
            collectBloodButton.onClick.AddListener(CollectBloodManually);
    }

    public void CollectBloodManually()
    {
        if (Health.Instance.HasEnoughHealth(HealthCostPerUse) == false)
        {
            Debug.Log($"❌ Не хватает здоровья! Нужно {HealthCostPerUse} HP");
            return;
        }

        Health.Instance.TakeDamage(HealthCostPerUse);
        G.ResourceManager.AddResource(ResourceType.Blood, BloodPerSacrifice);
        G.Game.Totem.MakeSacrifice(SacrificePresets.Blood);
        
        Debug.Log($"🩸 Пожертвовано {HealthCostPerUse} HP → получено {BloodPerSacrifice} крови");
        
        ShowBloodCollectionEffect();
    }
    
    public void SacrificeUnit(Unit unit)
    {
        if (unit == null) return;
        
        // Награда
        G.ResourceManager.AddResource(ResourceType.Blood, unit.BloodValue);
        G.Game.Totem.CurrentFavor += unit.FavorValue;
        
        // Логи
        LogSystem.Instance?.AddLog(
            $"Жертва! {unit.villagerName} принесён в жертву. +{unit.BloodValue} крови, +{unit.FavorValue} благосклонности", 
            Color.red, "🩸");
        
        // Визуальные эффекты
        ShowSacrificeEffect();
        
        // Уничтожаем крестьянина
        unit.Sacrifice();
        
        // Спавним нового крестьянина
        UnitSpawner.Instance?.SpawnVillager();
    }
    
    private void ShowSacrificeEffect()
    {
        if (BloodParticles != null)
            BloodParticles.Play();
        
        if (AltarLight != null)
        {
            StartCoroutine(FlickerLight());
        }
        
        if (SacrificeSound != null)
            SacrificeSound.Play();
        
        // if (animator != null)
        //     animator.SetTrigger("Sacrifice");
    }
    
    private IEnumerator FlickerLight()
    {
        float originalIntensity = AltarLight.intensity;
        AltarLight.intensity = 3f;
        yield return new WaitForSeconds(0.2f);
        AltarLight.intensity = originalIntensity;
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        Unit unit = other.GetComponent<Unit>();
        if (unit != null && unit.IsDragging == false)
        {
            // Притягиваем крестьянина к алтарю
            Vector2 direction = (transform.position - unit.transform.position).normalized;
            unit.transform.Translate(direction * 2f * Time.deltaTime);
        }
    }
    
    private void ShowBloodCollectionEffect()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke(nameof(ResetColor), 0.3f);
    }
    
    private void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}