using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Texture2D CursorTexture;
    public float MoveSpeed = 5f;
    public Vector2 MoveBoundsMin = new (-10f, -10f);
    public Vector2 MoveBoundsMax = new (10f, 10f);

    private Vector2 _moveInput;

    private void Start()
    {
        G.Game.InputSystem.Player.Move.performed += OnMove;
        G.Game.InputSystem.Player.Move.canceled += OnMove;
        
        Cursor.SetCursor(CursorTexture, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        Move();
    }

    private void OnDisable()
    {
        G.Game.InputSystem.Player.Move.performed -= OnMove;
        G.Game.InputSystem.Player.Move.canceled -= OnMove;
    }

    private void OnMove(InputAction.CallbackContext context) =>
        _moveInput = context.ReadValue<Vector2>();

    private void Move()
    {
        Vector3 movement = new Vector3(_moveInput.x, _moveInput.y, 0f) * (MoveSpeed * Time.deltaTime);
        Vector3 newPosition = transform.position + movement;

        newPosition.x = Mathf.Clamp(newPosition.x, MoveBoundsMin.x, MoveBoundsMax.x);
        newPosition.y = Mathf.Clamp(newPosition.y, MoveBoundsMin.y, MoveBoundsMax.y);

        transform.position = newPosition;
    }
}