using UnityEngine;
using System.Collections;

public class EasyQRCode : MonoBehaviour {

    [HideInInspector] public string textToEncode = "Hello World!";
    public Color darkColor = Color.black;
    public Color lightColor = Color.white;
    [SerializeField] private Material qrMat;


    void Start()
    {
        /*// Example usage of QR Generator
        // The text can be any string, link or other QR Code supported string

        Texture2D qrTexture = QRGenerator.EncodeString(textToEncode, darkColor, lightColor);

        // Set the generated texture as the mainTexture on the quad
        qrMat.mainTexture = qrTexture;
        // GetComponent<Renderer>().material.mainTexture = qrTexture;*/

#if UNITY_EDITOR
        if (!Application.isPlaying) return;
#endif
        GenerateQR();
    }

    public void SetQRText(string text)
    {
        textToEncode = text;
        GenerateQR();
    }
    private void GenerateQR()
    {
        Texture2D qrTexture = QRGenerator.EncodeString(textToEncode, darkColor, lightColor);
        qrMat.mainTexture = qrTexture;
    }
}
