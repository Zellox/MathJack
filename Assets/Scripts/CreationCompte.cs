using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class CreationCompte : MonoBehaviour
{
    public TMP_InputField pseudoInput;
    public TMP_InputField passwordInput;
    public TMP_InputField emailInput;
    public void CreateCompte()
    {
        StartCoroutine(GetRequest("http://localhost/SaeS4/AddUser.php?email="+ emailInput.text + "&pass=" + passwordInput.text + "&user=" + pseudoInput.text));
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
    
    // public void ReturnConnection()
    // {
    //     if (emailInput.text == null)
    //     {
    //         
    //     }
    // }
}
