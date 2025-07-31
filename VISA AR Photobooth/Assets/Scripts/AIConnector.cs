using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.Networking;
using UnityEngine.UI;
using DG.Tweening;
using LitJson;
using System.Net;
using System.Net.Sockets;

public class AIConnector : MonoBehaviour
{
    public static AIConnector instance;
    //public GameObject nnpanel,nnpanel2, ailogo, ailogo2;
    //public Text nnmsg, nnmsg2;
    struct ipdata{
        public string ip;
    }
private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            //UnityEngine.Debug.Log("calllled!!!");
            //KillProcess();
            StartAI();
            StartCoroutine(SendIPtoServer());
            //AnimateLogo();
        }
        else
        {
            Destroy(gameObject);
        }        
    }
    
    void StartAI()
    {
        try
        {
            //Process p = new Process();
            //p.StartInfo.UseShellExecute = true;
            ////p.StartInfo.RedirectStandardOutput = true;
            //p.StartInfo.FileName = ".\\AI\\nn.exe";
            //p.Start();
            //nnpanel.SetActive(true);
            //nnmsg2.text = nnmsg.text = "Nerual Network initializing...";
            // string nnPath=  Application.streamingAssetsPath + "/nn_koly/nn_koly.exe";
            // UnityEngine.Debug.Log(nnPath);
            // Process process = Process.Start(nnPath);
            StartCoroutine(ShowNNState());
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError(e.Message);
        }
    }
    IEnumerator ShowNNState()
    {
        // writes...................connecting 
        //nnmsg.text = "Trying to connect with Neural Net...";
        UnityWebRequest webRequest =  UnityWebRequest.Get("http://127.0.0.1:6969/connect");
        yield return webRequest.SendWebRequest();
        if (!webRequest.isNetworkError)
        {
            UnityEngine.Debug.Log(webRequest.downloadHandler.text);            
            var jsonObj = JsonMapper.ToObject(webRequest.downloadHandler.text);            
            string msg = jsonObj["msg"].ToString();
            
        }
        else
        {
            yield return new WaitForSeconds(2);
            StartCoroutine(ShowNNState());
        }
    }
   
    IEnumerator SendIPtoServer()
    {
        ipdata ip = new ipdata();
        ip.ip = GetLocalIP();
        string jsondata = JsonMapper.ToJson(ip);
        UnityWebRequest request = UnityWebRequest.PostWwwForm("http://robiip-env.eba-hgnwir5s.ap-southeast-1.elasticbeanstalk.com/postip", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsondata);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        if (!request.isNetworkError)
        {
            UnityEngine.Debug.Log(request.downloadHandler.text);
            var jsonObj = JsonMapper.ToObject(request.downloadHandler.text);
            string msg = jsonObj["msg"].ToString();
            if (msg == "success")
            {
                UnityEngine.Debug.Log("post!!!");
            }
        }
        else
        {
            StartCoroutine(SendIPtoServer());
        }
    }
    private string GetLocalIP()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }
    private void OnApplicationQuit()
    {
        KillProcess();
    }

    void KillProcess()
    {
        Process[] processes = Process.GetProcessesByName("nn");
        foreach (var process in processes)
        {
            process.Kill();
        }
    }
}
