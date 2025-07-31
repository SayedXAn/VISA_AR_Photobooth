using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using System.Linq;

public class MergePhotos : MonoBehaviour
{
    public Sprite[] backgroundImages;
    public Sprite[] foregroundImages;
    //public Sprite transparentImage;
    //public GameObject[] imageSlots;
    //int totalSlots;

    public GameObject QR_DisplayQuad;
    public Material transMat;
    public GameObject finalScreenShotCamera;
    public GameObject finalScreenshotCanvas;
    public GameObject finalPanel;
    public GameObject loadingPanel;
    int selectedFrameIndex;
    private int pendingEncodes = 0;

    //for screenshots
    private int resWidth = 1080; //500
    private int resHeight = 1920; //750
    private Texture2D screenShot;
    Camera scrCamera;
    public byte[] bytes;
    private const string API_KEY = "2ea2ced6be1c4867a94df3a6db09124e";

    public GameObject[] imageSlots;

    List<byte[]> allImagesBytes = new List<byte[]>();


    void Start()
    {
        //totalSlots = imageSlots.Length;
        scrCamera = finalScreenShotCamera.GetComponent<Camera>();
        //DisplayImages();
    }

    public void OnFrameSelected(int index)
    {
        UIManager.instance.defaultBG.SetActive(true);
        selectedFrameIndex = index;
        loadingPanel.SetActive(true);
        StartCoroutine(TakeScreenShot());
    }

    IEnumerator TakeScreenShot()
    {

        GameObject selectedFrame = Instantiate(imageSlots[selectedFrameIndex], finalScreenshotCanvas.transform);

        RectTransform rectTransform = selectedFrame.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.offsetMin = new Vector2(0, 0);
        rectTransform.offsetMax = new Vector2(0, 0);


        finalScreenshotCanvas.SetActive(true);
        finalScreenShotCamera.SetActive(true);

        UIManager.instance.chooseFramesPanel.SetActive(false);

        yield return new WaitForEndOfFrame();

        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        scrCamera.targetTexture = rt;
        screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        scrCamera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();

        scrCamera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        bytes = screenShot.EncodeToPNG();

        string directoryPath = (Application.dataPath + "/../Chosen Photos");
        if (!Directory.Exists(directoryPath))
        {
            //if it doesn't, create it
            Directory.CreateDirectory(directoryPath);

        }

        Destroy(selectedFrame);

        string allImagesDir = (Application.dataPath + "/../All_Photos");
        if (!Directory.Exists(allImagesDir))
        {
            //if it doesn't, create it
            Directory.CreateDirectory(allImagesDir);

        }

        allImagesBytes.Clear();

        foreach (GameObject img in imageSlots)
        {
            GameObject currFrame = Instantiate(img, finalScreenshotCanvas.transform);

            RectTransform _rectTransform = currFrame.GetComponent<RectTransform>();
            _rectTransform.anchorMin = new Vector2(0, 0);
            _rectTransform.anchorMax = new Vector2(1, 1);
            _rectTransform.offsetMin = new Vector2(0, 0);
            _rectTransform.offsetMax = new Vector2(0, 0);


            finalScreenshotCanvas.SetActive(true);
            finalScreenShotCamera.SetActive(true);

            UIManager.instance.chooseFramesPanel.SetActive(false);

            yield return new WaitForEndOfFrame();

            RenderTexture _rt = new RenderTexture(resWidth, resHeight, 24);
            scrCamera.targetTexture = _rt;
            screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            scrCamera.Render();
            RenderTexture.active = _rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            screenShot.Apply();

            scrCamera.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(_rt);
            byte[] b = screenShot.EncodeToPNG();
            allImagesBytes.Add(b);
        }

        WWWForm form = new WWWForm();
        //save locally
        string imageName = "IMG_With_Chosen_Frame-" + System.DateTime.Now.ToString("HH-mm-ss-dd-MM-yyyy") + "choosen.png";
        System.IO.File.WriteAllBytes(directoryPath + "/" + imageName, bytes);
        for (int i = 0; i < allImagesBytes.Count; i++)
        {
            string _imageName = "IMG_all_four-" + System.DateTime.Now.ToString("HH-mm-ss-dd-MM-yyyy") + $"_{i}.png";
            File.WriteAllBytes(allImagesDir + "/" + _imageName, allImagesBytes[i]);
            form.AddBinaryData("images", allImagesBytes[i], imageName, "image/png");
        }
        form.AddField("title", "VISA");
        form.AddField("description", "Scroll down to all photos");

        //form.AddBinaryData("image", bytes, imageName, "image/png");
        Debug.Log("Sending............");

        UnityWebRequest request = UnityWebRequest.Post($"https://sing-file-share.wskoly.xyz/api/upload/", form);
        request.SetRequestHeader("API-Key", API_KEY);
        yield return request.SendWebRequest();

        finalScreenshotCanvas.SetActive(false);
        finalScreenShotCamera.SetActive(false);

        if (!request.isNetworkError && !request.isHttpError)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log(responseText);
            SingFileApiResponse response = null;
            try
            {
                response = JsonConvert.DeserializeObject<SingFileApiResponse>(responseText);
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }

            if (response != null)
            {
                string url = response.link;
                //imageUrlText.text = url;
                QR_DisplayQuad.GetComponent<EasyQRCode>().textToEncode = url;
                //call to generate QRCode here
                loadingPanel.SetActive(false);
                QR_DisplayQuad.SetActive(true);
                finalPanel.SetActive(true);
            }
        }
        else
        {
            Debug.Log(request.error);
            Debug.Log(request.downloadHandler.text);
            NetworkError();
        }

        UIManager.instance.finalPanel.SetActive(true);
    }

    void NetworkError()
    {
        //networkErrorText.SetActive(true);
    }
}