using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreditEcranVictoire : MonoBehaviour
{ 
    public TMP_Text creditText;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager introuvable dans la scène !");
        }
        UpdateCreditText();
    }

    // Update is called once per frame
    void UpdateCreditText()
    {
        int poti = gameManager.pot*gameManager.quizMultiplier;
        string pot = poti.ToString();
        creditText.text = "+" + pot + " Crédits";
    }
    
    
}
