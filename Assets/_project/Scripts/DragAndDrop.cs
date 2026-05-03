using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour
{
    private Camera _mainCamera;
    private Unit _grabbedUnit;
    private Vector3 _mousePos;
    private bool _isGrabbing;

    public DragAndDrop(bool isGrabbing)
    {
        _isGrabbing = isGrabbing;
    }

    private void Start()
    {
        _mainCamera = G.Game.MainCamera;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.TryGetComponent(out Unit unit))
            {
                TryGrabUnit(unit);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (_isGrabbing && _grabbedUnit != null)
            {
                Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                
                _grabbedUnit.transform.position = mousePos;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            TryReleaseUnit();
        }
    }

    private void TryGrabUnit(Unit unit)
    {
        _isGrabbing = true;
        _grabbedUnit = unit;
        _grabbedUnit.OnGrabbed();
    }
    
    private void TryReleaseUnit()
    {
        if (_grabbedUnit == null) return;

        Vector3 centerPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] hits = Physics2D.OverlapCircleAll(centerPos, 0.5f);

        bool sacrificed = false;

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == _grabbedUnit.gameObject)
                continue;

            BloodAltar altar = hit.GetComponent<BloodAltar>();

            if (altar != null)
            {
                altar.SacrificeUnit(_grabbedUnit);
                sacrificed = true;
            }
        }

        if (sacrificed == false)
            _grabbedUnit.ReturnUnit();

        _isGrabbing = false;
        _grabbedUnit = null;
    }
}