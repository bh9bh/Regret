using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public PastPlayerRecorder recorder;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            Debug.Log("Player Death");

            recorder.StartPlayback();
        }
    }
}