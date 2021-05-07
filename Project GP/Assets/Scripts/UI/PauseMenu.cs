using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused;

    public GameObject pauseMenuUI;

    public float PreviousTimeScale;

    public GameObject settings;
    public GameObject buttons;
    public GameObject sButton;
    public GameObject sTitle;

    private Vector3 oldPos;
    private Vector3 newPos;

    private void Start()
    {
        isPaused = false;

        oldPos = buttons.transform.position;
        newPos = new Vector3(buttons.transform.position.x, buttons.transform.position.y - 225);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
       
        Time.timeScale = 0;
        isPaused = true;
    }

    public void loadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main menu");
    }

    public void ToggleScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void ToggleSettings()
    {
        if (settings.activeSelf)
        {
            // Close Settings
            settings.SetActive(false);
            sTitle.SetActive(true);
            sButton.SetActive(false);
            buttons.transform.position = oldPos;
        }
        else if (!settings.activeSelf)
        {
            // Open Settings
            settings.SetActive(true);
            sTitle.SetActive(false);
            sButton.SetActive(true);
            buttons.transform.position = newPos;
        }
    }
}
