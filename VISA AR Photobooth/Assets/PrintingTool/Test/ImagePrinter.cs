using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ImagePrinter : MonoBehaviour 
{
    private string _filePath;

    //public string filePath = "";

    public PrintingTool printingTool;

    private void Awake()
    {
        _filePath = Application.persistentDataPath + "/output.png";
    }
    public void PrintImage()
    {
        printingTool.CmdPrintThreaded(_filePath);
        Debug.Log(_filePath);
        printingTool.StartCheckIsPrintingDone();
    }
}
