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
    [SerializeField] GameObject screenShotPanel, shutter;
    private bool isRealTime;
    //[SerializeField] Toggle realtime;

    public Material transMat;

    public Camera scrCamera;
    //other references and fields
    //public GameObject networkErrorText;
    public Text imageUrlText;
    public GameObject qrQuad;
    public GameObject finishPanel;
    

    public Image[] individualPlaceHolders;
    //public Image[] facePlaceHolders;
    public Image testImage;

    public string latestFile = "";
    public Image prevImg;

    
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

    


    public void CheckIfNewPhoto()
    {
        string directoryPath = Application.dataPath + "/../Photos";
        if (!Directory.Exists(directoryPath))
        {
            Debug.LogError("Photos directory not found: " + directoryPath);
            return;
        }

        // Find the latest image file
        latestFile = Directory.GetFiles(directoryPath, "*.jpg")
                              .Concat(Directory.GetFiles(directoryPath, "*.png"))
                              .OrderByDescending(File.GetCreationTime)
                              .FirstOrDefault();

        if (string.IsNullOrEmpty(latestFile))
        {
            Debug.LogError("No photo found to process.");
            return;
        }
        else
        {
            byte[] bytes = File.ReadAllBytes(latestFile);

            // Load texture from bytes
            screenShot = new Texture2D(2, 2); // Let it auto-resize
            screenShot.LoadImage(bytes);

            // Create a sprite from the texture
            Sprite sprite = Sprite.Create(screenShot, new Rect(0, 0, screenShot.width, screenShot.height), new Vector2(0.5f, 0.5f));
            prevImg.sprite = sprite;
            prevImg.type = Image.Type.Simple;
            prevImg.preserveAspect = true;
            shutter.SetActive(true);

        }
    }

    public void TakeSCR_Coroutine()
    {
        byte[] bytes = File.ReadAllBytes(latestFile);
        screenShot = new Texture2D(2, 2); // Auto-resize
        screenShot.LoadImage(bytes);

        foreach (Image img in individualPlaceHolders)
        {
            img.gameObject.SetActive(true);
            img.sprite = Sprite.Create(screenShot, new Rect(0, 0, screenShot.width, screenShot.height), Vector2.zero);
            img.preserveAspect = true;
            img.type = Image.Type.Simple;
        }

        UIManager.instance.lowerPanel.SetActive(false);
        UIManager.instance.postCapturePanel.SetActive(true);
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