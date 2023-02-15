using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement instance;

    private Input input;

    public GameObject player;

    private Rigidbody2D rb;

    public GameObject rocket;
    
    public float acceleration = 20;
    public float jumpSpeed= 20;
    public float move;
    public float jump;
    public float click;
    public Vector2 pos;
    [SerializeField] private bool isGrounded;

    public float maxWalkSpeed;
    public float maxSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
      
        input = new Input();
        input.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
       
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        input.Movement.Side.performed += input => move = input.ReadValue<float>();
        input.Movement.Jump.performed += input => jump = input.ReadValue<float>();

        input.Mouse.Click.performed += input => click = input.ReadValue<float>();
        input.Mouse.Position.performed += input => pos = input.ReadValue<Vector2>();

        HandleMovement();
        HandleMouse();
        
        
    }

    void HandleMovement()
    {
            isGrounded = (Physics2D.Raycast((new Vector2(this.transform.position.x, this.transform.position.y+1f)), Vector3.down, 2f, 1 << LayerMask.NameToLayer("Ground"))); // raycast down to look for ground is not detecting ground? only works if allowing jump when grounded = false; // return "Ground" layer as layer

        Debug.Log(rb.velocity.magnitude);
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jump * jumpSpeed);

        } 
        if(rb.velocity.magnitude < maxWalkSpeed)
        {
            rb.AddForce(new Vector2(move * acceleration, 0));
        }
        if (rb.velocity.magnitude > maxWalkSpeed && rb.velocity.x > 0 && move < 0)
        {
            rb.AddForce(new Vector2(move * acceleration, 0));
        }
        if (rb.velocity.magnitude > maxWalkSpeed && rb.velocity.x < 0 && move > 0)
        {
            rb.AddForce(new Vector2(move * acceleration, 0));
        }
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxWalkSpeed);
        }
        



    }
    void HandleMouse()
    {
        if (click == 1)
        {
            Instantiate(rocket, player.transform.position, Quaternion.identity);
            click = 0;
        }
    }
    



}
