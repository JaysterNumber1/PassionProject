using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OnGameLoad : MonoBehaviour
{

    public GameObject diePanel;





    // Start is called before the first frame update
    void Start()
    {
        diePanel.SetActive(false);
    }

    public void Die()
    {
        diePanel.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
