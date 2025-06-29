using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private PlayerController playerController;

    private float mobileInputX = 0f;
    private Vector2 moveInput;

    private enum MovementState { idle, walk, jump, fall, run }

    [Header("Jump Settings")]
    [SerializeField] private LayerMask jumpableGround;
    private BoxCollider2D coll;

    // Double jump logic
    private int jumpCounter = 0;
    [SerializeField] private int maxJumpCount = 2;

    [Header("Knockback")]
    public float knockbackForceX = 5f;
    public float knockbackForceY = 5f;
    public float knockbackDuration = 0.3f;
    private bool isKnockbacked = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        playerController = new PlayerController();
    }

    private void OnEnable()
    {
        playerController.Enable();
        playerController.Movement.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerController.Movement.Move.canceled += ctx => moveInput = Vector2.zero;
        playerController.Movement.Jump.performed += ctx => Jump();
    }

    private void OnDisable()
    {
        playerController.Disable();
    }

    private void Update()
    {
        if (Time.timeScale == 0) return;  // Hentikan movement saat pause
        if (Application.isMobilePlatform)
        {
            moveInput = new Vector2(mobileInputX, 0f);
        }
        else
        {
            moveInput = playerController.Movement.Move.ReadValue<Vector2>();
        }

        if (isGrounded())
        {
            jumpCounter = 0;
        }
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0) return;  // Hentikan movement saat pause
        if (!isKnockbacked)
        {
            Vector2 targetVelocity = new Vector2((moveInput.x + mobileInputX) * moveSpeed, rb.velocity.y);
            rb.velocity = targetVelocity;
        }
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        MovementState state;
        float horizontal = moveInput.x != 0 ? moveInput.x : mobileInputX;

        if (horizontal > 0f)
        {
            state = MovementState.walk;
            sprite.flipX = false;
        }
        else if (horizontal < 0f)
        {
            state = MovementState.walk;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > 0.1f)
        {
            state = MovementState.jump;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.fall;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void Jump()
    {
        if (jumpCounter < maxJumpCount && !isKnockbacked)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCounter++;
        }
    }

    // Knockback Method
    public void ApplyKnockback(Transform enemy)
    {
        isKnockbacked = true;
        rb.velocity = Vector2.zero;

        Vector2 direction = (transform.position.x < enemy.position.x) ? Vector2.left : Vector2.right;
        rb.AddForce(new Vector2(direction.x * knockbackForceX, knockbackForceY), ForceMode2D.Impulse);
        StartCoroutine(ResetKnockback());
    }

    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(knockbackDuration);
        isKnockbacked = false;
    }

    // === MOBILE BUTTONS ===
    public void MoveRight(bool isPressed) => mobileInputX = isPressed ? 1f : 0f;
    public void MoveLeft(bool isPressed) => mobileInputX = isPressed ? -1f : 0f;
    public void MobileJump() => Jump();
}
