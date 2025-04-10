using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AddExp : MonoBehaviour
{
    public void AddExpToAccount(string pseudo, int amount)
    {
        StartCoroutine(GetRequest("http://localhost/SaeS4/AddExp.php?user=" + pseudo + "&exp=" + amount));
    }
    
    IEnumerator GetRequest(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            print(www.downloadHandler.text);
        }
    }
}
