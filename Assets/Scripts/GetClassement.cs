using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class ClassementEntry
{
    public string username;
    public int pieces;
    public int rang;
}

[System.Serializable]
public class Classement
{
    public List<ClassementEntry> entries;
}

public class DisplayClassement : MonoBehaviour
{
    public void ParseAndDisplayClassement(string json)
    {
        ClassementEntry[] classement = JsonHelper.FromJson<ClassementEntry>(json);

        foreach (var entry in classement)
        {
            Debug.Log($"Username: {entry.username}, Pièces: {entry.pieces}, Rang: {entry.rang}");
        }
    }
}


public class GetClassement : MonoBehaviour
{
    private const string url = "http://localhost/saes4/GetClassement.php"; 
    public TMP_Text classementText; 
    
    public void GetClassementData()
    {
        StartCoroutine(FetchClassement(url));
    }

    public IEnumerator FetchClassement(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError("Erreur lors de la récupération du classement : " + www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            ClassementEntry[] classement = JsonHelper.FromJson<ClassementEntry>(json);

            foreach (var entry in classement)
            {
                classementText.text += $"Username: {entry.username}, Pièces: {entry.pieces}, Rang: {entry.rang}\n";
                Debug.Log(classementText.text);
            }
        }
    }
}