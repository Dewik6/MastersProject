using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMain : MonoBehaviour
{
    //Method to direct user to 'Timed Demo' from Main Menu
    public void Timed()
    {
        SceneManager.LoadScene("Timed Game");
    }

    //Method to direct user to 'Swipe Demo' from Main Menu
    public void SwipeDemo()
    {
        SceneManager.LoadScene("Swipe Demo");
    }

    //Method to direct user to 'BT Scan Scene' from Main Menu
    public void ConnectBT()
    {
        SceneManager.LoadScene("BT Scan Scene");
    }
    
}
