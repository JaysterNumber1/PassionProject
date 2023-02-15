using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClipManager : MonoBehaviour
{


    public int bullets;
    public int maxBullets;
    public GameObject player;
    public GameObject bullet;
    public GameObject bulletClone;
    public GameObject clipManager;

    public Sprite cartidge;
    public Sprite casing;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        bullets = player.GetComponent<PlayerMovement>().shotCount;
        maxBullets = bullets;
        for (int i = 1; i <= bullets; i++)
        {
            bulletClone = Instantiate(bullet, new Vector3(bullet.transform.position.x+2*i*bullet.GetComponent<RectTransform>().rect.x, bullet.transform.position.y, bullet.transform.position.z), clipManager.transform.rotation,clipManager.transform);
            bulletClone.name = (bullet.name + i);
        }

    }

    public void DecreaseShot()
    {
        bullets = player.GetComponent<PlayerMovement>().shotCount;

        if (GameObject.Find("Bullet" + (bullets + 1)) != null)
        {
            bulletClone = GameObject.Find("Bullet" + (bullets + 1));

            bulletClone.GetComponent<Image>().sprite = casing;
        }
       


    }

    public void IncreaseShot()
    {
        bullets = player.GetComponent<PlayerMovement>().shotCount;

        Debug.Log((bullets));
        if(GameObject.Find("Bullet" + (bullets)) != null)
        {
            bulletClone = GameObject.Find("Bullet" + (bullets));
            bulletClone.GetComponent<Image>().sprite = cartidge;
        }
    

       
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
