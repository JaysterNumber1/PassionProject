using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public GameObject particleBreak;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Explosion")
        {
            GameObject BrokenBox = Instantiate(particleBreak);
            BrokenBox.transform.position = this.gameObject.transform.position;
            Destroy(this.gameObject);
        }
    }
}
