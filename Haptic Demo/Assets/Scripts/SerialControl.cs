using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

//Script to allow Serial Communication between Unity and Arduino
public class SerialControl : MonoBehaviour
{
    //Seial Communication information to connect to arduino via serial port
    public static SerialPort arduino = new SerialPort("COM6", 9600);

    // Start is called before the first frame update
    void Start()
    {
        //Check if Arduino is available
        if (arduino.IsOpen == false)
        {
            arduino.Open();
        }       
    }
   
    //LED blink test
    public void LightOn()
    {
        arduino.Write("1");
    }
    public void LightOff()
    {
        arduino.Write("0");
    }
    public void LightBlink()
    {
        arduino.Write("2");
    }

    //Swipe Demo controls
    public void SwipeStart()
    {
        arduino.Write("4");
    }
    public void SwipeRight()
    {
        arduino.Write("r");
    }
    public void SwipeLeft()
    {
        arduino.Write("l");
    }
    public void SwipeExit()
    {
        arduino.Write("e");        
    }

    //Timed Demo controls
    public void TimerBegin()
    {
        arduino.Write("f");
    }
    public void TimerRestart()
    {
        arduino.Write("b");
    }
}
