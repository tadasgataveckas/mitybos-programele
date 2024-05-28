using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class JumpPlayerController : MonoBehaviour
{
    public bool tiltControls;
    public float moveSpeed = 10f;
    public float jumpForce = 8f;
    public float tiltSensitivity = 2f; // Sensitivity multiplier for tilt input
    public Rigidbody2D rb;
    public Animator anim;
    public SpriteRenderer spriteRenderer;
    public bool sitting;

    public TextMeshProUGUI ControlSetText;

    [HideInInspector] public GameObject currentChef;

    private float moveX;

    // Player Movement
    [SerializeField] InputAction WASD;
    [SerializeField] InputAction Jump;
    Vector2 movementInput;

    private void OnEnable()
    {
        WASD.Enable();
        Jump.Enable();
        Jump.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        WASD.Disable();
        Jump.Disable();
        Jump.performed -= OnJumpPerformed;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        UpdateTiltText();
    }

    void Update()
    {
        if (tiltControls && Mathf.Abs(rb.velocity.y) > 0.01f) // Only use tilt input when not grounded
        {
            // Use the tilt of the phone to control movement, applying sensitivity multiplier
            movementInput = new Vector2(Input.acceleration.x * tiltSensitivity, 0);
        }
        /*else
        {
            movementInput = WASD.ReadValue<Vector2>();
        }*/

        moveX = movementInput.x * moveSpeed;

        // Flip sprite based on movement direction
        if (moveX < 0)
        {
            spriteRenderer.flipX = true; // Flip horizontally if moving left
        }
        else if (moveX > 0)
        {
            spriteRenderer.flipX = false; // Don't flip if moving right
        }
    }

    private void FixedUpdate()
    {
        if (!sitting)
        {
            Vector2 velocity = rb.velocity;
            velocity.x = moveX;
            rb.velocity = velocity;
        }
    }

    public void Land()
    {
        if (rb.velocity.y == 0) { anim.SetBool("grounded", true); }
        anim.SetTrigger("land");
    }

    public void LandSit()
    {
        if (rb.velocity.y == 0) { anim.SetBool("grounded", true); }
        anim.SetTrigger("landSit");
        spriteRenderer.flipX = false;
    }

    void JumpAction()
    {
        if (rb.velocity.y == 0)
        {
            if (currentChef != null)
            {
                currentChef.GetComponent<Chef>().PlayerTriesToJump();
                currentChef = null;
                anim.SetTrigger("curious");
            }
            else
            {
                anim.SetBool("grounded", false);
                Vector2 velocity = rb.velocity;
                velocity.y = jumpForce;
                rb.velocity = velocity;
                sitting = false;
            }
        }
    }

    // Method to handle the performed event of Jump action
    void OnJumpPerformed(InputAction.CallbackContext context)
    {
        JumpAction();
    }

    public void ScreenPressDown(bool right)
    {
        // On press anywhere
        JumpAction();
        if (!tiltControls)
        {
            if (right)
            {
                movementInput = new Vector2(1f, 0f);
            }
            else
            {
                movementInput = new Vector2(-1f, 0f);
            }
        }
    }

    public void ScreenPressUp(bool right)
    {
        if (!tiltControls)
        {
            if (right && movementInput == new Vector2(1f, 0f))
            {
                movementInput = new Vector2(0f, 0f);
            }
            else if (movementInput == new Vector2(-1f, 0f))
            {
                movementInput = new Vector2(0f, 0f);
            }
        }
    }

    public void SwitchTilt()
    {
        tiltControls = !tiltControls;
        UpdateTiltText();


    }
    private void UpdateTiltText()
    {
        if (tiltControls)
        {
            ControlSetText.text = "Current: Tilt Phone";
        }
        else
        {
            ControlSetText.text = "Current: Tap Screen";
        }
    }
}
