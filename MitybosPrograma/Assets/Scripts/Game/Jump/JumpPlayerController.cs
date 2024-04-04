using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpPlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 8f;
    public Rigidbody2D rb;
    public Animator anim;

    private float moveX;

    //Player Movement
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
    //

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movementInput = WASD.ReadValue<Vector2>();
        moveX = movementInput.x * moveSpeed;
    }

    private void FixedUpdate()
    {
        Vector2 veloctiy = rb.velocity;
        veloctiy.x = moveX;
        rb.velocity = veloctiy;
    }
    public void Land() 
    {
        if (rb.velocity.y == 0) { anim.SetBool("grounded", true); }
        anim.SetTrigger("land");
    }

    void JumpAction()
    {
        if (rb.velocity.y == 0)
        {
            anim.SetBool("grounded", false);
            Vector2 velocity = rb.velocity;
            velocity.y = jumpForce;
            rb.velocity = velocity;
        }
    }

    // Method to handle the performed event of Jump action
    void OnJumpPerformed(InputAction.CallbackContext context)
    {
        JumpAction();
    }
}
