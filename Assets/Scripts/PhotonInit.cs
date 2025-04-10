using UnityEngine;
using Photon.Pun;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("🔄 Connexion à Photon...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("✅ Connecté à Photon avec succès !");
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        Debug.LogError("❌ Déconnecté de Photon : " + cause.ToString());
    }
}
