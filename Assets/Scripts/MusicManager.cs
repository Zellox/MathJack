using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;

    // Référence à l'AudioClip du bouton
    public AudioClip buttonClickSound;
    public AudioClip buttonVictorySound;// Assurez-vous que ceci est assigné dans l'inspecteur

    void Start()
    {
        // Récupérer le composant AudioSource
        audioSource = GetComponent<AudioSource>();

        // Vérification si le clip n'est pas assigné dans l'inspecteur
        if (buttonClickSound == null)
        {
            Debug.LogError("Le clip audio du bouton n'est pas assigné !");
        }
    }

    public void PlayButtonClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound); // Joue le son du bouton
        }
        else
        {
            Debug.Log("AudioSource ou AudioClip manquant !");
        }
    }
    public void PlayVictorySound()
    {
        if (audioSource != null && buttonVictorySound != null)
        {
            audioSource.PlayOneShot(buttonVictorySound); // Joue le son du bouton
        }
        else
        {
            Debug.Log("AudioSource ou AudioClip manquant !");
        }
    }
    void Awake()
    {
        // Garde un seul MusicManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre scènes
        }
        else
        {
            Destroy(gameObject); // Évite les doublons
        }
    }

    // Vous pouvez ajouter d'autres fonctionnalités ici si nécessaire.
}