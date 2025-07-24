using UnityEngine;

public class VideoHandler : MonoBehaviour
{
    private WebCamTexture webCamTexture;
    private WebCamDevice[] camDevice;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] Material chromaKeyShader;
    private void Start()
    {
        //Camera.main.aspect = 1.7f;
        Application.targetFrameRate = 30;
        try
        {
            SetupCamera();
        }
        catch (System.Exception ex)
        {
            Debug.Log("no camera found");
        }
        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
    }
    void SetupCamera() { 
        camDevice = WebCamTexture.devices;
        for(int i = 0; i<camDevice.Length; i++)
        {
            Debug.Log(camDevice[i].name);
        }
        webCamTexture = new WebCamTexture(camDevice[0].name); //"XSplit VCam"
        webCamTexture.requestedWidth = 1920;
        webCamTexture.requestedHeight = 1080;
        webCamTexture.requestedFPS = 30;
        webCamTexture.Play();    
    }
    private void Update()
    {
        if (camDevice.Length > 0)
        {
            Graphics.Blit(webCamTexture, renderTexture);            
        }        
    }
    private void OnDestroy()
    {
        webCamTexture.Stop();
    }
}
