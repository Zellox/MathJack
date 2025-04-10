using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Navigation : MonoBehaviour
{
    void Start()
    {
        // Parcourt tous les boutons actifs dans la scène
        foreach (Button btn in FindObjectsOfType<Button>())
        {
            string btnName = btn.gameObject.name.ToLower(); // Nom en minuscules pour simplifier

            if (btnName.Contains("menu"))
                btn.onClick.AddListener(() => LoadScene("menuPrincipal"));
            
            else if (btnName.Contains("classement"))
                btn.onClick.AddListener(() => LoadScene("classement"));
            
            else if (btnName.Contains("rejouer"))
                btn.onClick.AddListener(() => LoadScene("GameScene"));

            else if (btnName.Contains("victoire"))
                btn.onClick.AddListener(() => LoadScene("Victoire"));

            else if (btnName.Contains("defaite"))
                btn.onClick.AddListener(() => LoadScene("defaite"));
            
            else if (btnName.Contains("regle"))
                btn.onClick.AddListener(() => LoadScene("regles"));

            else if (btnName.Contains("moncompte"))
                btn.onClick.AddListener(() => LoadScene("monCompte"));

            else if (btnName.Contains("creercompte"))
                btn.onClick.AddListener(() => LoadScene("créerCompte"));

            else if (btnName.Contains("boutique"))
                btn.onClick.AddListener(() => LoadScene("Boutique"));

            else if (btnName.Contains("quitter"))
                btn.onClick.AddListener(QuitterJeu);
            
            else if (btnName.Contains("jouer"))
                btn.onClick.AddListener(() => LoadScene("SampleScene"));
        }
    }

    void LoadScene(string sceneName)
    {
        Debug.Log("Chargement de la scène : " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    void QuitterJeu()
    {
        Debug.Log("Quitter le jeu...");
        Application.Quit();
    }
}
