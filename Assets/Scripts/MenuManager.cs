using Photon.Chat.UtilityScripts;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;


public class MenuManager : MonoBehaviourPunCallbacks
{
    public InputField mdpInput;
    public Text feedbackText;

    public MusicManager musicManager;
    public GameObject menuPanel;
    public GameObject gameCanvas;
    public GameObject gameManager;
    public GameObject playerGroup;
    public GameObject dealerGroup;
    public GameObject hideCard;
    public GameObject buttonCreer;
    public GameObject buttonRejoindre;

    public Button soloBtn;
    public Button multiBtn;
    public Button unv1Btn;

    public Button easyBtn;
    public Button mediumBtn;
    public Button hardBtn;

    public Button quitterBtn;
    public GameObject mdpInputField;
    public GameObject MdpOkBtn;

    private enum GameMode { Solo, Multi, Unv1 }
    private GameMode selectedMode;

    void Start()
    {
        // Affiche menu, cache tout le reste
        menuPanel.SetActive(true);
        gameCanvas.SetActive(false);
        gameManager.SetActive(false);
        playerGroup.SetActive(false);
        dealerGroup.SetActive(false);
        hideCard.SetActive(false);
        buttonCreer.SetActive(false);
        buttonRejoindre.SetActive(false);
        MdpOkBtn.SetActive(false);
        mdpInputField.SetActive(false);
        quitterBtn.gameObject.SetActive(true);

        // Lien des boutons de mode
        soloBtn.onClick.AddListener(() => { musicManager.PlayButtonClickSound(); AskDifficulty(GameMode.Solo); });
        multiBtn.onClick.AddListener(() => { musicManager.PlayButtonClickSound(); AskDifficulty(GameMode.Multi); });
        unv1Btn.onClick.AddListener(() => { musicManager.PlayButtonClickSound(); AskDifficulty(GameMode.Unv1); });

        // Lien des boutons de difficulté
        easyBtn.onClick.AddListener(() => { musicManager.PlayButtonClickSound(); SelectDifficulty(Difficulty.Facile); });
        mediumBtn.onClick.AddListener(() => { musicManager.PlayButtonClickSound(); SelectDifficulty(Difficulty.Moyen); });
        hardBtn.onClick.AddListener(() => { musicManager.PlayButtonClickSound(); SelectDifficulty(Difficulty.Difficile); });

        // Lien bouton quitter
        quitterBtn.onClick.AddListener(() => { musicManager.PlayButtonClickSound(); QuitToMenu(); });

        buttonCreer.GetComponent<Button>().onClick.AddListener(() => { musicManager.PlayButtonClickSound(); OnClickMdp(); });
        buttonRejoindre.GetComponent<Button>().onClick.AddListener(() => { musicManager.PlayButtonClickSound(); OnClickMdp(); });

        // Cache les boutons de difficulté au départ
        easyBtn.gameObject.SetActive(false);
        mediumBtn.gameObject.SetActive(false);
        hardBtn.gameObject.SetActive(false);
    }

    void AskDifficulty(GameMode mode)
    {
        selectedMode = mode;

        soloBtn.gameObject.SetActive(false);
        multiBtn.gameObject.SetActive(false);
        unv1Btn.gameObject.SetActive(false);

        if (mode == GameMode.Multi || mode == GameMode.Unv1)
        {
            buttonCreer.SetActive(true);
            buttonRejoindre.SetActive(true);
        }
        else
        {
            easyBtn.gameObject.SetActive(true);
            mediumBtn.gameObject.SetActive(true);
            hardBtn.gameObject.SetActive(true);
        }
    }

    void SelectDifficulty(Difficulty difficulty)
    {
        gameManager.GetComponent<GameManager>().currentDifficulty = difficulty;

        // Cache le menu
        menuPanel.SetActive(false);

        // Affiche le jeu
        gameCanvas.SetActive(true);
        gameManager.SetActive(true);
        playerGroup.SetActive(true);
        dealerGroup.SetActive(true);
        hideCard.SetActive(true);
    }

    public void QuitToMenu()
    {
        Debug.Log("QUITTER cliqué !");
        musicManager.PlayButtonClickSound();

        gameCanvas.SetActive(false);
        gameManager.SetActive(false);
        playerGroup.SetActive(false);
        dealerGroup.SetActive(false);
        hideCard.SetActive(false);

        // Affiche les boutons de mode
        soloBtn.gameObject.SetActive(true);
        multiBtn.gameObject.SetActive(true);
        unv1Btn.gameObject.SetActive(true);

        // Cache les boutons de difficulté
        easyBtn.gameObject.SetActive(false);
        mediumBtn.gameObject.SetActive(false);
        hardBtn.gameObject.SetActive(false);

        // Réaffiche le menu
        menuPanel.SetActive(true);
    }

    public void OnClickMdp()
    {
        musicManager.PlayButtonClickSound();
        mdpInputField.SetActive(true);
        buttonCreer.SetActive(false);
        buttonRejoindre.SetActive(false);
        MdpOkBtn.SetActive(true);
    }

    public void OnClickValiderMotDePasse()
    {
        musicManager.PlayButtonClickSound();

        string pwd = mdpInput.text;

        if (string.IsNullOrEmpty(pwd))
        {
            feedbackText.text = "Entrez un mot de passe.";
            return;
        }

        // Crée ou rejoint la room directement
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        options.CustomRoomProperties = new Hashtable() { { "pwd", pwd } };
        options.CustomRoomPropertiesForLobby = new string[] { "pwd" };

        PhotonNetwork.JoinOrCreateRoom(pwd, options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Rejoint la room !");

        // Cache tous les éléments du menu
        menuPanel.SetActive(false);
        mdpInputField.SetActive(false);
        MdpOkBtn.SetActive(false);
        buttonCreer.SetActive(false);
        buttonRejoindre.SetActive(false);

        // Affiche le jeu
        gameCanvas.SetActive(true);
        gameManager.SetActive(true);
        playerGroup.SetActive(true);
        dealerGroup.SetActive(true);
        hideCard.SetActive(true);
        Vector3 spawnPos = new Vector3(PhotonNetwork.CurrentRoom.PlayerCount * 3, 4.4f, 0);
        PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        feedbackText.text = "Échec de la connexion : " + message;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        feedbackText.text = "Échec de la création : " + message;
    }
}
