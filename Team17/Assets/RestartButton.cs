﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class RestartButton : MonoBehaviour
{
   
    Button button;
    // Use this for initialization
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            button.onClick.Invoke();
        }
    }
    public void ReStart()
    {
        SceneManager.LoadScene(1);
    }
}


