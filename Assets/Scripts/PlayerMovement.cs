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

    public float maxWalkSpeed;

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
    void Update()
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
        
        if ((rb.velocity.y < .05 && rb.velocity.y > -.05))
        {
            rb.velocity = new Vector2(rb.velocity.x, jump * jumpSpeed);

        }
        
        rb.AddForce(new Vector2(move * acceleration, 0));
        if (rb.velocity.x > maxWalkSpeed)
        {
            rb.velocity = new Vector2(maxWalkSpeed,rb.velocity.y);
        } else if (rb.velocity.x < -maxWalkSpeed)
        {
            rb.velocity = new Vector2(-maxWalkSpeed, rb.velocity.y);
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
