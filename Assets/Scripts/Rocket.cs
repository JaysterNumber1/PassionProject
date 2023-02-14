using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    
    public GameObject rocket;
    public GameObject player;
    public Rigidbody2D rb;
    public Vector2 pos;

    // Start is called before the first frame update
    void Awake()
    {


        input = new Input();
        input.Enable();
        rb = GetComponent<Rigidbody2D>();
       
        Debug.Log(pos);
        //rocket.transform.position = player.transform.position;
        //rocket.transform.eulerAngles = new Vector3(0, 0, 90);

    }
    // Update is called once per frame
    void Update()
    {
        rb.AddForce(new Vector2(10, 0));
        
    }
}
