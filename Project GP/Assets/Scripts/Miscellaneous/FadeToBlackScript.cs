using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlackScript : MonoBehaviour
{
    public static bool fading;

    private void Update()
    {
        if (fading)
        {
            FadeToBlack();
        }
        if (!fading)
        {
            FadeToScreen();
        }
    }

    public void FadeToBlack()
    {
        GetComponent<Image>().CrossFadeAlpha(1, 1.0f, false);
    }

    public void FadeToScreen()
    {
        GetComponent<Image>().CrossFadeAlpha(0, 1.0f, false);
    }
}
