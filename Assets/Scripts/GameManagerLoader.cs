using UnityEngine;

public class GameManagerLoader : MonoBehaviour
{
    public GameObject gameManagerPrefab; // Assignez le prefab dans l'inspecteur

    void Awake()
    {
        // Vérifie si un GameManager existe déjà dans la scène
        if (FindObjectOfType<GameManager>() == null)
        {
            // Instancie le GameManager à partir du prefab
            Instantiate(gameManagerPrefab);
        }
    }
}