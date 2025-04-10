using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.SceneManagement;

[System.Serializable]
public class User
{
    public int id;
    public string email;
    public string pass;
    public string username;
    public int pieces;
    public int xp;
}