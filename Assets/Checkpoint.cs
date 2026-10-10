using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform respawnPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerDeath playerDeath =
            other.GetComponentInParent<PlayerDeath>();

        if (playerDeath == null)
        {
            return;
        }

        playerDeath.SetCheckpoint(respawnPoint.position);
    }
}