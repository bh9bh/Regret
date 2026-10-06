using UnityEngine;

public class DoorController : MonoBehaviour
{
    public PressureButton pressureButton;
    public Transform doorVisual;

    public float openHeight = 2f;
    public float moveSpeed = 5f;

    private Collider2D doorCollider;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private bool isOpen = false;

    void Start()
    {
        doorCollider = GetComponent<Collider2D>();

        closedPosition = doorVisual.localPosition;
        openPosition = closedPosition + Vector3.up * openHeight;
    }

    void Update()
    {
        if (pressureButton.IsPressed && !isOpen)
        {
            isOpen = true;
            doorCollider.enabled = false;

            Debug.Log("Door Open");
        }
        else if (!pressureButton.IsPressed && isOpen)
        {
            isOpen = false;
            doorCollider.enabled = true;

            Debug.Log("Door Closed");
        }

        Vector3 targetPosition = isOpen ? openPosition : closedPosition;

        doorVisual.localPosition = Vector3.MoveTowards(
            doorVisual.localPosition,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
    }
}