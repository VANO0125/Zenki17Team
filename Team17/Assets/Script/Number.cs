using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Number : MonoBehaviour
{
    public Sprite[] numbers;
    private Image[] score = new Image[10];
    private int count = 9;
    private int damage;
    private int digit;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 10; i++)
            score[i] = transform.GetChild(i).gameObject.GetComponent<Image>();
    }

    public void Set(int score)
    {
        count = 9;
         digit += score;
        //要素数0には１桁目の値が格納
        List<int> number = new List<int>();
        while (digit != 0)
        {
            score = digit % 10;
            digit = digit / 10;
            number.Add(score);
            this.score[count].sprite = numbers[score];
            count--;
        }
    }
}
