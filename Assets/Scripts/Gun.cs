using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Camera cam;
    public enum Axis { x, y }
    public Axis axis = Axis.y;
    public bool inverted;
    public GameObject player;
    public GameObject gun;

    private PlayerMovement mouse;
    private Vector3 lookAtPosition;
    private Vector2 dir;

    private void Start()
    {
        player = GameObject.Find("Player");
       
    }

    private void Update()
    {
        if (cam == null)
        {
            Debug.LogError(gameObject.name + " target missing!");
            return;
        }
        // store mouse pixel coordinates
        mouse = player.GetComponent<PlayerMovement>();

        // distance in z between this object and the camera
        // so it always align with the object

        dir = mouse.pos;
        dir -= new Vector2(Screen.width/2, Screen.height/2); 

        // transform mousePosition from screen pixels to world position
        //lookAtPosition = cam.ScreenToWorldPoint(dir);

        // Calculate normalized direction
        //Vector2 direction = (dir - transform.position).normalized;

        Debug.DrawRay(transform.position, dir * 20f, Color.blue);


        //Debug.Log(direction);
        //transform.up = direction; // Point x axis towards direction

        gun.transform.eulerAngles = new Vector3(0,0, Mathf.Atan2(dir.y, dir.x) * 180 / Mathf.PI);
  

        
        if (mouse.pos.x < Screen.width/2)
        {

           gun.GetComponent<SpriteRenderer>().flipY = true;
        }
        else
        {
            gun.GetComponent<SpriteRenderer>().flipY = false;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(lookAtPosition, 0.2f);
    }
}
