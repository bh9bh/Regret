using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public PastPlayerRecorder recorder;
    public PlayerController playerController;
    public Rigidbody2D playerRb;

    private bool isDead = false;
    private Vector3 respawnPosition;

    public Collider2D playerCollider;
    public Collider2D pastPlayerCollider;

    void Start()
    {
        respawnPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        Debug.Log("Player Death");

        playerController.enabled = false;
        playerRb.linearVelocity = Vector2.zero;

        transform.position = respawnPosition;

        playerController.enabled = true;
        isDead = false;

        Debug.Log("Player Respawned");
    }

    public void SetCheckpoint(Vector3 newPosition)
    {
        respawnPosition = newPosition;

        Debug.Log("Checkpoint Updated: " + respawnPosition);
    }
}