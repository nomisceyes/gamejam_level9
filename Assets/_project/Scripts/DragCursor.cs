using UnityEngine;

public class DragCursor : MonoBehaviour
{
    public Texture2D normalCursor;
    public Texture2D dragCursor;
    public Texture2D sacrificeCursor;
    
    private void Start()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // Проверяем, над чем мышь
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);
            
            if (hit != null && hit.GetComponent<Unit>() != null)
                Cursor.SetCursor(dragCursor, Vector2.zero, CursorMode.Auto);
            else if (hit != null && hit.GetComponent<BloodAltar>() != null)
                Cursor.SetCursor(sacrificeCursor, Vector2.zero, CursorMode.Auto);
            else
                Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
        }
    }
}
