using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using LitJson;

public class LanHandler : MonoBehaviour
{
    public GameObject[] iploadingpanel;
    public Material material;
    public Image[] images;
    public int timeInterval;
    void Start() => StartCoroutine(GetLANIP());
    IEnumerator GetLANIP()
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get("http://robiip-env.eba-hgnwir5s.ap-southeast-1.elasticbeanstalk.com/getip");
        unityWebRequest.timeout = 60;
        yield return unityWebRequest.SendWebRequest();
        if (!unityWebRequest.isNetworkError)
        {
            var jsonObj = JsonMapper.ToObject(unityWebRequest.downloadHandler.text);
            string ip = jsonObj["data"][0]["IP"].ToString();
            if (ip != null)
            {
                StartListening(ip);
                for(int k = 0; k < iploadingpanel.Length; k++) iploadingpanel[k].SetActive(false);
                Debug.Log("listening to IP: " + ip);
            }
        }
        else
        {
            StartCoroutine(GetLANIP());                 
        }
    }

    public void StartListening(string ipno)
    {
        StartCoroutine(nameof(SatrtListeningIP),ipno);
    }
    IEnumerator SatrtListeningIP(string ipno)
    {

        UnityWebRequest request = UnityWebRequest.Get("http://"+ipno+":6969/gettrans"); //:6969
        yield return request.SendWebRequest();
        if (!request.isNetworkError)
        {
            Texture2D texture2D = new Texture2D(1080, 1920, TextureFormat.RGB24, false);
            texture2D.LoadImage(request.downloadHandler.data);            
            Debug.Log(ipno + ":6969/gettrans");
            material.mainTexture = texture2D;            
            for(int i=0; i<images.Length; i++)
            {
                images[i].gameObject.SetActive(false);
                images[i].gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.Log("can't connect to the IP address!!");
        }

        yield return new WaitForSeconds(timeInterval);
        StartCoroutine(nameof(SatrtListeningIP), ipno);
    }

}
