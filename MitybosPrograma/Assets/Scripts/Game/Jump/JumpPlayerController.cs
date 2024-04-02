using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpPlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public Rigidbody2D rb;
    public Animator anim;

    private float moveX;

    //Player Movement
    [SerializeField] InputAction WASD;
    Vector2 movementInput;

    private void OnEnable()
    {
        WASD.Enable();
    }

    private void OnDisable()
    {
        WASD.Disable();
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
        anim.SetTrigger("land");
    }
}
