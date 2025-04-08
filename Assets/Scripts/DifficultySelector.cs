using UnityEngine;

public class DifficultySelector : MonoBehaviour
{
    public DifficultyManager difficultyManager;

    public void SelectEasy()
    {
        difficultyManager.SetDifficulty("Easy");
    }

    public void SelectNormal()
    {
        difficultyManager.SetDifficulty("Normal");
    }
}