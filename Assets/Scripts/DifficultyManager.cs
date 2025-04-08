using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DifficultyManager : MonoBehaviour
{
    public enum Difficulty { Easy, Normal, Difficult }
    public Difficulty currentDifficulty = Difficulty.Easy;

    public void SetDifficulty(string difficulty)
    {
        if (difficulty == "Easy")
        {
            currentDifficulty = Difficulty.Easy;
        }
        else if (difficulty == "Normal")
        {
            currentDifficulty = Difficulty.Normal;
        }
        else if(difficulty == "Difficult")
        {
            currentDifficulty = Difficulty.Difficult;
        }
    }
}
