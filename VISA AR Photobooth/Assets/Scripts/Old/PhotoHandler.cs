using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoHandler : MonoBehaviour
{
    [SerializeField] Sprite[] photos;
    [SerializeField] Material photoMat;
    [SerializeField] GameObject[] buttonBorder;
    public void ChangePhoto(int i)
    {        
        photoMat.mainTexture = photos[i].texture;
        ActiveBorder(i);
    }

    public void ActiveBorder(int buttonNumber)
    {        
        for (int i =0; i<buttonBorder.Length; i++)
        {
            buttonBorder[i].SetActive(false);
        }
        buttonBorder[buttonNumber].SetActive(true);
    }

    public void DisableAllBorder()
    {
        for (int i = 0; i < buttonBorder.Length; i++)
        {
            buttonBorder[i].SetActive(false);
        }
    }
}
