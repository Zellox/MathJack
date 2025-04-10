using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GetUser : MonoBehaviour
{
    public TMP_Text emailText;
    public TMP_Text usernameText;
    public TMP_Text piecesText;
    public TMP_Text nvText;
    public TMP_Text xpText;
    
    void Start()
    {
        if (Connexion.pseudo != null)
        {
            emailText.text = "Email : " + Connexion.user.email;
            usernameText.text = "Pseudo : " + Connexion.user.username;
            piecesText.text = "Pièces : " + Connexion.user.pieces;
            nvText.text = "Niveau : " + Connexion.user.xp/100;
            xpText.text = "XP : " + Connexion.user.xp;
        }
        else
        {
            Debug.LogError("Aucun utilisateur connecté.");
        }
    }

    
}
