using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.SceneManagement;
using IEnumerator = System.Collections.IEnumerator;


public class Connexion : MonoBehaviour
{
    public TMP_InputField pseudoInput;
    public TMP_InputField passwordInput;
    public static string pseudo;
    public static User user;

    public void Connection()
    {
        StartCoroutine(GetRequest("http://localhost/SaeS4/GetUsers.php?id=" + pseudoInput.text));
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
            string json = www.downloadHandler.text;
            Debug.Log(json);
            User[] users = JsonHelper.FromJson<User>(json);

            if (users.Length > 0)
            {
                user = users[0];
                string hashedPassword = HashPassword(passwordInput.text);

                if (user.username == pseudoInput.text && user.pass == hashedPassword)
                {
                    Debug.Log("Connexion réussie !");
                    pseudo = user.username;
                    Debug.Log(pseudo);
                    // Ajoutez ici les actions à effectuer après la connexion (ex. charger une nouvelle scène)
                    SceneManager.LoadScene("menuPrincipal");
                }
                else
                {
                    Debug.Log(user.username + user.pass);
                    Debug.LogError("Pseudo ou mot de passe incorrect.");
                }
            }
            else
            {
                Debug.LogError("Utilisateur non trouvé.");
            }
        }
    }

    private string HashPassword(string password)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}