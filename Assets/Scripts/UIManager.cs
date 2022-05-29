using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private GameObject player;
    public Text scoreText;
    public Text gameOverText;
    public Text restartGameText;
    public Text pausedText;
    public Button resumeButton;
    public Button QuitButton;

    public bool isGamePaused = false;

    public Sprite[] liveSprites;

    [SerializeField]
    private Image LivesImg;
    void Start()
    {
        player = GameObject.Find("Player");
        scoreText.text = "Score: " + 0;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isGamePaused == false)
        {
            PauseGame();
        }
        RestartGame();
    }

    public void UpdateScore(int playerscore)
    {
        scoreText.text = "Score: " + playerscore;

    }

    public void UpdateLives(int currentLives)
    {
        LivesImg.sprite = liveSprites[currentLives];
    }
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartGameText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlicker());
    }
    public void RestartGame()
    {
        if(restartGameText.gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.R)
)
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0;

        pausedText.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(true);
        QuitButton.gameObject.SetActive(true);


    }
    public void UnpauseGame()
    {
        isGamePaused = false;
        Time.timeScale = 1;

        pausedText.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        QuitButton.gameObject.SetActive(false);
    }
    public void QuitToMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }
    

    private IEnumerator GameOverFlicker()
    {
        while(true)
        {
            gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator WaitOneSecond()
    {
        yield return new WaitForSeconds(1f);
    }
}
