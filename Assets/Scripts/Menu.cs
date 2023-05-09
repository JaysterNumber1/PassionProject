using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public  class Menu : MonoBehaviour
{
 
    [SerializeField]
    public  string gameScene;
    [SerializeField]
    public  string menuScene;




    public  void Start()
    {
        gameScene = "Levels";
        menuScene = "Menu";
      
    }


    public  void onPlayButton()
    {

        SceneManager.LoadScene(gameScene);
    }

    public  void onMenuButton()
    {
        SceneManager.LoadScene(menuScene);
    }

    public  void onQuitButton()
    {
        Application.Quit();

    }




}
