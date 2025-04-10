using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{
    // Game Buttons
    public Button dealBtn;
    public Button hitBtn;
    public Button standBtn;
    public Button betBtn;
    public Button betBtn2;
    public Button betBtn3;
    public MusicManager musicManager;
    
    private int standClicks = 0;
    public int quizMultiplier = 1; // commence à 1
    public bool quizReady = true; // indique si un quiz peut être lancé maintenant
    public bool gameOver = false; // indique que la manche est finie
    public Difficulty currentDifficulty = Difficulty.Moyen; // par défaut




    // Access the player and dealer's script
    public PlayerScript playerScript;
    public PlayerScript dealerScript;
    public QuizManager quizManager;
    public bool quizBonus = false;

    // Acces the AddMoney and AddExp script
    public AddMoney addMoneyScript;
    public AddExp addExpScript;

    // public Text to access and update - hud
    public Text scoreText;
    public Text dealerScoreText;
    public Text betsText;
    public Text cashText;
    public Text mainText;
    public Text standBtnText;
    public Button quitBtn;

    // Card hiding dealer's 2nd card
    public GameObject hideCard;
    // How much is bet
    public int pot = 0;

    public bool quizUsed = false;


    void Start()
    {
        dealBtn.onClick.RemoveAllListeners();
        dealBtn.onClick.AddListener(() => DealClicked());

        // Add on click listeners to the buttons
        dealBtn.onClick.AddListener(() => DealClicked());
        hitBtn.onClick.AddListener(() => HitClicked());
        standBtn.onClick.AddListener(() => StandClicked());
        betBtn.onClick.AddListener(() => BetClicked());
        betBtn2.onClick.AddListener(() => BetClicked2());
        betBtn3.onClick.AddListener(() => BetClicked3());
        hitBtn.gameObject.SetActive(false);
        standBtn.gameObject.SetActive(false);
        standBtn.GetComponentInChildren<Text>().text = "RESTER";
        hitBtn.GetComponentInChildren<Text>().text = "TIRER";
        dealBtn.GetComponentInChildren<Text>().text = "JOUER";
        scoreText.text = "";
        dealerScoreText.text = "";
        betsText.text = "";
        quizMultiplier = 1;
        mainText.gameObject.SetActive(false);
        dealerScoreText.gameObject.SetActive(false);
        quitBtn.gameObject.SetActive(false);


    }
    public int GetPot()
    {
        return pot;
    }


    private void DealClicked()
    {
        musicManager.PlayButtonClickSound();
        dealBtn.gameObject.SetActive(false);
        quitBtn.gameObject.SetActive(false);
        if (gameOver)
        {
            gameOver = false;
        }

        // Reset round, hide text, prep for new hand
        playerScript.ResetHand();
        dealerScript.ResetHand();
        // Hide deal hand score at start of deal
        dealerScoreText.gameObject.SetActive(false);
        mainText.gameObject.SetActive(false);
        dealerScoreText.gameObject.SetActive(false);

        GameObject.Find("Deck").GetComponent<DeckScript>().Shuffle();
        playerScript.StartHand();
        dealerScript.StartHand();
        scoreText.text = "Main: " + playerScript.handValue.ToString();
        dealerScoreText.text = "Main: " + dealerScript.handValue.ToString();
        betsText.text = "Somme: $" + (pot * quizMultiplier).ToString();


        // Place card back on dealer card, hide card
        hideCard.GetComponent<Renderer>().enabled = true;
        // Adjust buttons visibility
        hitBtn.gameObject.SetActive(true);
        standBtn.gameObject.SetActive(true);
        // On affiche "QUIZ" mais uniquement si la partie n'est pas terminée
        if (!gameOver)
        {
            dealBtn.GetComponentInChildren<Text>().text = "QUIZ";
            dealBtn.onClick.RemoveAllListeners();
            hitBtn.gameObject.SetActive(false);
            standBtn.gameObject.SetActive(false);
            dealBtn.onClick.AddListener(() => quizManager.OpenQuiz());
        }
        else
        {
            dealBtn.GetComponentInChildren<Text>().text = "REJOUER";
            dealBtn.onClick.RemoveAllListeners();
            dealBtn.onClick.AddListener(() => DealClicked());
        }


        standBtnText.text = "RESTER";
        // Set standard pot size
        betsText.text = "Somme: $" + (pot * quizMultiplier).ToString();
        playerScript.AdjustMoney(-20);
        cashText.text = "$" + playerScript.GetMoney().ToString();

        quizMultiplier = 1; // on recommence à 1 à chaque tour
        quizUsed = false;
        quizReady = true;
        if (!gameOver)
        {
            quizManager.OpenQuiz();
        }





    }

    private void HitClicked()
    {
        musicManager.PlayButtonClickSound();
        if (gameOver) return; // ne rien faire si la partie est finie

        // Check that there is still room on the table
        if (playerScript.cardIndex <= 10)
        {
            playerScript.GetCard();
            scoreText.text = "Main: " + playerScript.handValue.ToString();
            if (playerScript.handValue > 20) RoundOver();
        }
        if (quizUsed && !gameOver)
        {
            quizManager.OpenQuiz();
            quizUsed = false;
        }


    }

    private void StandClicked()
    {
        musicManager.PlayButtonClickSound();
        if (gameOver) return; // ne rien faire si la partie est finie

        standClicks++;
        if (standClicks > 1) RoundOver();
        HitDealer();
        if (quizUsed && !gameOver)
        {
            quizManager.OpenQuiz();
            quizUsed = false;
        }


    }

    private void HitDealer()
    {
        while (dealerScript.handValue < 16 && dealerScript.cardIndex < 10)
        {
            dealerScript.GetCard();
            dealerScoreText.text = "Main: " + dealerScript.handValue.ToString();
            if (dealerScript.handValue > 20) RoundOver();
        }
    }
    
    void LoadScene(string sceneName)
    {
        Debug.Log("Chargement de la scène : " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
    
    // Check for winnner and loser, hand is over
    void RoundOver()
    {
        // Booleans (true/false) for bust and blackjack/21
        bool playerBust = playerScript.handValue > 21;
        bool dealerBust = dealerScript.handValue > 21;
        bool player21 = playerScript.handValue == 21;
        bool dealer21 = dealerScript.handValue == 21;
        // If stand has been clicked less than twice, no 21s or busts, quit function
        if (standClicks < 2 && !playerBust && !dealerBust && !player21 && !dealer21) return;
        bool roundOver = true;
        // All bust, bets returned
        if (playerBust && dealerBust)
        {
            mainText.text = "Egalite : Sommes retrouvees";
            playerScript.AdjustMoney(pot / 2);
        }
        // if player busts, dealer didnt, or if dealer has more points, dealer wins
        else if (playerBust || (!dealerBust && dealerScript.handValue > playerScript.handValue))
        {
            LoadScene("defaite");
            addMoneyScript.AddMoneyToAccount(Connexion.user.username, -pot);
        }
        
        // if player busts, dealer didnt, or if dealer has more points, dealer wins

        // if dealer busts, player didnt, or player has more points, player wins
        else if (dealerBust || playerScript.handValue > dealerScript.handValue)
        {
            LoadScene("Victoire");
            playerScript.AdjustMoney(pot * quizMultiplier);
            addMoneyScript.AddMoneyToAccount(Connexion.user.username, pot * quizMultiplier);
            addExpScript.AddExpToAccount(Connexion.user.username, 50);
            musicManager.PlayVictorySound();
        }

        //Check for tie, return bets
        else if (playerScript.handValue == dealerScript.handValue)
        {
            LoadScene("egalite");
            playerScript.AdjustMoney(pot / 2);
        }
        else
        {
            roundOver = false;
        }
        // Set ui up for next move / hand / turn
        if (roundOver)
        {
            hitBtn.gameObject.SetActive(false);
            standBtn.gameObject.SetActive(false);
            dealBtn.gameObject.SetActive(true);
            Text btnText = dealBtn.GetComponentInChildren<Text>();


            // Remettre le listener original
            dealBtn.onClick.RemoveAllListeners();
            dealBtn.onClick.AddListener(() => DealClicked());

            mainText.gameObject.SetActive(true);
            dealerScoreText.gameObject.SetActive(true);
            hideCard.GetComponent<Renderer>().enabled = false;
            cashText.text = "$" + playerScript.GetMoney().ToString();
            standClicks = 0;
            gameOver = true;
            dealBtn.GetComponentInChildren<Text>().text = "REJOUER";
            dealBtn.onClick.RemoveAllListeners();
            dealBtn.onClick.AddListener(() => DealClicked());
            quitBtn.gameObject.SetActive(true);
            dealBtn.gameObject.SetActive(true);


        }

    }

    // Add money to pot if bet clicked
    void BetClicked()
    {
        Text newBet = betBtn.GetComponentInChildren(typeof(Text)) as Text;
        int intBet = int.Parse(newBet.text.ToString().Remove(0, 1));
        playerScript.AdjustMoney(-intBet);
        cashText.text = "$" + playerScript.GetMoney().ToString();
        pot += (intBet);
        betsText.text = "Somme: $" + (pot * quizMultiplier).ToString();
    }
    void BetClicked2()
    {
        Text newBet = betBtn2.GetComponentInChildren(typeof(Text)) as Text;
        int intBet = int.Parse(newBet.text.ToString().Remove(0, 1));
        playerScript.AdjustMoney(-intBet);
        cashText.text = "$" + playerScript.GetMoney().ToString();
        pot += (intBet);
        betsText.text = "Somme: $" + (pot * quizMultiplier).ToString();
    }
    void BetClicked3()
    {
        Text newBet = betBtn3.GetComponentInChildren(typeof(Text)) as Text;
        int intBet = int.Parse(newBet.text.ToString().Remove(0, 1));
        playerScript.AdjustMoney(-intBet);
        cashText.text = "$" + playerScript.GetMoney().ToString();
        pot += (intBet);
        betsText.text = "Somme: $" + (pot * quizMultiplier).ToString();
    }
    public void ResetState()
    {
        // Réinitialisation basique des textes et scores
        mainText.gameObject.SetActive(false);
        scoreText.text = "";
        dealerScoreText.text = "";
        betsText.text = "";

        // Cache les cartes visibles si tu veux aussi
        foreach (GameObject card in playerScript.hand)
        {
            card.GetComponent<Renderer>().enabled = false;
        }

        foreach (GameObject card in dealerScript.hand)
        {
            card.GetComponent<Renderer>().enabled = false;
        }

        hideCard.GetComponent<Renderer>().enabled = false;
    }
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
