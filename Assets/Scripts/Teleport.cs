using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject TeleportLocation;

    public void TeleportTo(GameObject obj)
    {
        obj.transform.position = TeleportLocation.transform.position;

        Debug.Log("Teleport " + obj.gameObject.name);
    }
}
