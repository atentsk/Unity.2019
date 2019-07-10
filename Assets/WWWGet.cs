using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// UnityWebRequest example
// https://docs.unity3d.com/Manual/UnityWebRequest-HLAPI.html
public class WWWGet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // A correct website page.
        StartCoroutine(GetRequest("https://www.example.com"));

        // A non-existing page.
        StartCoroutine(GetRequest("https://error.html"));

        // Texture File From Http
        StartCoroutine(GetTexture());
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }
    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(
            // 접근 가능한 이미지 파일 링크
            "https://www.google.com/images/branding/googlelogo/2x/googlelogo_color_92x30dp.png"
            );
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            Debug.LogFormat("{0}:\nReceived: {1}", www.url, myTexture.texelSize);
            // 완료후 텍스쳐 적용
            if(GetComponent<Renderer>()!=null)
                GetComponent<Renderer>().material.mainTexture = myTexture;
        }
    }
}
