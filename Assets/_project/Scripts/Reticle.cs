using UnityEngine;

public class Reticle : MonoBehaviour
{
    public static Reticle Instance;

    [Header("Визуал")] public GameObject reticleObject;
    public Color normalColor = Color.white;
    public Color grabbedColor = Color.yellow;

    [Header("Настройки")] public float grabDistance = 3f; // Дистанция захвата юнита

    private Camera _mainCamera;
    private SpriteRenderer _reticleRenderer;
    private bool _isGrabbing = false;
    private Unit _grabbedUnit = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _mainCamera = G.Game.MainCamera;
        _reticleRenderer = reticleObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Прицел всегда по центру экрана
        Vector3 centerPos = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10));
        centerPos.z = 0;
        reticleObject.transform.position = centerPos;

        if (!_isGrabbing)
        {
            // Проверяем, есть ли юнит под прицелом
            CheckForUnitUnderReticle();

            // Захват по пробелу
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TryGrabUnit();
            }
        }
        else
        {
            // Юнит следует за прицелом
            if (_grabbedUnit != null)
            {
                _grabbedUnit.transform.position = reticleObject.transform.position;
            }

            // Отпускание по пробелу
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TryReleaseUnit();
            }
        }
    }

    private void CheckForUnitUnderReticle()
    {
        Vector3 centerPos = reticleObject.transform.position;
        Collider2D hit = Physics2D.OverlapCircle(centerPos, 0.5f);

        if (hit != null && hit.GetComponent<Unit>() != null)
        {
            _reticleRenderer.color = grabbedColor;
        }
        else
        {
            _reticleRenderer.color = normalColor;
        }
    }

    private void TryGrabUnit()
    {
        Vector3 centerPos = reticleObject.transform.position;
        Collider2D hit = Physics2D.OverlapCircle(centerPos, 0.5f);

        if (hit != null)
        {
            Unit unit = hit.GetComponent<Unit>();
            if (unit != null)
            {
                _isGrabbing = true;
                _grabbedUnit = unit;
                _grabbedUnit.OnGrabbed();

                Debug.Log($"Захвачен юнит: {_grabbedUnit.Name}");
            }
        }
    }

    private void TryReleaseUnit()
    {
        if (_grabbedUnit == null) return;

        // Проверяем, попали ли на алтарь
        Vector3 centerPos = reticleObject.transform.position;
        Collider2D hit = Physics2D.OverlapCircle(centerPos, 2f);

        bool sacrificed = false;

        if (hit != null)
        {
            Debug.Log($"Hit: {hit.gameObject.name}"); 
            
            BloodAltar altar = hit.GetComponent<BloodAltar>();
            if (altar == null)
                Debug.Log("Altar");

            if (altar != null)
            {
                Debug.Log("Sacrifice");
                altar.SacrificeUnit(_grabbedUnit);
                sacrificed = true;
            }
        }

        if (!sacrificed)
        {
            _grabbedUnit.OnReleased();
        }

        _isGrabbing = false;
        _grabbedUnit = null;

        Debug.Log(sacrificed ? "Юнит принесён в жертву!" : "Юнит отпущен");
    }
}