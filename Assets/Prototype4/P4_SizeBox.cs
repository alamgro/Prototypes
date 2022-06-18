using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class P4_SizeBox : MonoBehaviour
{
    private int numSize;
    //[SerializeField] private Image[] letterSize ;
    [SerializeField] private TextMeshProUGUI letterText;
    //[SerializeField] private Text letterText;

    enum TypeSize
    {
        s = 1,
        m = 2,
        l = 3,
        xl = 4,
    }

    private void Start()
    {
        numSize = 0;
    }
    private void OnMouseDown()
    {

        if (numSize < 4)
            numSize++;
        else
            numSize = 1;

        switch(numSize)
        {
            case 1:
                transform.localScale = Vector2.one * 1;
                letterText.text = 's'.ToString();
                break;

            case 2:
                transform.localScale = Vector2.one * 1.5f;
                letterText.text = "m";

                break;

            case 3:
                transform.localScale = Vector2.one * 2f;
                letterText.text = 'L'.ToString();

                break;

            case 4:
                transform.localScale = Vector2.one * 2.5f;
                letterText.text = "xl".ToString();

                break;
        }


    }
}
