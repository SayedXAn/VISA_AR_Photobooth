using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.IO;

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
    int selectedFrameIndex;

    //for screenshots
    private int resWidth = 1080; //500
    private int resHeight = 1920; //750
    private Texture2D screenShot;
    Camera scrCamera;
    public byte[] bytes;
    private const string API_KEY = "2ea2ced6be1c4867a94df3a6db09124e";

    public GameObject[] imageSlots;

    List<byte[]> allImagesBytes = new List<byte[]>();


    //private int ssResWidth = 1080;
    //private int ssResHeight = 1920;
    //private Texture2D frameScreenshots;
    //public Camera framesScrCamera;


    void Start()
    {
        //totalSlots = imageSlots.Length;
        scrCamera = finalScreenShotCamera.GetComponent<Camera>();
        //DisplayImages();
    }

    //public void DisplayImages()
    //{
    //    for(int i=0; i < totalSlots; i++)
    //    {
    //        imageSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = backgroundImages[i];
    //        imageSlots[i].transform.GetChild(1).gameObject.SetActive(true);/*GetComponent<Image>().sprite = transparentImage;*/
    //        imageSlots[i].transform.GetChild(1).GetComponent<Image>().material = transMat;
    //        imageSlots[i].transform.GetChild(2).GetComponent<Image>().sprite = foregroundImages[i];
    //    }
    //}

    public void ProceedButtonPressed()
    {
        StartCoroutine(TakeScreenShot());
    }

    public void OnFrameSelected(int index)
    {
        UIManager.instance.defaultBG.SetActive(true);
        selectedFrameIndex = index;
        UIManager.instance.chooseFramesPanel.SetActive(false);
    }

    //IEnumerator TakeScreenShot()
    //{
    //    GameObject selectedFrame = Instantiate(imageSlots[selectedFrameIndex], finalScreenshotCanvas.transform);

    //    RectTransform rectTransform = selectedFrame.GetComponent<RectTransform>();
    //    rectTransform.anchorMin = new Vector2(0, 0);
    //    rectTransform.anchorMax = new Vector2(1, 1);
    //    rectTransform.offsetMin = new Vector2(0, 0);
    //    rectTransform.offsetMax = new Vector2(0, 0);


    //    finalScreenshotCanvas.SetActive(true);
    //    finalScreenShotCamera.SetActive(true);

    //    //UIManager.instance.chooseFramesPanel.SetActive(false);

    //    yield return new WaitForEndOfFrame();

    //    RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
    //    scrCamera.targetTexture = rt;
    //    screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
    //    scrCamera.Render();
    //    RenderTexture.active = rt;
    //    screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
    //    screenShot.Apply();

    //    scrCamera.targetTexture = null;
    //    RenderTexture.active = null; // JC: added to avoid errors
    //    Destroy(rt);
    //    bytes = screenShot.EncodeToPNG();

    //    string directoryPath = (Application.dataPath + "/../Chosen Photos");
    //    if (!Directory.Exists(directoryPath))
    //    {
    //        //if it doesn't, create it
    //        Directory.CreateDirectory(directoryPath);

    //    }

    //    Destroy(selectedFrame);

    //    string allImagesDir = (Application.dataPath + "/../All_Photos");
    //    if (!Directory.Exists(allImagesDir))
    //    {
    //        //if it doesn't, create it
    //        Directory.CreateDirectory(allImagesDir);

    //    }

    //    allImagesBytes.Clear();

    //    foreach (GameObject img in imageSlots)
    //    {
    //        GameObject currFrame = Instantiate(img, finalScreenshotCanvas.transform);

    //        RectTransform _rectTransform = currFrame.GetComponent<RectTransform>();
    //        _rectTransform.anchorMin = new Vector2(0, 0);
    //        _rectTransform.anchorMax = new Vector2(1, 1);
    //        _rectTransform.offsetMin = new Vector2(0, 0);
    //        _rectTransform.offsetMax = new Vector2(0, 0);


    //        finalScreenshotCanvas.SetActive(true);
    //        finalScreenShotCamera.SetActive(true);

    //        //UIManager.instance.chooseFramesPanel.SetActive(false);

    //        yield return new WaitForEndOfFrame();

    //        RenderTexture _rt = new RenderTexture(resWidth, resHeight, 24);
    //        scrCamera.targetTexture = _rt;
    //        screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
    //        scrCamera.Render();
    //        RenderTexture.active = _rt;
    //        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
    //        screenShot.Apply();

    //        scrCamera.targetTexture = null;
    //        RenderTexture.active = null; // JC: added to avoid errors
    //        Destroy(_rt);
    //        byte[] b = screenShot.EncodeToPNG();
    //        allImagesBytes.Add(b);
    //    }

    //    WWWForm form = new WWWForm();
    //    //save locally
    //    string imageName = "IMG_With_Chosen_Frame-" + System.DateTime.Now.ToString("HH-mm-ss-dd-MM-yyyy") + "choosen.png";
    //    System.IO.File.WriteAllBytes(directoryPath + "/" + imageName, bytes);
    //    for (int i = 0; i < allImagesBytes.Count; i++)
    //    {
    //        string _imageName = "IMG_all_four-" + System.DateTime.Now.ToString("HH-mm-ss-dd-MM-yyyy") + $"_{i}.png";
    //        File.WriteAllBytes(allImagesDir + "/" + _imageName, allImagesBytes[i]);
    //        form.AddBinaryData("images", allImagesBytes[i], imageName, "image/png");
    //    }
    //    form.AddField("title", "VISA");
    //    form.AddField("description", "Test description");

    //    //form.AddBinaryData("image", bytes, imageName, "image/png");
    //    Debug.Log("Sending............");

    //    UnityWebRequest request = UnityWebRequest.Post($"https://sing-file-share.wskoly.xyz/api/upload/", form);
    //    request.SetRequestHeader("API-Key", API_KEY);
    //    yield return request.SendWebRequest();

    //    finalScreenshotCanvas.SetActive(false);
    //    finalScreenShotCamera.SetActive(false);

    //    if (!request.isNetworkError && !request.isHttpError)
    //    {
    //        string responseText = request.downloadHandler.text;
    //        Debug.Log(responseText);
    //        SingFileApiResponse response = null;
    //        try
    //        {
    //            response = JsonConvert.DeserializeObject<SingFileApiResponse>(responseText);
    //        }
    //        catch (System.Exception e)
    //        {
    //            Debug.Log(e.Message);
    //        }

    //        if (response != null)
    //        {
    //            string url = response.link;
    //            //imageUrlText.text = url;
    //            QR_DisplayQuad.GetComponent<EasyQRCode>().textToEncode = url;
    //            //call to generate QRCode here
    //            QR_DisplayQuad.SetActive(true);
    //            finalPanel.SetActive(true);
    //            //Texture2D texture2D = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
    //            //texture2D.LoadImage(request.downloadHandler.data);
    //            //transMat.mainTexture = texture2D;
    //            //GetURL(request.downloadHandler.text);
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log(request.error);
    //        Debug.Log(request.downloadHandler.text);
    //        NetworkError();
    //    }
    //    //make the printer workable

    //    UIManager.instance.finalPanel.SetActive(true);
    //}


    IEnumerator TakeScreenShot()
    {
        // Step 1: Capture chosen frame
        GameObject selectedFrame = Instantiate(imageSlots[selectedFrameIndex], finalScreenshotCanvas.transform);

        RectTransform rectTransform = selectedFrame.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        finalScreenshotCanvas.SetActive(true);
        finalScreenShotCamera.SetActive(true);

        yield return new WaitForEndOfFrame();

        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        scrCamera.targetTexture = rt;
        screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        scrCamera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();

        scrCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        bytes = screenShot.EncodeToPNG();

        Destroy(selectedFrame); // ✅ Important to avoid memory build-up

        string directoryPath = Path.Combine(Application.dataPath, "../Chosen Photos");
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

        string allImagesDir = Path.Combine(Application.dataPath, "../All_Photos");
        if (!Directory.Exists(allImagesDir)) Directory.CreateDirectory(allImagesDir);

        allImagesBytes.Clear();

        // Step 2: Capture all frames
        for (int i = 0; i < imageSlots.Length; i++)
        {
            GameObject currFrame = Instantiate(imageSlots[i], finalScreenshotCanvas.transform);

            RectTransform _rectTransform = currFrame.GetComponent<RectTransform>();
            _rectTransform.anchorMin = Vector2.zero;
            _rectTransform.anchorMax = Vector2.one;
            _rectTransform.offsetMin = Vector2.zero;
            _rectTransform.offsetMax = Vector2.zero;

            yield return new WaitForEndOfFrame();

            RenderTexture _rt = new RenderTexture(resWidth, resHeight, 24);
            scrCamera.targetTexture = _rt;
            screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            scrCamera.Render();
            RenderTexture.active = _rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            screenShot.Apply();

            scrCamera.targetTexture = null;
            RenderTexture.active = null;
            Destroy(_rt);

            byte[] b = screenShot.EncodeToPNG();
            allImagesBytes.Add(b);

            Destroy(currFrame); // ✅ Prevents memory leak
            yield return null;  // ✅ Allows Unity to breathe
        }

        // Step 3: Save to disk and prepare form
        string imageName = $"IMG_With_Chosen_Frame-{System.DateTime.Now:HH-mm-ss-dd-MM-yyyy}-choosen.png";
        File.WriteAllBytes(Path.Combine(directoryPath, imageName), bytes);

        WWWForm form = new WWWForm();
        for (int i = 0; i < allImagesBytes.Count; i++)
        {
            string _imageName = $"IMG_all_four-{System.DateTime.Now:HH-mm-ss-dd-MM-yyyy}_{i}.png";
            File.WriteAllBytes(Path.Combine(allImagesDir, _imageName), allImagesBytes[i]);
            form.AddBinaryData("images", allImagesBytes[i], _imageName, "image/png");
        }
        form.AddField("title", "VISA");
        form.AddField("description", "Test description");

        Debug.Log("Sending............");

        UnityWebRequest request = UnityWebRequest.Post("https://sing-file-share.wskoly.xyz/api/upload/", form);
        request.SetRequestHeader("API-Key", API_KEY);

        // Optional: add timeout logic
        float startTime = Time.time;
        float timeout = 10f;

        yield return request.SendWebRequest();

        while (!request.isDone && Time.time - startTime < timeout)
            yield return null;

        finalScreenshotCanvas.SetActive(false);
        finalScreenShotCamera.SetActive(false);

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log(responseText);
            try
            {
                var response = JsonConvert.DeserializeObject<SingFileApiResponse>(responseText);
                if (response != null)
                {
                    string url = response.link;
                    QR_DisplayQuad.GetComponent<EasyQRCode>().textToEncode = url;
                    QR_DisplayQuad.SetActive(true);
                    finalPanel.SetActive(true);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("JSON Parse Error: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("Upload Error: " + request.error);
            NetworkError();
        }

        UIManager.instance.finalPanel.SetActive(true);
    }


    void NetworkError()
    {
        //networkErrorText.SetActive(true);
        Debug.LogError("Network error hoiseeeeeeeeeeee");
    }
}
