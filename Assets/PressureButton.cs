using UnityEngine;

public class PressureButton : MonoBehaviour
{
    private bool isPressed = false;

    public bool IsPressed => isPressed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPressed = true;
            Debug.Log("Player 또는 PastPlayer가 버튼을 밟았습니다.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPressed = false;
            Debug.Log("Player 또는 PastPlayer가 버튼에서 나갔습니다.");
        }
    }
}