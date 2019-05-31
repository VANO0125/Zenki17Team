using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GamepadInput;


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
        //if (GamePad.GetAnyButton(GamePad.Button.B))
        //0 =B,1=A,2=X,3=Y,7=start
            if(
            Input.GetKeyDown(KeyCode.JoystickButton0) ||
            Input.GetKeyDown(KeyCode.JoystickButton1) ||
            Input.GetKeyDown(KeyCode.JoystickButton2) ||
            Input.GetKeyDown(KeyCode.JoystickButton3) ||
            Input.GetKeyDown(KeyCode.JoystickButton7) )
        {
            button.onClick.Invoke();
        }
       
    }
    public void ReStart()
    {
        SceneManager.LoadScene(1);
    }
}


