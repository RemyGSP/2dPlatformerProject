using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    public void ExitGame()
    {
        Application.Quit();
        Debug.Break();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void StopGame()
    {
        Time.timeScale = 0;
    }

    public void Options()
    {

    }
}
