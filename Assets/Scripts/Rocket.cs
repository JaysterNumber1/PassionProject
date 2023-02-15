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



    // Start is called before the first frame update
    void Awake()
    {


     
        rb = GetComponent<Rigidbody2D>();

        player = GameObject.Find("Player");
        dir = player.GetComponent<PlayerMovement>().pos;
        dir -= new Vector2(Screen.width / 2, Screen.height / 2);
        //Debug.DrawLine(player.transform.position, dir);
        //dir.x -= player.transform.position.x;
        //dir.y -= player.transform.position.y;
        
        Debug.Log(dir);
        dir.Normalize();
        rocket.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x)*180/Mathf.PI); 
        rb.velocity = player.GetComponent<Rigidbody2D>().velocity;
        

    }
    // Update is called once per frame
    void Update()
    {
        rb.AddForce(dir*speed);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        GameObject expol = Instantiate(explode);
        expol.transform.position= this.gameObject.transform.position;
        Explosion();
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
}
