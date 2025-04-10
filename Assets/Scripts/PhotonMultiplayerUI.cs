using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PhotonMultiplayerUI : MonoBehaviourPunCallbacks
{
    public InputField passwordInput;
    public Text feedbackText;


    public void OnCreateClicked()
    {
        string pwd = passwordInput.text;

        if (string.IsNullOrEmpty(pwd))
        {
            feedbackText.text = "Entrez un mot de passe pour creer une partie.";
            return;
        }

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        options.CustomRoomProperties = new Hashtable() { { "pwd", pwd } };
        options.CustomRoomPropertiesForLobby = new string[] { "pwd" };

        PhotonNetwork.CreateRoom(pwd, options); // Le nom de la Room = mot de passe
    }

    public void OnJoinClicked()
    {
        string pwd = passwordInput.text;

        if (string.IsNullOrEmpty(pwd))
        {
            feedbackText.text = "Entrez le mot de passe pour rejoindre.";
            return;
        }

        PhotonNetwork.JoinRoom(pwd);
    }

    public override void OnJoinedRoom()
    {
        feedbackText.text = "🎉 Connecte a la partie avec le mot de passe.";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        feedbackText.text = "❌ Erreur creation : " + message;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        feedbackText.text = "❌ Aucune partie trouvee avec ce mot de passe.";
    }
}
