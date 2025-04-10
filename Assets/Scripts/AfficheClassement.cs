using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfficheClassement : MonoBehaviour
{
    public GetClassement classement;
    // Start is called before the first frame update
    void Start()
    {
        UpdateClassementText();
    }

    // Update is called once per frame
    void UpdateClassementText()
    {
        classement.GetClassementData();
    }
}
