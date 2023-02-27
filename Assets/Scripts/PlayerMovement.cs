using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

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
    public float interact;
    public Vector2 pos;
    [SerializeField] private bool isGrounded;

    public float maxWalkSpeed;
    public float maxSpeed;
    public float floorDrag;

    public float reloadShot;
    public float timer;
    public int shotCount =3;
    public float reloadGunTime = 1;
    private float reloadTime;
    public float timeFromClickGroundedTime;
    private float timeFromClickGrounded;

    private bool canInteract;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        clipManager = GameObject.Find("GunClip");
        input = new Input();
        input.Enable();
    }

    void FixedUpdate()
    {
        input.Movement.Side.performed += input => move = input.ReadValue<float>();
        input.Movement.Jump.performed += input => jump = input.ReadValue<float>();

        input.Mouse.Click.performed += input => click = input.ReadValue<float>();
        input.Mouse.Position.performed += input => pos = input.ReadValue<Vector2>();

        input.Movement.Interact.performed += input => interact = input.ReadValue<float>();
        if(interact == 0) canInteract= true;

        HandleMovement();
        HandleMouse();

        if(isGrounded) timeFromClickGrounded += Time.deltaTime;
        if (!isGrounded) { 
            reloadTime = 0;
            timeFromClickGrounded = 0;
        }

        else if (reloadTime < reloadGunTime && shotCount < clipManager.GetComponent<ClipManager>().maxBullets) reloadTime += Time.deltaTime;

        if (isGrounded && reloadGunTime < reloadTime && shotCount < clipManager.GetComponent<ClipManager>().maxBullets && timeFromClickGrounded > timeFromClickGroundedTime)
        {
            player.GetComponent<PlayerMovement>().shotCount++;

            player.GetComponent<PlayerMovement>().IncreaseShot();

            reloadTime= 0;

        }

    }

    void HandleMovement()
    {
            isGrounded = (Physics2D.Raycast((new Vector2(this.transform.position.x, this.transform.position.y)), Vector3.down, 1f, 1 << LayerMask.NameToLayer("Ground"))); // raycast down to look for ground is not detecting ground? only works if allowing jump when grounded = false; // return "Ground" layer as layer
            
  
        //if grounded and trying to jump, jump
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jump * jumpSpeed);

        }
       
        player.GetComponentInChildren<SpriteRenderer>().flipX = rb.velocity.x < 0;


        if (Mathf.Abs(rb.velocity.x) < maxWalkSpeed)
        {
            rb.AddForce(new Vector2(move * (maxWalkSpeed - Mathf.Abs(rb.velocity.x)), 0), ForceMode2D.Impulse);
            Debug.DrawRay((new Vector2(this.transform.position.x, this.transform.position.y)), Vector3.down, Color.green, 1.0f);
        }
        else if((rb.velocity.x > 0 && move < 0) || (rb.velocity.x < 0 && move > 0) && !isGrounded)
        {
            rb.AddForce(new Vector2(move *.075f * (maxWalkSpeed + 0.5f - Mathf.Clamp(Mathf.Abs(rb.velocity.x), -1, 1)), 0), ForceMode2D.Impulse);
            Debug.DrawRay((new Vector2(this.transform.position.x, this.transform.position.y)), Vector3.down, Color.yellow, 1.0f);
        }
        else
        {
            Debug.DrawRay((new Vector2(this.transform.position.x, this.transform.position.y)), Vector3.down, Color.red, 1.0f);
        }
        //Debug.Log(rb.velocity.x);
 

    }
  
    void HandleMouse()
    {
        if (click == 1 && shotCount!=0 && timer>=reloadShot)
        {
            timeFromClickGrounded = 0;
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (interact == 1 && canInteract)
        {        
            canInteract= false;
            if(collision.gameObject.tag == "Teleport")
            {
                collision.gameObject.GetComponent<Teleport>().TeleportTo(this.gameObject);
                if (shotCount < clipManager.GetComponent<ClipManager>().maxBullets)
                {
                    player.GetComponent<PlayerMovement>().shotCount++;

                    player.GetComponent<PlayerMovement>().IncreaseShot();

                    reloadTime = 0;

                }
            }
            
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "extraBullet")
        {
            Destroy(collision.gameObject);
            clipManager.GetComponent<ClipManager>().addBullet();

        }
    }





}
