using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public GameObject quizPanel;
    public Text questionText;
    public InputField answerInput;
    public Button validateButton;
    public GameManager gameManager;
    public DifficultyManager difficultyManager;
    
    private int correctAnswer;

    void Start()
    {
        quizPanel.SetActive(false);
        validateButton.onClick.AddListener(CheckAnswer);
    }

    public void OpenQuiz()
    {
        answerInput.text = "";
        GenerateQuestion();
        quizPanel.SetActive(true);
    }


    void GenerateQuestion()
    {
        if (difficultyManager == null)
        {
            Debug.LogError("DifficultyManager n'est pas assigné dans QuizManager.");
            return;
        }
        
        int a, b;
        string op;
        correctAnswer = 0;

        // Vérifiez la difficulté actuelle
        bool isEasy = difficultyManager.currentDifficulty == DifficultyManager.Difficulty.Easy;

        int operation = isEasy ? Random.Range(0, 2) : Random.Range(2, 4); // 0 = +, 1 = -, 2 = *, 3 = /

        switch (operation)
        {
            case 0: // Addition
                a = Random.Range(1, 11);
                b = Random.Range(1, 11);
                op = "+";
                correctAnswer = a + b;
                break;

            case 1: // Soustraction
                a = Random.Range(1, 11);
                b = Random.Range(1, a + 1); // pour éviter résultat négatif
                op = "-";
                correctAnswer = a - b;
                break;

            case 2: // Multiplication
                a = Random.Range(1, 11);
                b = Random.Range(1, 11);
                op = "×";
                correctAnswer = a * b;
                break;

            case 3: // Division (entière, sans reste)
                b = Random.Range(1, 11);
                correctAnswer = Random.Range(1, 11);
                a = b * correctAnswer;
                op = "÷";
                break;

            default:
                a = 1; b = 1; op = "+"; correctAnswer = 2;
                break;
        }

        questionText.text = $"Combien font {a} {op} {b} ?";
        Debug.Log($"[Quiz] Question : {a} {op} {b} = {correctAnswer}");
    }



    void CheckAnswer()
    {
        if (int.TryParse(answerInput.text, out int playerAnswer))
        {
            if (playerAnswer == correctAnswer)
            {
                gameManager.hideCard.GetComponent<Renderer>().enabled = false;
                gameManager.dealerScoreText.gameObject.SetActive(true);
                gameManager.quizUsed = true;

                questionText.text = "WIN";
            }
            else
            {
                gameManager.quizUsed = true;

                questionText.text = "LOSE";
            }
        }
        validateButton.onClick.RemoveAllListeners();
        validateButton.onClick.AddListener(() => QuitPanel());
        
    }

    void QuitPanel()
    {
        quizPanel.SetActive(false);
        validateButton.onClick.RemoveAllListeners();
        Start();
    }
}
