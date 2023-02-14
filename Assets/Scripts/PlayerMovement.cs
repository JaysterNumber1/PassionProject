using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement instance;

    private Input input;

    private Rigidbody2D rb;
    
    public float acceleration = 20;
    public float jumpSpeed= 20;
    public float move;
    public float jump;
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
        HandleMovement();
           
        
        
    }

    void HandleMovement()
    {
        if ((rb.velocity.y < .05 && rb.velocity.y > -.05) && jump != 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jump * jumpSpeed);

        }
        else
        {
            jump = 0;
        }
        rb.velocity =new Vector2(move * acceleration, rb.velocity.y);
        if (Mathf.Abs(rb.velocity.x) > maxWalkSpeed)
        {
            rb.velocity = new Vector2(maxWalkSpeed*move,rb.velocity.y);
        }


        if (move == 0)
        {
            rb.velocity = new Vector2(0,rb.velocity.y);
        }
    }




}
