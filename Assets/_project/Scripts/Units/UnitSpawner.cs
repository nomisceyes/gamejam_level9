using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public static UnitSpawner Instance;

    [Header("Настройки спавна")]
    public GameObject UnitPrefab;
    public Transform[] SpawnPoints;
    public int MaxVillagers = 5;
    public float SpawnDelay = 10f;
    
    private int _currentVillagers = 0;
    private float _spawnTimer = 0f;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    private void Start()
    {
        // Спавним начальных крестьян
        for (int i = 0; i < MaxVillagers; i++)
        {
            SpawnUnit();
        }
    }
    
    private void Update()
    {
        if (_currentVillagers < MaxVillagers)
        {
            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= SpawnDelay)
            {
                _spawnTimer = 0f;
                SpawnUnit();
            }
        }
    }
    
    public void SpawnUnit()
    {
        if (_currentVillagers >= MaxVillagers) return;
        
        Transform spawnPoint = GetRandomSpawnPoint();
        if (spawnPoint == null) return;
        
        GameObject newUnit = Instantiate(UnitPrefab, spawnPoint.position, Quaternion.identity);
        _currentVillagers++;
        
        // Подписываемся на уничтожение
        Unit unit = newUnit.GetComponent<Unit>();
        if (unit != null)
        {
            // Можно добавить событие при смерти
        }
        
        LogSystem.Instance?.AddLog($"Новый крестьянин появился в деревне", Color.white, "👨‍🌾");
    }
    
    private Transform GetRandomSpawnPoint()
    {
        if (SpawnPoints == null || SpawnPoints.Length == 0)
            return null;
        
        return SpawnPoints[Random.Range(0, SpawnPoints.Length)];
    }
    
    public void OnVillagerDied()
    {
        _currentVillagers--;
    }
}