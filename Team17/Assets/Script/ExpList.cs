using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpList : MonoBehaviour
{
    public List<int> expList;
    
    // Start is called before the first frame update
    void Start()
    {
        expList = new List<int>(20)
        {
            20,//Lr1
            30,//Lr2
            50,//Lr3
            100,//Lr4
            120,//Lr5
            150,//Lr6
            190,//Lr7
            240,//Lr8
            300,//Lr9
            360,//Lr10
            400,//Lr11
            430,//Lr12
            450,//Lr13
            500,//Lr14
            520,//Lr15
            550,//Lr16
            590,//Lr17
            620,//Lr18
            660,//Lr19
            700//Lr20
        };
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
