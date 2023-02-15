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

    public GameObject clipManager;
    
    public float acceleration = 20;
    public float jumpSpeed= 20;
    public float move;
    public float jump;
    public float click;
    public Vector2 pos;
    [SerializeField] private bool isGrounded;
    //[SerializeField] private bool isLeftWall;
    //[SerializeField] private bool isRightWall;

    public float maxWalkSpeed;
    public float maxSpeed;
    public float floorDrag;

    public float reloadShot;
    public float timer;
    public int shotCount =3;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        clipManager = GameObject.Find("GunClip");
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

        if (isGrounded && rb.velocity.x > 0) rb.velocity = new Vector2(rb.velocity.x - floorDrag, rb.velocity.y);
        if (isGrounded && rb.velocity.x < 0) rb.velocity = new Vector2(rb.velocity.x + floorDrag,rb.velocity.y);

        HandleMovement();
        HandleMouse();
        
        
    }

    void HandleMovement()
    {
            isGrounded = (Physics2D.Raycast((new Vector2(this.transform.position.x, this.transform.position.y)), Vector3.down, 1f, 1 << LayerMask.NameToLayer("Ground"))); // raycast down to look for ground is not detecting ground? only works if allowing jump when grounded = false; // return "Ground" layer as layer
            //isLeftWall = (Physics2D.Raycast((new Vector2(this.transform.position.x, this.transform.position.y -.5f)), Vector3.left, .55f, 1 << LayerMask.NameToLayer("Ground")));
            //isRightWall = (Physics2D.Raycast((new Vector2(this.transform.position.x, this.transform.position.y - .5f)), Vector3.right, .55f, 1 << LayerMask.NameToLayer("Ground")));

        //Debug.Log(rb.velocity.magnitude);
        //if grounded and trying to jump, jump
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jump * jumpSpeed);

        }
        if (rb.velocity.x > 0)
        {
            player.GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
        else if (rb.velocity.x < 0)
        {
            player.GetComponentInChildren<SpriteRenderer>().flipX = true;
        }




        if (rb.velocity.magnitude < maxWalkSpeed)
        {
            rb.AddForce(new Vector2(move * acceleration, 0));
        }
        if (rb.velocity.x > maxWalkSpeed && rb.velocity.x > 0 && move < 0)
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
        if (click == 1&&shotCount!=0&&timer>=reloadShot)
        {
            Instantiate(rocket, new Vector3(player.transform.position.x, player.transform.position.y - .25f, player.transform.position.z), Quaternion.identity);
            
            shotCount--;

            clipManager.GetComponent<ClipManager>().DecreaseShot();

            //click = 0;
            timer = 0;
        } else
        if (timer < reloadShot)
        {
            timer += Time.deltaTime;
        }
    }

    public void IncreaseShot()
    {
        clipManager.GetComponent<ClipManager>().IncreaseShot();
    }
    



}
