using UnityEngine;
using UnityEngine.Serialization;

public class CubeUp : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Vector2 minBounds;
    private Vector2 maxBounds;
    private float objectWidth;
    private float objectHeight;

    //private Rigidbody2D rb;

    private Vector3 lastMousePosition;
    private Vector3 curMousePosition;
    private Vector2 velocity;

    public float force = 1f;
    public float maxSpeed = 20f; //nie usuwac bo wyklatuje poza ekran

    public Rigidbody2D centerPointRb;

    void Start()
    {
        Camera cam = Camera.main;

        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
        minBounds = new Vector2(bottomLeft.x, bottomLeft.y);
        maxBounds = new Vector2(topRight.x, topRight.y);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            objectWidth = sr.bounds.extents.x;
            objectHeight = sr.bounds.extents.y;
        }

        if (centerPointRb != null)
        {
            centerPointRb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    void OnMouseDown()
    {
        Camera cam = Camera.main;

        isDragging = true;

        if (centerPointRb != null)
        {
            centerPointRb.bodyType = RigidbodyType2D.Kinematic;
            centerPointRb.linearVelocity = Vector2.zero;
        }
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        curMousePosition = mouseWorld;
        lastMousePosition = mouseWorld;
        offset = centerPointRb.transform.position - new Vector3(mouseWorld.x, mouseWorld.y, centerPointRb.transform.position.z);
    }

    void OnMouseUp()
    {        
        Camera cam = Camera.main;

        isDragging = false;

        if (centerPointRb != null)
        {
            centerPointRb.bodyType = RigidbodyType2D.Dynamic;

            Vector3 PositionDelta = curMousePosition - lastMousePosition;

            if (PositionDelta.magnitude > 0.01f)
            {
                velocity = (PositionDelta / Time.deltaTime) * force;
                velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
                centerPointRb.linearVelocity = velocity;
            }
        }
    }

    void Update()
    {
        if (isDragging)
        {
            Camera cam = Camera.main;

            Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPos = new Vector3(mouseWorld.x, mouseWorld.y, centerPointRb.transform.position.z) + offset;

            float clampX = Mathf.Clamp(targetPos.x, minBounds.x + objectWidth, maxBounds.x - objectWidth);
            float clampY = Mathf.Clamp(targetPos.y, minBounds.y + objectHeight, maxBounds.y - objectHeight);
            
            centerPointRb.transform.position = new Vector3(clampX, clampY, centerPointRb.transform.position.z);
        }
    }

    void LateUpdate()
    {
        if (isDragging)
        {
            Camera cam = Camera.main;

            lastMousePosition = curMousePosition;
            curMousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
