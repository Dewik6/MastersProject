using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimedMenu : MonoBehaviour
{
    //Method 'Quit' button in 'Menu' gameObject
    public void Quit()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
