using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwipeManager : MonoBehaviour
{
    //Card variables
    public GameObject card;
    public SwipeCard swipe;
    public SpriteRenderer sprite;    
    public float cardSpeed = 1f;

    //Card triggers
    public float triggerMargin = 2f;
    public float textMargin = 1f;    
    private int direction = 0;

    //Arduino variables
    public SerialControl arduino;
    private bool oneTime = false;

    // Start is called before the first frame update
    void Start()
    {
        //arduino.SwipeStart();
        BTScanSceneManager.SwipeStart();
        sprite = card.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Allowing the card to move where the user wants it
        if (Input.GetMouseButton(0) && swipe.isMouseOver)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            card.transform.position = pos;
        }
        //Setting card position back to the middle
        else
        {
            card.transform.position = Vector2.MoveTowards(card.transform.position, new Vector2(0, 0), cardSpeed);
        }

        //Swipe Right
        if (card.transform.position.x > textMargin)
        {
            swipe.Right();
            if (card.transform.position.x > triggerMargin)
            {
                sprite.color = Color.green;
                direction = 1;
                if (!oneTime)
                {
                    Debug.Log("Right");
                    //arduino.SwipeRight();                 //If Arduino is connected to PC and using Serial Communication
                    BTScanSceneManager.SwipeRight();
                    oneTime = true;
                }

                //When the card is released after swiping past the trigger distance
                if (!Input.GetMouseButton(0))
                {
                    //Animations for card falling off screen *Still in progress but not required*
                    //animator.SetTrigger("Right");
                    
                }
            }            
        }
        //Swipe Left
        else if (card.transform.position.x < -textMargin)
        {
            swipe.Left();
            if (card.transform.position.x < -triggerMargin)
            {
                sprite.color = Color.red;
                direction = -1;
                if (!oneTime)
                {
                    Debug.Log("Left");
                    //arduino.SwipeLeft();              //If Arduino is connected to PC and using Serial Communication
                    BTScanSceneManager.SwipeLeft();
                    oneTime = true;
                }

                //When the card is released after swiping
                if (!Input.GetMouseButton(0))
                {
                    //Animations for card falling off screen
                    //animator.SetTrigger("Left");
                    
                }
            }            
        }
        //Middle
        else
        {
            sprite.color = Color.white;
            swipe.Middle();            
            if (direction == 1)
            {
                //arduino.SwipeLeft();
                BTScanSceneManager.SwipeLeft();
                direction = 0;
            }
            else if (direction == -1)
            {
                //arduino.SwipeRight();
                BTScanSceneManager.SwipeRight();
                direction = 0;
            }
            
            oneTime = false;
            //Debug.Log("Middle");
        }
    }

    //Exit to Main Menu scene
    public void Quit()
    {
        //arduino.SwipeExit();  
        BTScanSceneManager.SwipeExit();         //Resetting haptic device to starting position
        SceneManager.LoadScene("Main Menu");        
    }
}
