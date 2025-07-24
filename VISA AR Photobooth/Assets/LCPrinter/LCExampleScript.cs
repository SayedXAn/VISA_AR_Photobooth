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

    public void printSmileButton()
    {
        texture2D = GetComponent<MergePhotos>().bytes;

        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(texture2D); // Load image from byte array

        // Step 2: Create a new texture with the same size as the original texture
        int width = texture.width;
        int height = texture.height;
        Texture2D newTexture = new Texture2D(width, height);

        // Step 3: Set all pixels of the new texture to white (background color)
        Color[] whitePixels = new Color[width * height];
        for (int i = 0; i < whitePixels.Length; i++)
        {
            whitePixels[i] = Color.white;
        }
        newTexture.SetPixels(whitePixels);

        // Step 4: Resize the original image to fit into the top-left 1/4 of the new texture
        int quarterWidth = (int)(width / 2.5f);
        int quarterHeight = (int)(height / 2.5f);

        // Resize the original texture to fit in the 1/4th space (resize operation)
        Texture2D resizedTexture = new Texture2D(quarterWidth, quarterHeight);
        for (int y = 0; y < quarterHeight; y++)
        {
            for (int x = 0; x < quarterWidth; x++)
            {
                // Sample the original texture, scaled down to fit into the quarter space
                float u = (float)x / (quarterWidth);
                float v = (float)y / (quarterHeight);
                resizedTexture.SetPixel(x, y, texture.GetPixelBilinear(u, v));
            }
        }

        // Apply the resized texture
        resizedTexture.Apply();

        // Step 5: Place the resized texture into the new texture (top-left corner)
        for (int y = 0; y < quarterHeight; y++)
        {
            for (int x = 0; x < quarterWidth; x++)
            {
                // Copy the resized pixel to the top-left corner of the new texture
                newTexture.SetPixel(x+25, height - quarterHeight + y - 285, resizedTexture.GetPixel(x, y));
            }
        }

        // Step 6: Apply the changes to the new texture
        newTexture.Apply();

        // Step 7: Convert the texture back to a byte array (PNG format)
        byte[] modifiedTextureBytes = newTexture.EncodeToPNG();

        //string filePath = Application.persistentDataPath;
        //System.IO.File.WriteAllBytes(filePath + "/" + "cropped.png", modifiedTextureBytes);
        //Debug.Log(filePath);

        Print.PrintTexture(modifiedTextureBytes, copies, printerName);
    }

    public void printByPathButton()
    {
        Print.PrintTextureByPath(inputField.text.Trim(), copies, printerName);
    }
}