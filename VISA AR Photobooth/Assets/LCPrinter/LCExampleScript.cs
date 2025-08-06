using UnityEngine;
using System.Collections;
using System;
using System.IO;
using LCPrinter;
using UnityEngine.UI;

public class LCExampleScript : MonoBehaviour
{

    byte[] texture2D;
    public string printerName = "";
    public int copies = 1;
    public InputField inputField;

    //public void printSmileButton()
    //{
    //    texture2D = GetComponent<MergePhotos>().bytes;

    //    Texture2D originalTexture = new Texture2D(2, 2);
    //    originalTexture.LoadImage(texture2D); // Load original image

    //    int fullWidth = 1200;
    //    int fullHeight = 1800;
    //    int sidePadding = 10;
    //    int topPadding = 40;
    //    int bottomPadding = 10;

    //    // Scale image to 90% to leave more padding
    //    float scale = 0.6f;
    //    int scaledWidth = (int)(fullWidth * scale);
    //    int scaledHeight = (int)(fullHeight * scale);

    //    // Step 1: Resize original image to scaled dimensions
    //    Texture2D resizedTexture = new Texture2D(scaledWidth, scaledHeight);
    //    for (int y = 0; y < scaledHeight; y++)
    //    {
    //        for (int x = 0; x < scaledWidth; x++)
    //        {
    //            float u = (float)x / scaledWidth;
    //            float v = (float)y / scaledHeight;
    //            resizedTexture.SetPixel(x, y, originalTexture.GetPixelBilinear(u, v));
    //        }
    //    }
    //    resizedTexture.Apply();

    //    // Step 2: Create final canvas with white background and padding
    //    int finalWidth = scaledWidth + sidePadding * 2;
    //    int finalHeight = scaledHeight + topPadding + bottomPadding;

    //    Texture2D finalTexture = new Texture2D(finalWidth, finalHeight);
    //    Color[] whitePixels = new Color[finalWidth * finalHeight];
    //    for (int i = 0; i < whitePixels.Length; i++)
    //        whitePixels[i] = Color.white;
    //    finalTexture.SetPixels(whitePixels);

    //    // Step 3: Paste scaled image into the center area
    //    for (int y = 0; y < scaledHeight; y++)
    //    {
    //        for (int x = 0; x < scaledWidth; x++)
    //        {
    //            Color pixel = resizedTexture.GetPixel(x, y);
    //            finalTexture.SetPixel(x + sidePadding, y + bottomPadding, pixel);
    //        }
    //    }
    //    finalTexture.Apply();

    //    // Step 4: Encode and print
    //    byte[] finalImageBytes = finalTexture.EncodeToPNG();
    //    Print.PrintTexture(finalImageBytes, copies, printerName);
    //}

    public void printSmileButton()
    {
        texture2D = GetComponent<MergePhotos>().bytes;

        Texture2D originalTexture = new Texture2D(2, 2);
        originalTexture.LoadImage(texture2D); // Load original image

        int canvasWidth = 1012;   // 4 inches @ 300 DPI
        int canvasHeight = 1800;  // 6 inches @ 300 DPI
        float scale = 0.75f;

        int scaledWidth = (int)(canvasWidth * scale);
        int scaledHeight = (int)(canvasHeight * scale);

        // Step 1: Resize original image to scaled size
        Texture2D resizedTexture = new Texture2D(scaledWidth, scaledHeight);
        for (int y = 0; y < scaledHeight; y++)
        {
            for (int x = 0; x < scaledWidth; x++)
            {
                float u = (float)x / scaledWidth;
                float v = (float)y / scaledHeight;
                resizedTexture.SetPixel(x, y, originalTexture.GetPixelBilinear(u, v));
            }
        }
        resizedTexture.Apply();

        // Step 2: Create full-size canvas with white background
        Texture2D finalTexture = new Texture2D(canvasWidth, canvasHeight);
        Color[] whitePixels = new Color[canvasWidth * canvasHeight];
        for (int i = 0; i < whitePixels.Length; i++)
            whitePixels[i] = Color.white;
        finalTexture.SetPixels(whitePixels);

        // Step 3: Center the image
        int offsetX = (canvasWidth - scaledWidth) / 2;
        int offsetY = (canvasHeight - scaledHeight) / 2 + 10; // +10 for extra top padding

        for (int y = 0; y < scaledHeight; y++)
        {
            for (int x = 0; x < scaledWidth; x++)
            {
                Color pixel = resizedTexture.GetPixel(x, y);
                finalTexture.SetPixel(x + offsetX, y + offsetY, pixel);
            }
        }

        finalTexture.Apply();

        // Step 4: Encode and print
        byte[] finalImageBytes = finalTexture.EncodeToPNG();
        Print.PrintTexture(finalImageBytes, copies, printerName);
    }


    public void printByPathButton()
    {
        Print.PrintTextureByPath(inputField.text.Trim(), copies, printerName);
    }
}