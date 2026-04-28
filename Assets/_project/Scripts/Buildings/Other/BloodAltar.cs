using System.Collections;
using UnityEngine;

public class BloodAltar : Building
{
    public ParticleSystem BloodParticles;
    public Light AltarLight;
    public AudioSource SacrificeSound;
    
    // public int BloodPerSacrifice = 5;  
    // public int HealthCostPerUse = 10;
    
    private void Start()
    {
        ResourceType = ResourceType.Blood;
        //BaseGatherPerSecond = 0f; 
        AutoGather = false;
    }

    // public void CollectBloodManually()
    // {
    //     if (Health.Instance.HasEnoughHealth(HealthCostPerUse) == false)
    //     {
    //         Debug.Log($"❌ Не хватает здоровья! Нужно {HealthCostPerUse} HP");
    //         return;
    //     }
    //     
    //     G.ResourceManager.AddResource(ResourceType.Blood, BloodPerSacrifice);
    //     G.Game.Totem.MakeSacrifice(SacrificePresets.Blood);
    //     
    //     Debug.Log($"🩸 Пожертвовано {HealthCostPerUse} HP → получено {BloodPerSacrifice} крови");
    //     
    //     ShowBloodCollectionEffect();
    // }
    
    public void SacrificeUnit(Unit unit)
    {
        if (unit == null) return;
        
        // Награда
        G.ResourceManager.AddResource(ResourceType.Blood, unit.BloodValue);
        G.Game.Totem.CurrentFavor += unit.FavorValue;

        LogSystem.Instance.AddLog($"Жертва - {unit.Name}: получено {unit.BloodValue} крови, {unit.FavorValue} благосклонности", Color.brown);
        
        // Эффекты
        if (BloodParticles != null)
            BloodParticles.Play();
        
        if (AltarLight != null)
            StartCoroutine(FlickerLight());
        
        // Уничтожаем крестьянина
        unit.Sacrifice();
        
        UnitSpawner.Instance?.SpawnUnit();
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
    
    // private void ShowBloodCollectionEffect()
    // {
    //     GetComponent<SpriteRenderer>().color = Color.red;
    //     Invoke(nameof(ResetColor), 0.3f);
    // }
    //
    // private void ResetColor()
    // {
    //     GetComponent<SpriteRenderer>().color = Color.white;
    // }
}