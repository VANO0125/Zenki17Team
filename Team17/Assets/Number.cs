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

    // Use this for initialization
    void Start()
    {
    }

    public void Set(int score)
    {
        for (int i = 0; i < 10; i++)
            this.score[i] = transform.GetChild(i).gameObject.GetComponent<Image>();
        var digit = score;
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
        // Destroy(gameObject,1f);
    }
}
