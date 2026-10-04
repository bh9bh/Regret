using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    public SpriteRenderer spriteRenderer;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private float moveInput;
    private bool jumpRequested;
    private bool isGrounded;

    public float moveSpeed = 5f;
    public float jumpForce = 10f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleMovementInput();
        CheckGround();
        HandleJumpInput();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false;
        }
    }

    void HandleMovementInput()
    {
        if (Keyboard.current.dKey.isPressed)
        {
            moveInput = 1f;
            spriteRenderer.flipY = false;
        }
        else if (Keyboard.current.aKey.isPressed)
        {
            moveInput = -1f;
            spriteRenderer.flipY = true;
        }
        else
        {
            moveInput = 0f;
        }
    }

    void HandleJumpInput()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            jumpRequested = true;
        }
    }

    void CheckGround()
    {
        Collider2D groundCollider = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        isGrounded = groundCollider != null;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundCheckRadius
        );
    }
}