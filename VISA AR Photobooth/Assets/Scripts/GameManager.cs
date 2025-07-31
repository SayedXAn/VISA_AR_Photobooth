using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public Image Logo;

    void Start()
    {
        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
        Logo.gameObject.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
        Logo.gameObject.transform.DOScale(Vector3.one, 3f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }
}
