using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArduinoBluetoothAPI;
using System;
using UnityEngine.UI;
using System.Runtime.InteropServices;

//Script to allow Bluetooth functionality and communication between Unity game and Arduino via smartphone/tablet
//This script was provided by the Arduino Bluetooth Plugin for Unity,
// and modified for to be used for the Haptic Demo
public class BTScanSceneManager : MonoBehaviour
{
    //To allow other scripts access to this class' methods
    public static BTScanSceneManager BTManager;
    // Use this for initialization
    public static BluetoothHelper bluetoothHelper;
    string deviceName = "Nano 33 IoT";

    public Text text;
    public GameObject sphere;

    string received_message;

    void Start()
    {
        Reconnect();        
    }


    //Asynchronous method to receive messages
    void OnMessageReceived()
    {
        //StartCoroutine(blinkSphere());
        received_message = bluetoothHelper.Read();
        text.text = received_message;
        Debug.Log(System.DateTime.Now.Second);
        //Debug.Log(received_message);
    }

    void OnScanEnded(LinkedList<BluetoothDevice> nearbyDevices)
    {
        text.text = "Found " + nearbyDevices.Count + " devices";
        if (nearbyDevices.Count == 0){
            bluetoothHelper.ScanNearbyDevices();
            return;
        }


        foreach(BluetoothDevice device in nearbyDevices)
        {
            if(device.DeviceName == deviceName)
                Debug.Log("FOUND!!");
        }
            
        text.text = deviceName;
        bluetoothHelper.setDeviceName(deviceName);
        bluetoothHelper.Connect();
        bluetoothHelper.isDevicePaired();
    }

    /*void Update()
    {
        //Debug.Log(bluetoothHelper.IsBluetoothEnabled());
        if (!bluetoothHelper.IsBluetoothEnabled())
        {
            bluetoothHelper.EnableBluetooth(true);
        }
    }*/

    void OnConnected()
    {
        sphere.GetComponent<Renderer>().material.color = Color.green;
        try
        {
            bluetoothHelper.StartListening();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    void OnConnectionFailed()
    {
        sphere.GetComponent<Renderer>().material.color = Color.red;
        Debug.Log("Connection Failed");
    }

    //BT Scan Scene GUI
    void OnGUI()
    {
        if (bluetoothHelper != null)
            bluetoothHelper.DrawGUI();
        else
            return;

        if (!bluetoothHelper.isConnected())
            if (GUI.Button(new Rect(Screen.width / 2 - Screen.width / 10, Screen.height / 10, Screen.width / 5, Screen.height / 10), "Connect"))
            {
                Reconnect();
            }
            else
            { 
                sphere.GetComponent<Renderer>().material.color = Color.magenta;
            }

        if (bluetoothHelper.isConnected())
            if (GUI.Button(new Rect(Screen.width / 2 - Screen.width / 10, Screen.height - 2 * Screen.height / 10, Screen.width / 5, Screen.height / 10), "Disconnect"))
            {
                bluetoothHelper.Disconnect();
                sphere.GetComponent<Renderer>().material.color = Color.blue;
            }

        if (bluetoothHelper.isConnected())
            if (GUI.Button(new Rect(Screen.width / 2 - Screen.width / 10, Screen.height / 10, Screen.width / 5, Screen.height / 10), "Send text"))
            {
                bluetoothHelper.SendData("2");
                //bluetoothHelper.SendData("This is a very long long long long text");
            }
    }

    //Timed Demo commands
    public static void TimerBegin()
    {
        if (bluetoothHelper.isConnected())
        {
            bluetoothHelper.SendData("f");
        }
        
    }
    public static void TimerRestart()
    {
        if (bluetoothHelper.isConnected())
        {
            bluetoothHelper.SendData("b");
        }
    }

    //Swipe Demo commands
    public static void SwipeStart()
    {
        if (bluetoothHelper.isConnected())
        {
            bluetoothHelper.SendData("4");
        }
    }
    public static void SwipeRight()
    {
        if (bluetoothHelper.isConnected())
        {
            bluetoothHelper.SendData("r");
        }
    }
    public static void SwipeLeft()
    {
        if (bluetoothHelper.isConnected())
        {
            bluetoothHelper.SendData("l");
        }
    }
    public static void SwipeExit()
    {
        if (bluetoothHelper.isConnected())
        {
            bluetoothHelper.SendData("e");
        }
    }

    //Connection process to Arduino using Bluetooth
    void Reconnect()
    {
        try
        {
            // Debug.Log(getNumber());
            Debug.Log(Application.unityVersion);
            BluetoothHelper.BLE = true;  //use Bluetooth Low Energy Technology
            bluetoothHelper = BluetoothHelper.GetInstance();
            bluetoothHelper.OnConnected += OnConnected;
            bluetoothHelper.OnConnectionFailed += OnConnectionFailed;
            bluetoothHelper.OnDataReceived += OnMessageReceived; //read the data
            bluetoothHelper.OnScanEnded += OnScanEnded;

            BluetoothHelperCharacteristic characteristic = new BluetoothHelperCharacteristic("2A57"); 
            characteristic.setService("180A"); 
            bluetoothHelper.setTxCharacteristic(characteristic);
            bluetoothHelper.setRxCharacteristic(characteristic);

            bluetoothHelper.setTerminatorBasedStream("\n");
            
            if (!bluetoothHelper.ScanNearbyDevices())
            {
                sphere.GetComponent<Renderer>().material.color = Color.black;

                bluetoothHelper.setDeviceName(deviceName);
                bluetoothHelper.Connect();
            }
            else
            {
                text.text = "start scan";
                // sphere.GetComponent<Renderer>().material.color = Color.green;
            }

        }
        catch (BluetoothHelper.BlueToothNotEnabledException ex)
        {
            sphere.GetComponent<Renderer>().material.color = Color.yellow;
            Debug.Log(ex.ToString());
            text.text = ex.Message;
        }
    }

    /*void OnDestroy()
    {
        if (bluetoothHelper != null)
            bluetoothHelper.Disconnect();
    }*/
}
