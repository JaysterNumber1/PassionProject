using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    
    public GameObject rocket;
    private GameObject player;
    public Rigidbody2D rb;
    public Vector2 dir;
    public float speed;
    public GameObject explode;

    Collider2D[] inexplosion = null;
    public float exploradius = 5;
    public float exploforce = 5;

    public float timer;
    public float rocketLife = 5f;


    // Start is called before the first frame update
    void Awake()
    {


        //set rb of rocket, set the player and where the mouse clicked at, then set the v2 for the rocket's path.
        rb = GetComponent<Rigidbody2D>();

        player = GameObject.Find("Player");
        dir = player.GetComponent<PlayerMovement>().pos;
        dir -= new Vector2(Screen.width / 2, Screen.height / 2);
        //Debug.DrawLine(player.transform.position, dir);
        //dir.x -= player.transform.position.x;
        //dir.y -= player.transform.position.y;
        
        //Debug.Log(dir);
        dir.Normalize();
        rocket.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x)*180/Mathf.PI);
        //Takes the Player velocity in account.
        //rb.velocity = player.GetComponent<Rigidbody2D>().velocity;
        //Make the rocket start out at a faster speed, can adjust later down the line
        rb.velocity = dir*speed;
        
    }
    // Update is called once per frame
    void Update()
    {
        //rb.AddForce(dir*speed);

        //USE IF NOT USING PLAYER VELOCITY FOR INITIAL LAUNCH
        if (rb.velocity.y < (dir.y * speed))
        {
            //Increase y direction velocity
            rb.velocity = new Vector2(rb.velocity.x, dir.y * speed);
        }
        if (rb.velocity.x < dir.x * speed)
        {
            //increase x direction velocity
            rb.velocity = new Vector2(dir.x * speed,rb.velocity.y);
        }
        
        RocketTimer();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        GameObject expol = Instantiate(explode);
        expol.transform.position= this.gameObject.transform.position;
        Explosion();
        player.GetComponent<PlayerMovement>().shotCount++;

        player.GetComponent<PlayerMovement>().IncreaseShot();

        Destroy(rocket);
    }

    public void Explosion()
    {
        inexplosion = Physics2D.OverlapCircleAll(transform.position, exploradius);

        foreach (Collider2D o in inexplosion)
        {
            Rigidbody2D rb = o.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 distex = o.transform.position - transform.position;
                if (distex.magnitude > 0)
                {
                    rb.AddForce(distex.normalized * exploforce);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, exploradius);
    }

    private void RocketTimer()
    {
        // if the timer has been going longer than the rocket's life, destroy the rocket
        if (timer > rocketLife)
        {
            player.GetComponent<PlayerMovement>().shotCount++;

            player.GetComponent<PlayerMovement>().IncreaseShot();

            Destroy(rocket);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

}
