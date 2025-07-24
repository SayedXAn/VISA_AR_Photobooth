using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
    }

    /*public void SwitchDisplay(int displayIndex)
    {
        if (displayIndex >= 0 && displayIndex < Display.displays.Length)
        {
            // Change the current display
            Screen.SetResolution(Display.displays[displayIndex].systemWidth,
                                 Display.displays[displayIndex].systemHeight,
                                 FullScreenMode.FullScreenWindow, 60);
            Debug.Log($"Switched to display {displayIndex}");
        }
        else
        {
            Debug.LogWarning($"Display index {displayIndex} is out of range. There are only {Display.displays.Length} displays available.");
        }
    }*/
}
