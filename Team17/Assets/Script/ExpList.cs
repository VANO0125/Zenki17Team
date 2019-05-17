using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpList : MonoBehaviour
{
    public List<int> expList;
    
    // Start is called before the first frame update
    void Start()
    {
        expList = new List<int>(10)
        {
            15,
            30,
            50,
            100,
            120,
            150,
            190,
            240,
            300,
            360
        };
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
