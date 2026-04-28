using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Характеристики")]
    public string villagerName = "Крестьянин";
    public int BloodValue = 10;      
    public int FavorValue = 5;       
    
    [Header("Визуал")]
    public SpriteRenderer SpriteRenderer;
    public GameObject SelectionCircle;
    public ParticleSystem WalkParticles;
    
    [Header("Движение")]
    public float WalkSpeed = 1f;
    public Vector2[] Waypoints;      // Точки маршрута
    private int _currentWaypoint = 0;
    
    public bool IsDragging = false;
    private Vector3 _dragOffset;
    private Camera _mainCamera;
    private Color _originalColor;
    
    private void Start()
    {
        _mainCamera = Camera.main;
        _originalColor = SpriteRenderer.color;
        
        string[] names = { "Иван", "Пётр", "Мария", "Анна", "Дмитрий", "Елена" };
        villagerName = names[Random.Range(0, names.Length)];
        
        // Случайные точки маршрута, если не заданы
        if (Waypoints == null || Waypoints.Length == 0)
            GenerateRandomWaypoints();
    }
    
    private void Update()
    {
        if (IsDragging == false)
            MoveAlongPath();
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
        // Генерируем случайные точки вокруг
        Waypoints = new Vector2[4];
        for (int i = 0; i < 4; i++)
        {
            float angle = i * 90 * Mathf.Deg2Rad;
            Waypoints[i] = (Vector2)transform.position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 2f;
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("111");
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        IsDragging = true;
        SelectionCircle.SetActive(true);
        SpriteRenderer.color = Color.yellow;
        
        // Отключаем коллайдер, чтобы не мешал
        GetComponent<Collider2D>().enabled = false;
        
        // Рассчитываем смещение от мыши до центра объекта
        Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _dragOffset = transform.position - mousePos;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x + _dragOffset.x, mousePos.y + _dragOffset.y, 0);
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        IsDragging = false;
        SelectionCircle.SetActive(false);
        SpriteRenderer.color = _originalColor;
        GetComponent<Collider2D>().enabled = true;
        
        // Проверяем, попали ли на алтарь
        CheckForAltarDrop();
    }
    
    private void CheckForAltarDrop()
    {
        // Получаем точку под мышью
        Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        
        // Проверяем коллайдеры в точке
        Collider2D hit = Physics2D.OverlapPoint(mousePos);
        
        if (hit != null)
        {
            BloodAltar altar = hit.GetComponent<BloodAltar>();
            if (altar != null)
            {
                // Жертвуем крестьянина
                altar.SacrificeUnit(this);
                return;
            }
        }
        
        // Если не попали на алтарь, возвращаемся на ближайший путь
        ReturnToNearestWaypoint();
    }
    
    private void ReturnToNearestWaypoint()
    {
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
        // // Эффект жертвы
        // Instantiate(sacrificeEffect, transform.position, Quaternion.identity);
        // Destroy(gameObject);
    }
}