using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpList : MonoBehaviour
{
    public List<int> expList;
    
    // Start is called before the first frame update
    void Start()
    {
        expList = new List<int>(6)
        {
            1,
            3,
            50,
            100,
            120,
            150
        };
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
