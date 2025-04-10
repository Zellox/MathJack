using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetUserMenu : MonoBehaviour
{
    public TMP_Text piecesText;
    public TMP_Text nvText;
    
    void Start()
    {
        if (Connexion.pseudo != null)
        {
            piecesText.text = "Pièces : " + Connexion.user.pieces;
            nvText.text = "Niveau : " + Connexion.user.xp/100;
        }
        else
        {
            Debug.LogError("Aucun utilisateur connecté.");
        }
    }
}
