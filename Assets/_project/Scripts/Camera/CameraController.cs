using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public Vector2 MoveBoundsMin = new Vector2(-10f, -10f);
    public Vector2 MoveBoundsMax = new Vector2(10f, 10f);
    
    private InputSystem _inputSystem;
    private Camera _camera;
    private Vector2 _moveInput;
    
    private void Awake()
    {
        _camera = Camera.main;
        
        _inputSystem = new InputSystem();
        _inputSystem.Enable();
    }

    private void Update()
    {
        Move();
    }
    
    private void OnEnable()
    {
        _inputSystem.Player.Enable();
        _inputSystem.Player.Move.performed += OnMove;
        _inputSystem.Player.Move.canceled += OnMove;
    }
    
    private void OnDisable()
    {
        _inputSystem.Player.Move.performed -= OnMove;
        _inputSystem.Player.Move.canceled -= OnMove;
        _inputSystem.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context) =>
    _moveInput = context.ReadValue<Vector2>();
    
    private void Move()
    {
        Vector3 movement = new Vector3(_moveInput.x, _moveInput.y, 0f) * MoveSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position + movement;
        
        newPosition.x = Mathf.Clamp(newPosition.x, MoveBoundsMin.x, MoveBoundsMax.x);
        newPosition.y = Mathf.Clamp(newPosition.y, MoveBoundsMin.y, MoveBoundsMax.y);
        
        transform.position = newPosition;
    }
}