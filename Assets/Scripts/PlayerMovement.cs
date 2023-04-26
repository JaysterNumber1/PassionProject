using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    public static PlayerMovement instance;

    private Input input;

    public GameObject player;
    public SpriteRenderer playerSprite;

    private Rigidbody2D rb;

    public GameObject rocket;

    public GameObject clipManager;

    public GameObject gun;
    public SpriteRenderer gunSprite;

    public float acceleration = 20;
    public float jumpSpeed = 20;
    public float move;
    public float storeMove;
    public float jump;
    public float click;
    public float interact;
    public Vector2 pos;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool touchingLeftWall;
    [SerializeField] private bool touchingRightWall;
    public bool blastJumping;
  

    [SerializeField]
    private float blastDrag = .5f;
    [SerializeField]
    private float normalDrag = 1f;
  
  

    public float maxWalkSpeed;
    
    public float maxAirSpeed;

    public float shotCooldown;
    public float cooldownTimer;
    public int shotCount = 3;
    public float reloadGunTime = 1;
    public float reloadTimer;
    public float subsequentBulletReloadTime = .3f;

    //public float timeFromClickGroundedTime;
    //private float timeFromClickGrounded;

    private bool canInteract;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        clipManager = GameObject.Find("GunClip");
        gun = GameObject.Find("Gun");
        gunSprite = gun.GetComponentInChildren<SpriteRenderer>();
        playerSprite = player.GetComponentInChildren<SpriteRenderer>();
       input = new Input();
        input.Enable();
       
    }

    void FixedUpdate()
    {
        input.Movement.Side.performed += input => storeMove = input.ReadValue<float>();
        move = storeMove;
        input.Movement.Jump.performed += input => jump = input.ReadValue<float>();

        input.Mouse.Click.performed += input => click = input.ReadValue<float>();
        input.Mouse.Position.performed += input => pos = input.ReadValue<Vector2>();

        input.Movement.Interact.performed += input => interact = input.ReadValue<float>();
        if (interact == 0) canInteract = true;
        HandleMouse();

        
        HandleMovement();
        ReloadGun();


    }

    void HandleMovement()
    {
        isGrounded = (Physics2D.Raycast((new Vector2(this.transform.position.x, this.transform.position.y)), Vector3.down, 1f, 1 << LayerMask.NameToLayer("Ground"))); // raycast down to look for ground is not detecting ground? only works if allowing jump when grounded = false; // return "Ground" layer as layer\
        touchingRightWall = (Physics2D.Raycast((new Vector2(this.transform.position.x, this.transform.position.y)), Vector3.right, .5f, 1 << LayerMask.NameToLayer("Ground")));
        touchingLeftWall = (Physics2D.Raycast((new Vector2(this.transform.position.x, this.transform.position.y)), Vector3.left, .5f, 1 << LayerMask.NameToLayer("Ground")));

        Debug.DrawLine(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(this.transform.position.x, this.transform.position.y-1), Color.red, 1);
        Debug.DrawLine(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(this.transform.position.x+.5f, this.transform.position.y ), Color.blue, 1);
        Debug.DrawLine(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(this.transform.position.x - .5f, this.transform.position.y), Color.magenta, 1);

        #region Old Movement
        /*
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
        */
        #endregion
        #region New Movement
        
        if (touchingLeftWall && move == -1)
        {
            
            move = 0;
        }
        else if (touchingRightWall && move == 1)
        {
            move = 0;
        }
        else if(!touchingLeftWall&&!touchingRightWall)
        {
           
            move = storeMove;
        }

        if (isGrounded)
        {
            

           

            

            if (blastJumping)
            {
                rb.drag = blastDrag;
                               
            }
            else
            {
              
                rb.drag = normalDrag;

                if (rb.velocity.x >= maxWalkSpeed || rb.velocity.x <= -maxWalkSpeed)
                {
                    rb.velocity = new Vector2(maxWalkSpeed * Mathf.Sign(rb.velocity.x), 0);
                }

                if (move == 1 || move == -1)
                {
                    rb.drag = 0;
                }
              
                
            }
            if (((rb.velocity.x <= maxWalkSpeed && rb.velocity.x >= -maxWalkSpeed))&& rb.velocity.y==0 )
            {
                
                blastJumping = false;
            }

            if (move != 0 && !blastJumping)
            {
                rb.AddForce(new Vector2(move * acceleration, 0));
            }
            else 
            {
                if (move !=0 && Mathf.Sign(move)!=Mathf.Sign(rb.velocity.x)&&rb.velocity.x!=0)
                {

                
                    rb.AddForce(new Vector2(move * 1.5f * acceleration, 0));
                }
            } 
            
            if(jump != 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jump * jumpSpeed);
            }
        }
        else
        {
            rb.drag = 0;

            if((rb.velocity.x<= maxAirSpeed && rb.velocity.x >= -maxAirSpeed)) //can strafe normally
            {
                rb.AddForce(new Vector2(move * acceleration, 0));
            }
            else if (move != 0 && Mathf.Sign(move) != Mathf.Sign(rb.velocity.x) && rb.velocity.x != 0)
            {


                rb.AddForce(new Vector2(move * 1.5f * acceleration, 0));
            }
            

            
            
        }
        #endregion

    }
    
   
    private IEnumerator ChangeBlast()
    {
        yield return new WaitUntil(()=>!isGrounded);

        blastJumping = true;
        
    }
    public void RunChangeBlast()
    {
        StartCoroutine(ChangeBlast());
    }

   

    void HandleMouse()
    {
        if (click == 1 && shotCount > 0 && cooldownTimer >= shotCooldown)
        {
            //timeFromClickGrounded = 0; Not currently using
            Instantiate(rocket, gun.transform.position, Quaternion.identity);

            shotCount--;

            clipManager.GetComponent<ClipManager>().DecreaseShot();

            //click = 0;
            cooldownTimer = 0;
            reloadTimer = 0;
        }
        else
        if (cooldownTimer < shotCooldown)
        {
            cooldownTimer += Time.deltaTime;
        }
        if(rb.velocity.x > 0)
        {
            playerSprite.flipX = false;

        }
        else if (rb.velocity.x<0)
        {
            playerSprite.flipX = true;

        }
    }

    private void ReloadGun()
    {
        #region Grounded Reload
        //I don't like reloading only while grounded, I think general timing would be better.
        /*if(isGrounded) timeFromClickGrounded += Time.deltaTime; //reloading while grounded
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

        }  */
        #endregion

        #region Time Reload
        if (reloadTimer >= reloadGunTime && shotCount < clipManager.GetComponent<ClipManager>().maxBullets)
        {
            player.GetComponent<PlayerMovement>().shotCount++;

            player.GetComponent<PlayerMovement>().IncreaseShot();

            reloadTimer = subsequentBulletReloadTime;
        }
        else if (reloadTimer < reloadGunTime)
        {
            reloadTimer += Time.deltaTime;
        }
        #endregion
    }

    public void IncreaseShot()
    {
        clipManager.GetComponent<ClipManager>().IncreaseShot();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (interact == 1 && canInteract)
        {
            canInteract = false;
            if (collision.gameObject.tag == "Teleport")
            {
                collision.gameObject.GetComponent<Teleport>().TeleportTo(this.gameObject);
                if (shotCount < clipManager.GetComponent<ClipManager>().maxBullets)
                {
                    player.GetComponent<PlayerMovement>().shotCount++;

                    player.GetComponent<PlayerMovement>().IncreaseShot();

                    //reloadTime = 0; not using

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
            //Debug.Log("hihi");


        }





    }
}
