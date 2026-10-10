using UnityEngine;
using UnityEngine.InputSystem;

public class PastPlayerRecorder : MonoBehaviour
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

    PlayerState[] recordedStates;

    int writeIndex = 0;
    int recordedCount = 0;

    float recordTimer = 0f;
    bool isRecording = false;

    [SerializeField]
    private float recordInterval = 0.05f;
    [SerializeField]
    private float recordDuration = 3f;

    int playbackIndex = 0;
    float playbackTimer = 0f;

    public Transform pastPlayer;
    public Rigidbody2D pastPlayerRb;
    public Collider2D playerCollider;
    public Collider2D pastPlayerCollider;

    bool isPlayingBack = false;

    public bool IsPlayingBack => isPlayingBack;

    void Start()
    {
        int maxRecordedStates = Mathf.CeilToInt(recordDuration / recordInterval);

        recordedStates = new PlayerState[maxRecordedStates];

        pastPlayer.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isRecording && !isPlayingBack)
        {
            recordTimer += Time.deltaTime;

            if (recordTimer >= recordInterval)
            {
                RecordState();
                recordTimer = 0f;
            }

            if (recordedCount >= recordedStates.Length)
            {
                StopRecording();
            }
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            if (isRecording)
            {
                StopRecording();
            }
            else if (!isPlayingBack)
            {
                StartRecording();
            }
        }

        if (Keyboard.current.pKey.wasPressedThisFrame
            && !isRecording
            && !isPlayingBack)
        {
            StartPlayback();
        }

        if (isPlayingBack && recordedCount > 0)
        {
            playbackTimer += Time.deltaTime;

            if (playbackTimer >= recordInterval)
            {
                //Debug.Log("Playback Index: " + playbackIndex);

                PlaybackState();

                playbackIndex++;
                playbackTimer = 0f;

                if (playbackIndex >= recordedCount)
                {
                    isPlayingBack = false;
                    pastPlayerRb.linearVelocity = Vector2.zero;

                    SetPlayerCollisionIgnored(false);
                }
            }
        }
    }

    void StartRecording()
    {
        writeIndex = 0;
        recordedCount = 0;
        recordTimer = 0f;

        isRecording = true;

        Debug.Log("Recording Started");
    }

    void StopRecording()
    {
        isRecording = false;

        Debug.Log("Recording Stopped. States: " + recordedCount);
    }

    void RecordState()
    {
        PlayerState state;

        state.position = player.position;
        state.velocity = playerRb.linearVelocity;
        state.isGrounded = playerController.IsGrounded;
        state.facingLeft = playerController.spriteRenderer.flipX;

        recordedStates[writeIndex] = state;

        writeIndex++;

        if (writeIndex >= recordedStates.Length)
        {
            writeIndex = 0;
        }

        if (recordedCount < recordedStates.Length)
        {
            recordedCount++;
        }
    }

    void PlaybackState()
    {
        int actualIndex;

        if (recordedCount < recordedStates.Length)
        {
            actualIndex = playbackIndex;
        }
        else
        {
            actualIndex = (writeIndex + playbackIndex) % recordedStates.Length;
        }

        PlayerState state = recordedStates[actualIndex];

        pastPlayerRb.linearVelocity = state.velocity;
        pastPlayer.GetComponentInChildren<SpriteRenderer>().flipX = state.facingLeft;
    }

    public void StartPlayback()
    {
        if (recordedCount <= 0 || isPlayingBack)
        {
            return;
        }

        int playbackStartIndex;

        if (recordedCount < recordedStates.Length)
        {
            playbackStartIndex = 0;
        }
        else
        {
            playbackStartIndex = writeIndex;
        }

        pastPlayer.position = recordedStates[playbackStartIndex].position;

        pastPlayer.gameObject.SetActive(true);

        playbackIndex = 0;
        playbackTimer = 0f;
        isPlayingBack = true;
        SetPlayerCollisionIgnored(true);
    }

    void SetPlayerCollisionIgnored(bool ignored)
    {
        Physics2D.IgnoreCollision(
            playerCollider,
            pastPlayerCollider,
            ignored
        );
    }
}