using UnityEngine;

public class Reticle : MonoBehaviour
{
    public static Reticle Instance;

    [Header("Визуал")] public GameObject reticleObject;
    public Color NormalColor = Color.white;
    public Color GrabbedColor = Color.yellow;

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
        Vector3 centerPos = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10));
        centerPos.z = 0;
        reticleObject.transform.position = centerPos;

        if (!_isGrabbing)
        {
            CheckForUnitUnderReticle();

            if (Input.GetKeyDown(KeyCode.Space))
                TryGrabUnit();
        }
        else
        {
            if (_grabbedUnit != null)
                _grabbedUnit.transform.position = reticleObject.transform.position;

            if (Input.GetKeyDown(KeyCode.Space))
                TryReleaseUnit();
        }
    }

    private void CheckForUnitUnderReticle()
    {
        Vector3 centerPos = reticleObject.transform.position;
        Collider2D hit = Physics2D.OverlapCircle(centerPos, 0.5f);

        if (hit != null && hit.GetComponent<Unit>() != null)
            _reticleRenderer.color = GrabbedColor;
        else
            _reticleRenderer.color = NormalColor;
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
            }
        }
    }

    private void TryReleaseUnit()
    {
        if (_grabbedUnit == null) return;

        Vector3 centerPos = reticleObject.transform.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(centerPos, 2f);

        bool sacrificed = false;

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == _grabbedUnit.gameObject)
                continue;

            BloodAltar altar = hit.GetComponent<BloodAltar>();
            if (altar == null)
                Debug.Log("Altar");

            if (altar != null)
            {
                altar.SacrificeUnit(_grabbedUnit);
                sacrificed = true;
            }
        }

        if (!sacrificed)
            _grabbedUnit.OnReleased();

        _isGrabbing = false;
        _grabbedUnit = null;
    }
}