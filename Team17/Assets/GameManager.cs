using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isStart;

    // Start is called before the first frame update
    void Start()
    {
        isStart = false;
    }

    void SetStart()
    {
        isStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
