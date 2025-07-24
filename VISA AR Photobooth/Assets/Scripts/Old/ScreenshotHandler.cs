using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using LitJson;
using System.Linq;
public class ScreenshotHandler : MonoBehaviour
{
    private int resWidth = 1080; //500
    private int resHeight = 1920; //750
    Texture2D screenShot;
    [SerializeField] MeshRenderer videoMeshRenderer;
    [SerializeField] GameObject screenShotPanel, videoPanel, shutter, previewPanel;
    private bool isRealTime;
    [SerializeField] Toggle realtime;

    public Material transMat;

    public Camera scrCamera;
    //other references and fields
    public GameObject networkErrorText;
    public Text imageUrlText;
    public GameObject qrQuad;
    public GameObject finishPanel;

    public Image[] individualPlaceHolders;
    public Image[] facePlaceHolders;
    public Image testImage;
    void LateUpdate()
    {        
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    TakeSCR();
        //}
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ApplicationQuit();
        }
    }

    public void TakeSCR()
    {
        StartCoroutine(TakeSCR_Coroutine());
    }

    //public IEnumerator TakeSCR_Coroutine()
    //{
    //    RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
    //    //scrCamera.targetTexture = rt;
    //    //screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false); 
    //    //scrCamera.Render();
    //    RenderTexture.active = rt;
    //    //screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
    //    //screenShot.Apply();

    //    string directoryPath = Application.dataPath + "/../Photos";
    //    if (!Directory.Exists(directoryPath))
    //    {
    //        Debug.LogError("Photo folder not found.");
    //        yield break;
    //    }

    //    // Get the latest file
    //    string latestFile = Directory.GetFiles(directoryPath, "*.jpg")
    //                                 .OrderByDescending(File.GetCreationTime)
    //                                 .FirstOrDefault();

    //    if (string.IsNullOrEmpty(latestFile))
    //    {
    //        Debug.LogError("No photo found to process.");
    //        yield break;
    //    }

    //    byte[] bytes = File.ReadAllBytes(latestFile);
    //    screenShot = new Texture2D(2, 2); // size will be updated when loading
    //    screenShot.LoadImage(bytes);


    //    shutter.transform.DOScale(.1f, .5f).OnComplete(()=> {
    //        shutter.transform.DOScale(1f, .5f).OnComplete(()=> {                 
    //            StartCoroutine(nameof(ShowPreview));
    //        });
    //    });

    //    scrCamera.targetTexture = null;
    //    RenderTexture.active = null; // JC: added to avoid errors
    //    Destroy(rt);
    //    byte[] bytes = screenShot.EncodeToPNG();


    //    string directoryPath = (Application.dataPath + "/../Photos");
    //    if (!Directory.Exists(directoryPath))
    //    {
    //        //if it doesn't, create it
    //        Directory.CreateDirectory(directoryPath);

    //    }

    //    //save locally
    //    string imageName = "IMG-" + System.DateTime.Now.ToString("HH-mm-ss-dd-MM-yyyy") + ".png";
    //    //Debug.Log(imageName);
    //    System.IO.File.WriteAllBytes(directoryPath + "/"+imageName, bytes);
    //    //send it to the server
    //    WWWForm form = new WWWForm();
    //    form.AddBinaryData("image", bytes, imageName, "image/png");

    //    UnityWebRequest request = UnityWebRequest.Post("http://127.0.0.1:6969/removebg", form);
    //    /*request.SetRequestHeader("key", "riP8pOt2Kq");
    //    request.SetRequestHeader("Accept", "application/json");*/
    //    yield return request.SendWebRequest();
    //    if (!request.isNetworkError)
    //    {
    //        //Debug.Log(request.downloadHandler.text);            
    //        Debug.Log(request.downloadHandler.data);
    //        Texture2D texture2D = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
    //        texture2D.LoadImage(request.downloadHandler.data);
    //        transMat.mainTexture = texture2D;
    //        //GetURL(request.downloadHandler.text);

    //        foreach(Image img in individualPlaceHolders)
    //        {
    //            img.gameObject.SetActive(true);
    //            img.sprite = Sprite.Create(texture2D, new Rect(0, 0, resWidth, resHeight), Vector2.zero);
    //        }

    //        UnityWebRequest request2 = UnityWebRequest.Get("http://127.0.0.1:6969/getface");
    //        yield return request2.SendWebRequest();
    //        if (!request2.isNetworkError)
    //        {
    //            Texture2D texture2D2 = new Texture2D(2, 2, TextureFormat.RGB24, false);
    //            texture2D2.LoadImage(request2.downloadHandler.data);
    //            int width = texture2D2.width;
    //            int height = texture2D2.height;
    //            foreach (Image img in facePlaceHolders)
    //            {
    //                img.gameObject.SetActive(true);
    //                img.sprite = Sprite.Create(texture2D2, new Rect(0, 0, width, height), Vector2.zero);
    //            }
    //        }
    //        else
    //        {
    //            Debug.Log(request2.error);
    //            NetworkError();
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log(request.error);
    //        NetworkError();
    //    }

    //    UIManager.instance.lowerPanel.SetActive(false);
    //    UIManager.instance.postCapturePanel.SetActive(true);
    //    //Debug.Log(string.Format("Took screenshot to: {0}", filename));
    //    if (!isRealTime)
    //    {
    //        //screenShotPanel.SetActive(false);
    //        //videoPanel.SetActive(true);
    //    }

    //}

    //TakeSCR Ends here




    public IEnumerator TakeSCR_Coroutine()
    {
        // Load the latest photo sent by phone into Unity
        string directoryPath = Application.dataPath + "/../Photos";
        if (!Directory.Exists(directoryPath))
        {
            Debug.LogError("Photos directory not found: " + directoryPath);
            yield break;
        }

        // Find the latest image file
        string latestFile = Directory.GetFiles(directoryPath, "*.jpg")
                                     .Concat(Directory.GetFiles(directoryPath, "*.png"))
                                     .OrderByDescending(File.GetCreationTime)
                                     .FirstOrDefault();

        if (string.IsNullOrEmpty(latestFile))
        {
            Debug.LogError("No photo found to process.");
            yield break;
        }

        // Load image as Texture2D
        byte[] bytes = File.ReadAllBytes(latestFile);
        screenShot = new Texture2D(2, 2); // Auto-resize
        screenShot.LoadImage(bytes);

        // Flash effect (optional)
        shutter.transform.DOScale(.1f, .5f).OnComplete(() =>
        {
            shutter.transform.DOScale(1f, .5f).OnComplete(() =>
            {
                StartCoroutine(nameof(ShowPreview));
            });
        });

        // Prepare upload
        WWWForm form = new WWWForm();
        string fileName = Path.GetFileName(latestFile);
        form.AddBinaryData("image", bytes, fileName, "image/png");

        UnityWebRequest request = UnityWebRequest.Post("http://127.0.0.1:6969/removebg", form);
        yield return request.SendWebRequest();

        if (!request.isNetworkError && !request.isHttpError)
        {
            Debug.Log("Received processed image");
            Texture2D texture2D = new Texture2D(2, 2);
            texture2D.LoadImage(request.downloadHandler.data);
            transMat.mainTexture = texture2D;

            // Update preview images
            foreach (Image img in individualPlaceHolders)
            {
                img.gameObject.SetActive(true);
                img.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
            }

            // Fetch cropped face
            UnityWebRequest request2 = UnityWebRequest.Get("http://127.0.0.1:6969/getface");
            yield return request2.SendWebRequest();
            if (!request2.isNetworkError && !request2.isHttpError)
            {
                Texture2D texture2D2 = new Texture2D(2, 2);
                texture2D2.LoadImage(request2.downloadHandler.data);
                foreach (Image img in facePlaceHolders)
                {
                    img.gameObject.SetActive(true);
                    img.sprite = Sprite.Create(texture2D2, new Rect(0, 0, texture2D2.width, texture2D2.height), Vector2.zero);
                }
            }
            else
            {
                Debug.Log(request2.error);
                NetworkError();
            }
        }
        else
        {
            Debug.Log(request.error);
            NetworkError();
        }

        UIManager.instance.lowerPanel.SetActive(false);
        UIManager.instance.postCapturePanel.SetActive(true);
    }



    public void ScaleUpButton(Button button)
    {
        button.transform.DOScale(1.5f, 0.2f).SetEase(Ease.OutExpo);
    }
    public void ScaleDownButton(Button button)
    {
        button.transform.DOScale(1, 0.2f).SetEase(Ease.InExpo);
    }

    IEnumerator ShowPreview()
    {
        previewPanel.transform.DOScale(1, 0);
        previewPanel.GetComponent<Image>().sprite = Sprite.Create(screenShot,new Rect(0,0,screenShot.width, screenShot.height),Vector2.zero);
        previewPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        previewPanel.SetActive(false);
        //previewPanel.transform.DOScale(0, .5f).OnComplete(()=> {
        //    previewPanel.SetActive(false);
        //});
    }

    public void UpdateSettings()
    {        
        if (realtime.isOn)
        {
            videoMeshRenderer.enabled = false;
            screenShotPanel.SetActive(true);
            isRealTime = true;
            //PlayerPrefs.SetInt("isRealTime", 1);
        }
        else if (!realtime.isOn)
        {
            //PlayerPrefs.SetInt("isRealTime", 0);
            screenShotPanel.SetActive(false);
            videoMeshRenderer.enabled = true;
            isRealTime = false;
        }
        //RestartApplication();
    }
    public void ApplicationQuit()
    {
        Application.Quit();
    }

    public void RestartApplication()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

    void NetworkError()
    {
        //networkErrorText.SetActive(true);
    }

    void GetURL(string jsonStr)
    {
        var jsonObj = JsonMapper.ToObject(jsonStr);
        //extract the url here
        string url = jsonObj["link"].ToString();
        imageUrlText.text = url;
        CallForQRCodeGeneration(url);
    }

    void CallForQRCodeGeneration(string url)
    {
        qrQuad.GetComponent<EasyQRCode>().textToEncode = url;
        //call to generate QRCode here
        qrQuad.SetActive(true);
        finishPanel.SetActive(true);
    }

    public void OnFinishButton()
    {
        SceneManager.LoadScene(0);
    }
}