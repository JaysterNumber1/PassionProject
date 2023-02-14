using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject TeleportLocation;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.transform.position = TeleportLocation.transform.position;
  
        Debug.Log("Teleport " + collision.gameObject.name);
    }
}
