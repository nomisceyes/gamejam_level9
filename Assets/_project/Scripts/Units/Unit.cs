using UnityEngine;
using UnityEngine.InputSystem;

public class Unit : MonoBehaviour
{
    [Header("Характеристики")] public string villagerName = "Крестьянин";
    public int BloodValue = 10;
    public int FavorValue = 5;

    [Header("Визуал")] public SpriteRenderer SpriteRenderer;
    public GameObject SelectionCircle;
    public ParticleSystem WalkParticles;

    [Header("Движение")] public float WalkSpeed = 5f;
    public Vector2[] Waypoints;
    
    private int _currentWaypoint = 0;
    private Color _originalColor;
    public bool IsGrabbed = false;
    private BoxCollider2D _collider;

    private void Start()
    {
        _originalColor = SpriteRenderer.color;
        _collider = GetComponent<BoxCollider2D>();
        
        string[] names = { "Иван", "Пётр", "Мария", "Анна", "Дмитрий", "Елена" };
        villagerName = names[Random.Range(0, names.Length)];
        
        if (Waypoints == null || Waypoints.Length == 0)
            GenerateRandomWaypoints();
        
        if (SelectionCircle != null)
            SelectionCircle.SetActive(false);
    }
    
    private void Update()
    {
        if (IsGrabbed == false)
        {
            MoveAlongPath();
        }
    }
    
    public void OnGrabbed()
    {
        IsGrabbed = true;
        
        if (_collider != null)
            _collider.enabled = false;
        
        if (SelectionCircle != null)
            SelectionCircle.SetActive(true);
        
        SpriteRenderer.color = Color.yellow;
        
        if (WalkParticles != null)
            WalkParticles.Stop();
    }
    
    public void OnReleased()
    {
        IsGrabbed = false;
        
        if (_collider != null)
            _collider.enabled = true;
        
        if (SelectionCircle != null)
            SelectionCircle.SetActive(false);
        
        SpriteRenderer.color = _originalColor;
        
        if (WalkParticles != null)
            WalkParticles.Play();
        
        // Возвращаемся на путь
        ReturnToNearestWaypoint();
    }
    
    private void MoveAlongPath()
    {
        if (Waypoints.Length == 0) return;
        
        Vector3 target = Waypoints[_currentWaypoint];
        transform.position = Vector3.MoveTowards(transform.position, target, WalkSpeed * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            _currentWaypoint = (_currentWaypoint + 1) % Waypoints.Length;
        }
    }
    
    private void GenerateRandomWaypoints()
    {
        Waypoints = new Vector2[4];
        for (int i = 0; i < 4; i++)
        {
            float angle = i * 90 * Mathf.Deg2Rad;
            Waypoints[i] = (Vector2)transform.position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 2f;
        }
    }
    
    private void ReturnToNearestWaypoint()
    {
        if (Waypoints.Length == 0) return;
        
        Vector3 nearest = Waypoints[0];
        float minDist = Vector3.Distance(transform.position, nearest);
        
        foreach (var wp in Waypoints)
        {
            float dist = Vector3.Distance(transform.position, wp);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = wp;
            }
        }
        
        transform.position = nearest;
    }
    
    public void Sacrifice()
    {
        // Эффект жертвы
        Debug.Log($"Юнит {villagerName} принесён в жертву!");
        Destroy(gameObject);
    }
}