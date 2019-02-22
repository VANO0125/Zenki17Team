using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Number : MonoBehaviour
{
	/*
    public Sprite numbers;
	public Sprite[] sprites;
    private GameObject[] objects = new GameObject[7];
    private int count = 6;
    private int damage;

    // Use this for initialization
    void Start()
    {
		for(int i=0;i<9;i++)
		{
		//numbers
		}
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Set(int score)
    {
        for (int i = 0; i < 7; i++)
            objects[i] = transform.GetChild(i).gameObject;
        var digit = score;
        //要素数0には１桁目の値が格納
        List<int> number = new List<int>();
        while (digit != 0)
        {
            score = digit % 10;
            digit = digit / 10;
            number.Add(score);
            objects[count].GetComponent<SpriteRenderer>().sprite= numbers[score];
            count--;
        }
        Destroy(gameObject,1f);
    }*/
}
