using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject defaultBG;
    public GameObject lowerPanel; // initial panel
    public GameObject postCapturePanel;
    public GameObject chooseFramesPanel;
    public GameObject finalPanel;

    public float placeLen = 0.5f;
    void Start()
    {
        instance = this;
    }

    public void OnRetakeButton()
    {
        SceneManager.LoadScene(0);
    }

    public void OnProceedButton()
    {
        postCapturePanel.SetActive(false);
        chooseFramesPanel.SetActive(true);
        //GetComponent<MergePhotos>().DisplayImages();
    }
}
