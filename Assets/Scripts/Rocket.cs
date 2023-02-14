using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    
    public GameObject rocket;
    public GameObject player;
    public Rigidbody2D rb;
    public Vector2 dir;
    public float speed;

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
        

        

    }
    // Update is called once per frame
    void Update()
    {
        rb.AddForce(dir*speed);
        
    }
}
