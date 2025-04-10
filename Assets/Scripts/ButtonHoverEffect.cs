using UnityEngine;
using UnityEngine.UI;

public class ButtonHoverEffect : MonoBehaviour
{
    private Vector3 originalScale;
    public Color hoverColor = Color.yellow;
    public Color normalColor = Color.white;
    private Image buttonImage;

    void Start()
    {
        // Stocke la taille initiale et l'image du bouton
        originalScale = transform.localScale;
        buttonImage = GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = normalColor;
        }
    }

    public void OnHoverEnter()
    {
        // Agrandit le bouton et change sa couleur
        transform.localScale = originalScale * 1.1f;
        if (buttonImage != null)
        {
            buttonImage.color = hoverColor;
        }
    }

    public void OnHoverExit()
    {
        // Rétablit la taille et la couleur d'origine
        transform.localScale = originalScale;
        if (buttonImage != null)
        {
            buttonImage.color = normalColor;
        }
    }
}