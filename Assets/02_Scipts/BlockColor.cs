using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockColor : MonoBehaviour
{
    SpriteRenderer spriteR;

    string[] colorArr = new string[6] {"Red", "Green", "Blue", "Yellow", "Sky", "Purple"};
    public Sprite[] colorSprite = new Sprite[6];
    public string randColor;

    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        ChangeColor();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeColor()
    {
        int randomIndex = Random.Range(0, colorArr.Length);
        randColor = colorArr[randomIndex];
        
        if (randColor == "Red")
        {
            spriteR.sprite = colorSprite[0]; //Sprite º¯°æ
            //spriteR.color = Color.red;   
        }
        else if (randColor == "Green")
        {
            spriteR.sprite = colorSprite[1];
            //spriteR.color = Color.green;
        }
        else if (randColor == "Blue")
        {
            spriteR.sprite = colorSprite[2];
            //spriteR.color = Color.blue;
        }
        else if (randColor == "Yellow")
        {
            spriteR.sprite = colorSprite[3];
            //spriteR.color = Color.yellow;
        }
        else if (randColor == "Sky")
        {
            spriteR.sprite = colorSprite[4];
            //spriteR.color = Color.cyan;
        }
        else if (randColor == "Purple")
        {
            spriteR.sprite = colorSprite[5];
            //spriteR.color = new Color(0.6f, 0f, 0.8f);
        }

    }


}
