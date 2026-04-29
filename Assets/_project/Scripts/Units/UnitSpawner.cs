using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public static UnitSpawner Instance;

    [Header("Настройки спавна")]
    public GameObject UnitPrefab;
    public Transform[] SpawnPoints;
    public int MaxVillagers = 5;
    public float MinSpawnDelay = 10f;
    public float MaxSpawnDelay = 25f;
    
    private int _currentVillagers = 0;
    private readonly List<Transform> _usedSpawnPoints = new ();
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }
    
    private System.Collections.IEnumerator SpawnLoop()
    {
        while (true)
        {
            while (_currentVillagers < MaxVillagers)
            {
                SpawnUnit();
                float delay = Random.Range(MinSpawnDelay, MaxSpawnDelay);
                yield return new WaitForSeconds(delay);
            }
            yield return new WaitForSeconds(1f);
        }
    }
    
    public void SpawnUnit()
    {
        if (_currentVillagers >= MaxVillagers) return;
        
        Transform spawnPoint = GetRandomUnusedSpawnPoint();
        if (spawnPoint == null)
        {
            spawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        }
        
        if (spawnPoint == null) return;
        
        GameObject newUnit = Instantiate(UnitPrefab, spawnPoint.position, Quaternion.identity);
        _currentVillagers++;
    }
    
    private Transform GetRandomUnusedSpawnPoint()
    {
        if (SpawnPoints.Length == 0) return null;
        
        List<Transform> freePoints = new List<Transform>();
        foreach (Transform point in SpawnPoints)
        {
            if (!_usedSpawnPoints.Contains(point))
                freePoints.Add(point);
        }
        
        if (freePoints.Count == 0) return null;
        
        return freePoints[Random.Range(0, freePoints.Count)];
    }
    
    public void UnitDie()
    {
        _currentVillagers--;
    }
}