using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwipeCard : MonoBehaviour
{
    public bool isMouseOver = false;
    public TMP_Text text;

    //Checks if the mouse is over the card
    private void OnMouseOver()
    {
        isMouseOver = true;
    }

    //CHecks when the mouse is no longer over the card
    private void OnMouseExit()
    {
        isMouseOver = false;
    }
    
    //Changing text and colour of the card when swiped right
    public void Right()
    {
        text.text = "Right";
        text.color = Color.red;
    }

    //Changing text and colour of the card when swiped left
    public void Left()
    {
        text.text = "Left";
        text.color = Color.green;
    }

    //Setting the colour of the card when it is in starting position
    public void Middle()
    {
        text.color = Color.white;
        
    }
}
