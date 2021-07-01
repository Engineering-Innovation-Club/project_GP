using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void NewGame()
    {
        SceneManager.LoadScene("lab");
    }

    public void Continue()
    {
        SceneManager.LoadScene("lab");
    }

    public void Settings()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }
}
