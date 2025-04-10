using UnityEngine;
using UnityEngine.UI;



public class QuizManager : MonoBehaviour
{
    public GameObject quizPanel;
    public Text questionText;
    public InputField answerInput;
    public Button validateButton;
    public GameManager gameManager;
    public Button hitBtn;
    public Button standBtn;

    public Text timerText;

    private float timeLeft;
    private bool timerRunning = false;
    private int correctAnswer;
    private bool aRepondu = false;
    private string resultatMessage = "";

    void Start()
    {
        quizPanel.SetActive(false);
        timerText.gameObject.SetActive(false);
        validateButton.onClick.AddListener(CheckAnswer);
    }

    public void OpenQuiz()
    {
        answerInput.text = "";
        GenerateQuestion();
        quizPanel.SetActive(true);

        timeLeft = 10f;
        timerRunning = true;
        timerText.text = "10";
        timerText.gameObject.SetActive(true);

        // Reset
        aRepondu = false;
        validateButton.GetComponentInChildren<Text>().text = "Valider";
    }

    void Update()
    {
        if (timerRunning)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = Mathf.CeilToInt(timeLeft).ToString();

            if (timeLeft <= 0f)
            {
                timerRunning = false;
                TimeOut();
            }
        }
    }

    void TimeOut()
    {
        gameManager.quizUsed = true;
        gameManager.quizBonus = false;

        Text btnText = gameManager.dealBtn.GetComponentInChildren<Text>();
        btnText.text = "PERDU";

        gameManager.dealBtn.onClick.RemoveAllListeners();
        validateButton.onClick.AddListener(FermerQuiz);
        questionText.GetComponentInChildren<Text>().text = "TEMPS ECOULE";
        answerInput.gameObject.SetActive(false);
        hitBtn.gameObject.SetActive(true);
        standBtn.gameObject.SetActive(true);
    }

    void FermerQuiz()
    {
        timerText.gameObject.SetActive(false);
        quizPanel.SetActive(false);
    }
    
    void CheckAnswer()
    {
        if (!aRepondu)
        {
            timerRunning = false;
            timerText.gameObject.SetActive(false);

            if (int.TryParse(answerInput.text, out int playerAnswer))
            {
                if (playerAnswer == correctAnswer)
                {
                    if (!gameManager.hideCard.GetComponent<Renderer>().enabled)
                    {
                        gameManager.quizMultiplier *= 2;
                        gameManager.betsText.text = "Somme: $" + (gameManager.GetPot() * gameManager.quizMultiplier).ToString();
                        resultatMessage = "Bonne réponse ! Bonus : Gain doublé 💰";
                    }
                    else
                    {
                        gameManager.hideCard.GetComponent<Renderer>().enabled = false;
                        gameManager.dealerScoreText.gameObject.SetActive(true);
                        resultatMessage = "Bonne réponse ! Bonus : Carte du croupier révélée 🃏";
                    }

                    gameManager.quizUsed = true;
                }
                else
                {
                    gameManager.quizUsed = true;
                    resultatMessage = "Mauvaise réponse ❌";
                }
            }
            else
            {
                resultatMessage = "Réponse invalide ❓";
            }

            // Change le texte sans fermer le panel
            questionText.text = resultatMessage;
            validateButton.GetComponentInChildren<Text>().text = "OK";
            aRepondu = true;
        }
        else
        {
            // 2e clic : ferme le panel
            quizPanel.SetActive(false);
            aRepondu = false;
            validateButton.GetComponentInChildren<Text>().text = "Valider";
            hitBtn.gameObject.SetActive(true);
            standBtn.gameObject.SetActive(true);
        }
    }

    void GenerateQuestion()
    {
        int a = 0, b = 0;
        string op = "";
        correctAnswer = 0;

        Difficulty level = gameManager.currentDifficulty;

        if (level == Difficulty.Facile)
        {
            int operation = Random.Range(0, 2); // 0 = +, 1 = -
            a = Random.Range(1, 11);
            b = Random.Range(1, 11);

            if (operation == 0)
            {
                op = "+";
                correctAnswer = a + b;
            }
            else
            {
                op = "-";
                correctAnswer = a - b;
            }

            questionText.text = $"Combien font {a} {op} {b} ?";
        }
        else if (level == Difficulty.Moyen)
        {
            GenerateMediumQuestion();
        }
        else if (level == Difficulty.Difficile)
        {
            int type = Random.Range(0, 2); // 0 = opération, 1 = équation

            if (type == 0)
            {
                GenerateMediumQuestion();
                return;
            }
            else
            {
                int x = Random.Range(1, 11);
                int coeff = Random.Range(1, 5);
                int constant = Random.Range(0, 6);
                correctAnswer = x;

                int opType = Random.Range(0, 3); // 0 = +, 1 = -, 2 = /
                string equationText = "";
                int result = 0;

                switch (opType)
                {
                    case 0:
                        result = coeff * x + constant;
                        equationText = $"{coeff}x + {constant} = {result}";
                        break;
                    case 1:
                        result = coeff * x - constant;
                        equationText = $"{coeff}x - {constant} = {result}";
                        break;
                    case 2:
                        coeff = coeff * x;
                        result = coeff / x + constant;
                        equationText = $"{coeff}/x + {constant} = {result}";
                        break;
                }

                questionText.text = $"Résous : {equationText}";
            }
        }

        Debug.Log($"[Quiz] Réponse correcte : {correctAnswer}");
    }

    void GenerateMediumQuestion()
    {
        int a = 0, b = 0;
        string op = "";
        int operation = Random.Range(0, 4); // 0 = +, 1 = -, 2 = ×, 3 = ÷

        switch (operation)
        {
            case 0:
                a = Random.Range(1, 11);
                b = Random.Range(1, 11);
                op = "+";
                correctAnswer = a + b;
                break;
            case 1:
                a = Random.Range(1, 11);
                b = Random.Range(1, a + 1);
                op = "-";
                correctAnswer = a - b;
                break;
            case 2:
                a = Random.Range(1, 11);
                b = Random.Range(1, 11);
                op = "×";
                correctAnswer = a * b;
                break;
            case 3:
                b = Random.Range(1, 11);
                correctAnswer = Random.Range(1, 11);
                a = b * correctAnswer;
                op = "÷";
                break;
        }

        questionText.text = $"Combien font {a} {op} {b} ?";
    }
}
