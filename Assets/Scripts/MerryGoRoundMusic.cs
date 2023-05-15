using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerryGoRoundMusic : MonoBehaviour
{

    private AudioSource music;
    // Start is called before the first frame update
    void Start()
    {
        music = GetComponent<AudioSource>();
        music.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!music.isPlaying)
        {
            if (music.pitch < 3)
            {
                music.pitch += .025f;
            }
            music.Play();

        }
    }
}

