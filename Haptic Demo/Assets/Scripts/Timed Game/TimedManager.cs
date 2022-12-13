using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TimedManager : MonoBehaviour
{
    //Countdown timer variables
    public Slider timeSlider;
    public TMP_Text timerText;
    public  float gameTime;
    private float time;
    private bool stopTimer;

    //Arduino variables
    public SerialControl arduino;
    private bool oneTime = false;    
    
    //Game variables
    public GameObject[] buttons;
    public GameObject game;
    public GameObject gameOver;
    private int randomIndex;
    private int prevIndex = -1;
    private int points = 0;
    public TMP_Text finalScore;
    public float beginDelay = 3f;

    
    // Start is called before the first frame update
    void Start()
    {
        stopTimer = true;
        timeSlider.maxValue = gameTime;
        timeSlider.value = gameTime;
        time = gameTime;        
        
    }
    //Update is called once per frame
    void Update()
    {        
        //Countdown Timer code
        if (!stopTimer)
        {
            if (time >= 0)
            {
                time -= Time.deltaTime;
                                
                if (!oneTime)
                {
                    Debug.Log("Arduio begins");
                    //arduino.TimerBegin();         //If Arduino is connected to PC and using Serial Communication
                    oneTime = true;
                }
            }
            else
            {
                Debug.Log("Timer finish");
                stopTimer = true;
                game.SetActive(false);
                gameOver.SetActive(true);
                finalScore.text = points.ToString();
            }
            UpdateTimer(time);
            timeSlider.value = time;
        }
    }
    //Sets times to string to be displayed in-game
    void UpdateTimer(float currentTime)
    {
        currentTime++;

        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime - minutes * 60f);

        timerText.text = seconds.ToString();
    }
    
    //Method 'Begin' button in 'Menu' gameObject 
    public void BeginGame()
    {
        StartCoroutine(DelayAction(beginDelay));                
    }

    //Method 'Try Again' button in 'Game Over' gameObject
    public void TryAgain()
    {
        stopTimer = true;
        time = gameTime;
        points = 0;
        gameOver.SetActive(false);
        ResetButtons();
        //arduino.TimerRestart();               //If Arduino is connected to PC and using Serial Communication
        BTScanSceneManager.TimerRestart();
    }
    
    //Method 'Quit' button in 'Game Over' gameObject 
    public void Quit()
    {
        stopTimer = true;
        time = gameTime;
        points = 0;
        //arduino.TimerRestart();               //If Arduino is connected to PC and using Serial Communication
        BTScanSceneManager.TimerRestart();
        SceneManager.LoadScene("Main Menu");
    }

    //Game start delay
    IEnumerator DelayAction (float delay)
    {
        yield return new WaitForSeconds(delay);
        BTScanSceneManager.TimerBegin();
        stopTimer = false;
        RandomButtonStart();
    }
    
    //Initialise buttons
    public void RandomButtonStart()
    {        
        randomIndex = Random.Range(0, buttons.Length);

        ResetButtons();

        //So the same button doesn't come up too many times
        if (randomIndex != prevIndex)
        {
            prevIndex = randomIndex;
            buttons[randomIndex].GetComponent<Image>().color = Color.green;
            buttons[randomIndex].GetComponent<Button>().enabled = true;
        }
        else
        {
            randomIndex = Random.Range(0, buttons.Length);
            prevIndex = randomIndex;
            buttons[randomIndex].GetComponent<Image>().color = Color.green;
            buttons[randomIndex].GetComponent<Button>().enabled = true;
        }        
    }
    
    //Reset buttons
    void ResetButtons()
    {
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().enabled = false;
            button.GetComponent<Image>().color = Color.red;
        }
    }
    
    //Rondomly choose next button to light up after previous button has been pressed
    public void RandomButtonClick()
    {
        int current = randomIndex;
        Debug.Log("Current index: " + current);
        points++;
        

        if (buttons[randomIndex].GetComponent<Image>().color == Color.green)
        {
            buttons[randomIndex].GetComponent<Image>().color = Color.red;

            RandomButtonStart();
            Debug.Log("New index: " + randomIndex);
            Debug.Log("Points: " + points);
        }
    }
}
