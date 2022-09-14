using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Left side player
    [SerializeField] private int p1Score;
    // Right side player
    [SerializeField] private int p2Score;
    public int scoreToWin = 7;
    public float startBallDelay = 1;

    private bool P1AIEnabled;
    private bool P2AIEnabled;
    public ControlGoalie P1GoaliePlayerControl;
    public ControlGoalie P2GoaliePlayerControl;
    public AIControlGoalie P1GoalieAIControl;
    public AIControlGoalie P2GoalieAIControl;

    [Header("InGameUI")]
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private TextMeshProUGUI p1ScoreTextIG;
    [SerializeField] private TextMeshProUGUI p2ScoreTextIG;
    [SerializeField] private TextMeshProUGUI winConText;

    [Header("End Of Game UI")]
    [SerializeField] private GameObject gameWonUI;
    [SerializeField] private TextMeshProUGUI p1ScoreTextEOG;
    [SerializeField] private TextMeshProUGUI p2ScoreTextEOG;
    [SerializeField] private TextMeshProUGUI winnerAnnouncementText;

    public BallController ball;
    public MenuHelper menuHelper;

    private void Start()
    {
        StartCoroutine(StartBall());
    }

    // Called when Player 1 has scored
    public void Player1Scored()
    {
        p1Score++;
        p1ScoreTextIG.text = p1Score.ToString();
        if (!CheckIfGameWon())
            ResetBall();
    }

    // Called when Player 2 has scored
    public void Player2Scored()
    {
        p2Score++;
        p2ScoreTextIG.text = p2Score.ToString();
        if (!CheckIfGameWon())
            ResetBall();
    }

    public void PauseBall()
    {
        ball.Pause();
    }

    public void UnpauseBall()
    {
        ball.Unpause();
    }

    public void ResetBall()
    {
        ball.ResetBall();
        ball.Pause();
        StartCoroutine(StartBall());
    }

    public void RestartGame()
    {
        menuHelper.RestartGame();
    }

    private IEnumerator StartBall()
    {
        yield return new WaitForSeconds(startBallDelay);

        ball.Unpause();
        ball.Jumpstart();
    }

    // Checks if either player has scored enough to end the game
    private bool CheckIfGameWon()
    {
        if (p1Score >= scoreToWin)
        {
            ConcludeGame("Player One");
            return true;
        }
        else if (p2Score >= scoreToWin)
        {
            ConcludeGame("Player Two");
            return true;
        }
        return false;
    }

    // Ends the game and displays the end of game UI
    private void ConcludeGame(string winner)
    {
        Destroy(ball.gameObject);
        inGameUI.SetActive(false);
        gameWonUI.SetActive(true);
        p1ScoreTextEOG.text = p1Score.ToString();
        p2ScoreTextEOG.text = p2Score.ToString();
        winnerAnnouncementText.text = winner + " has Won the Game!";
    }

    private void Update()
    {
        winConText.text = "First to " + scoreToWin.ToString() + " wins!";
    }

    public void ToggleP1AI()
    {
        if (!P1AIEnabled)
        {
            P1GoalieAIControl.enabled = true;
            P1GoaliePlayerControl.enabled = false;
            P1AIEnabled = true;
        }
        else
        {
            P1GoalieAIControl.enabled = false;
            P1GoaliePlayerControl.enabled = true;
            P1AIEnabled = false;
        }
    }

    public void ToggleP2AI()
    {
        if (!P2AIEnabled)
        {
            P2GoalieAIControl.enabled = true;
            P2GoaliePlayerControl.enabled = false;
            P2AIEnabled = true;
        }
        else
        {
            P2GoalieAIControl.enabled = false;
            P2GoaliePlayerControl.enabled = true;
            P2AIEnabled = false;
        }
    }
}
