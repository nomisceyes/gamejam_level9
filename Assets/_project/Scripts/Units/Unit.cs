using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Unit : MonoBehaviour
{
    public Vector2[] Waypoints;
    public string Name = "Послушник";
    public int BloodValue = 10;
    public int FavorValue = 5;
    public float WalkSpeed = 3f;
    public bool IsGrabbed;
    
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private BoxCollider2D _collider;
    private Vector2 _lastPosition;
    private Vector2 _startPosition;
    private Color _originalColor;
    private int _currentWaypoint = 0;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Start()
    {
        _originalColor = _spriteRenderer.color;

        if (Waypoints == null || Waypoints.Length == 0)
            GenerateRandomWaypoints();
    }

    private void Update()
    {
        if (IsGrabbed == false)
        {
            MoveAlongPath();
            UpdateFacingDirection();
        }
    }

    private void UpdateFacingDirection()
    {
        if (transform.localPosition.x > Waypoints[_currentWaypoint].x)
            _spriteRenderer.flipX = true;
        else
            _spriteRenderer.flipX = false;
    }

    public void OnGrabbed()
    {
        IsGrabbed = true;
        _collider.enabled = false;

        _spriteRenderer.color = Color.yellow;
        G.AudioManager.PlaySound(R.Audio.UnitGrabSound);
    }

    public void ReturnUnit()
    {
        IsGrabbed = false;
        _collider.enabled = true;

        _spriteRenderer.color = _originalColor;

        ReturnToNearestWaypoint();
    }

    private void MoveAlongPath()
    {
        if (Waypoints.Length == 0) return;

        Vector3 target = Waypoints[_currentWaypoint];
        transform.position = Vector3.MoveTowards(transform.position, target, WalkSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            _currentWaypoint = Random.Range(0, Waypoints.Length);
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

    public void Sacrifice() =>
        StartCoroutine(SacrificeCoroutine());

    private IEnumerator SacrificeCoroutine()
    {
        _animator.SetTrigger("Sacrifice");
        G.AudioManager.PlaySound(R.Audio.UnitDieSound);

        yield return new WaitForSeconds(0.6f);

        UnitSpawner.Instance.UnitDie();
        Destroy(gameObject);
    }
}