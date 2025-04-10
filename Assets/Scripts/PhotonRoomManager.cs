using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PhotonRoomManager : MonoBehaviourPunCallbacks
{
    public InputField mdpInput;
    public Button okBtn;

    private bool isJoining = false;

    void Start()
    {
        okBtn.onClick.AddListener(OnClickOk);
    }

    void OnClickOk()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogWarning("Pas encore connecté à Photon !");
            return;
        }

        string roomName = mdpInput.text;

        if (string.IsNullOrEmpty(roomName))
        {
            Debug.Log("Mot de passe vide !");
            return;
        }

        if (isJoining)
        {
            PhotonNetwork.JoinRoom(roomName);
        }
        else
        {
            RoomOptions options = new RoomOptions { MaxPlayers = 2 };
            PhotonNetwork.CreateRoom(roomName, options);
        }
    }

    public void SetIsJoining(bool join)
    {
        isJoining = join;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Rejoint !");
        // Tu peux cacher l'input et afficher les cartes maintenant
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Échec JoinRoom : " + message);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Échec CreateRoom : " + message);
    }
}
