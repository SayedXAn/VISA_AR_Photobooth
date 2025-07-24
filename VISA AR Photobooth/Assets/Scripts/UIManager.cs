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

    public GameObject nameHandlePlaceholder;
    public Text nameHandleText;
    public InputField nameIF;
    public float placeLen = 0.5f;
    void Start()
    {
        instance = this;
    }

    private void Update()
    {
        nameHandleText.text = nameIF.text;
        if(nameHandlePlaceholder.transform.localScale.x <= 4f || nameHandleText.text.Length < 20)
        {
            nameHandlePlaceholder.transform.DOScaleX(placeLen * nameHandleText.text.Length, .1f);
        }
        
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
