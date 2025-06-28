using UnityEngine;
using UnityEngine.Timeline;

public class PlayerAnimator : MonoBehaviour
{
    private enum AnimMode { Idle, Walk, Attack }
    private AnimMode currentAnimMode = AnimMode.Idle;

    public SpriteRenderer spriteRenderer;

    // Idle sprites
    public Sprite idleUp;
    public Sprite idleDown;
    public Sprite idleRight;

    // Walk sprites (2-frames)
    public Sprite[] walkUp;
    public Sprite[] walkDown;
    public Sprite[] walkRight;

    // Attack sprites (2-frames)
    public Sprite[] attackUp;
    public Sprite[] attackDown;
    public Sprite[] attackRight;

    public float animationSpeed = 0.2f;

    private Vector2 moveInput;
    private string currentDirection = "right"; // Default
    private float animTimer;
    private int frameIndex;
    private bool isMoving;
    private bool isAttacking;

    private Controls controls;

    private void Awake()
    {
        controls = new Controls();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Attack.performed += ctx =>
        {
            isAttacking = true;
            frameIndex = 0;
            animTimer = 0f;
        };

        controls.Player.Attack.canceled += ctx =>
        {
            isAttacking = false;
            frameIndex = 0;
        };
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    void Update()
    {
        isMoving = moveInput != Vector2.zero;

        // Update direction even while attacking
        if (isMoving)
        {
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
                currentDirection = moveInput.x > 0 ? "right" : "left";
            else
                currentDirection = moveInput.y > 0 ? "up" : "down";
        }

        if (isAttacking)
        {
            currentAnimMode = AnimMode.Attack;
            Animate();
        }
        else if (isMoving)
        {
            currentAnimMode = AnimMode.Walk;
            Animate();
        }
        else
        {
            currentAnimMode = AnimMode.Idle;
            SetIdleSprite();
        }
    }



    void Animate()
    {
        animTimer += Time.deltaTime;
        if (animTimer >= animationSpeed)
        {
            animTimer = 0f;
            frameIndex = (frameIndex + 1) % 2;

            Sprite spriteToSet = null;
            Sprite[] currentArray = null;

            // Decide which sprite array to use
            switch (currentAnimMode)
            {
                case AnimMode.Walk:
                    switch (currentDirection)
                    {
                        case "up": currentArray = walkUp; break;
                        case "down": currentArray = walkDown; break;
                        case "right":
                        case "left": currentArray = walkRight; break;
                    }
                    break;

                case AnimMode.Attack:
                    switch (currentDirection)
                    {
                        case "up": currentArray = attackUp; break;
                        case "down": currentArray = attackDown; break;
                        case "right":
                        case "left": currentArray = attackRight; break;
                    }
                    break;
            }

            if (currentArray != null)
            {
                spriteToSet = currentArray[frameIndex];
                spriteRenderer.flipX = currentDirection == "left";
                spriteRenderer.sprite = spriteToSet;
            }
        }
    }


    void SetIdleSprite()
    {
        Sprite idle = null;
        spriteRenderer.flipX = false;

        switch (currentDirection)
        {
            case "up":
                idle = idleUp;
                break;
            case "down":
                idle = idleDown;
                break;
            case "right":
                idle = idleRight;
                break;
            case "left":
                idle = idleRight;
                spriteRenderer.flipX = true;
                break;
        }

        spriteRenderer.sprite = idle;
    }
}