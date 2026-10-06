using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateListTest : MonoBehaviour
{
    struct PlayerState
    {
        public Vector2 position;
        public Vector2 velocity;
        public bool isGrounded;
        public bool facingLeft;
    }

    public Transform player;
    public Rigidbody2D playerRb;
    public PlayerController playerController;

    List<PlayerState> recordedStates = new List<PlayerState>();

    float recordTimer = 0f;
    float recordInterval = 0.05f;

    bool isRecording = false;

    int playbackIndex = 0;
    float playbackTimer = 0f;

    public Transform pastPlayer;
    public Rigidbody2D pastPlayerRb;

    bool isPlayingBack = false;

    void Start()
    {
        pastPlayer.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isRecording)
        {
            recordTimer += Time.deltaTime;

            if (recordTimer >= recordInterval)
            {
                RecordState();
                recordTimer = 0f;
            }
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            isRecording = !isRecording;

            if (isRecording)
            {
                pastPlayer.gameObject.SetActive(false);

                recordedStates.Clear();
                recordTimer = 0f;
                playbackIndex = 0;
                playbackTimer = 0f;

                Debug.Log("Recording Started");
            }

            else
            {
                Debug.Log("Recording Stopped");

                if (recordedStates.Count > 0)
                {
                    pastPlayer.position = recordedStates[0].position;

                    pastPlayer.gameObject.SetActive(true);

                    playbackIndex = 0;
                    isPlayingBack = true;
                }
            }
        }

        if (isPlayingBack && recordedStates.Count > 0)
        {
            playbackTimer += Time.deltaTime;

            if (playbackTimer >= recordInterval)
            {
                //Debug.Log("Playback Index: " + playbackIndex);

                PlaybackState();

                playbackIndex++;
                playbackTimer = 0f;

                if (playbackIndex >= recordedStates.Count)
                {
                    isPlayingBack = false;
                    pastPlayerRb.linearVelocity = Vector2.zero;
                }
            }
        }
    }

    void RecordState()
    {
        PlayerState state;

        state.position = player.position;
        state.velocity = playerRb.linearVelocity;
        state.isGrounded = playerController.IsGrounded;
        state.facingLeft = playerController.spriteRenderer.flipX;

        recordedStates.Add(state);
    }

    void PlaybackState()
    {
        PlayerState state = recordedStates[playbackIndex];

        pastPlayerRb.linearVelocity = state.velocity;
        pastPlayer.GetComponentInChildren<SpriteRenderer>().flipX = state.facingLeft;
    }
}