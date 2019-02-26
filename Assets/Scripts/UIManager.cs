using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager> {

    [SerializeField] GameObject m_mainMenu;
    [SerializeField] GameObject m_message;
    [SerializeField] GameObject m_intro;
    [SerializeField] GameObject m_hud;
    [SerializeField] GameObject m_pause;
    [SerializeField] GameObject m_sinkEndMenu;
    [SerializeField] GameObject m_islandEndMenu;
    [SerializeField] TextMeshProUGUI m_duelWinner;
    [SerializeField] TextMeshProUGUI m_goldWinner;
    [SerializeField] TextMeshProUGUI m_islandWinner;
    [SerializeField] TextMeshProUGUI m_playerOneHP;
    [SerializeField] TextMeshProUGUI m_playerTwoHP;
    [SerializeField] TextMeshProUGUI m_playerOneGold;
    [SerializeField] TextMeshProUGUI m_playerTwoGold;

    void Start()
    {
        Time.timeScale = 0.0f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !m_pause.activeInHierarchy)
        {
            m_pause.SetActive(true);
            Time.timeScale = 0;
            Music.Instance.PauseMusic();
        }
    }

    public void Unpause()
    {
        m_pause.SetActive(false);
        Time.timeScale = 1.0f;
        Music.Instance.UnpauseMusic();
    }

    public void MainMenuToMessage()
    {
        m_mainMenu.SetActive(false);
        m_message.SetActive(true);
    }

    public void MessageToIntro()
    {
        m_message.SetActive(false);
        m_intro.SetActive(true);
    }

    public void MessageToMainMenu()
    {
        m_message.SetActive(false);
        m_mainMenu.SetActive(true);
    }

    public void IntroToMessage()
    {
        m_intro.SetActive(false);
        m_message.SetActive(true);
    }

    public void BackToMainMenu()
    {
        Music.Instance.PlayMenuMusic();
        m_hud.SetActive(false);
        m_pause.SetActive(false);
        m_mainMenu.SetActive(true);
    }

    public void SinkEndGameToMainMenu()
    {
        m_sinkEndMenu.SetActive(false);
        m_hud.SetActive(false);
        Game.Instance.ResetGame();
        m_mainMenu.SetActive(true);
        Music.Instance.PlayMenuMusic();
    }

    public void SinkEndGameToIntro()
    {
        m_sinkEndMenu.SetActive(false);
        m_hud.SetActive(false);
        Game.Instance.ResetGame();
        m_intro.SetActive(true);
        Music.Instance.PlayMenuMusic();
    }

    public void IslandEndGameToMainMenu()
    {
        m_islandEndMenu.SetActive(false);
        m_hud.SetActive(false);
        Game.Instance.ResetGame();
        m_mainMenu.SetActive(true);
        Music.Instance.PlayMenuMusic();
    }

    public void IslandEndGameToIntro()
    {
        m_islandEndMenu.SetActive(false);
        m_hud.SetActive(false);
        Game.Instance.ResetGame();
        m_intro.SetActive(true);
        Music.Instance.PlayMenuMusic();
    }

    public void StartGame()
    {
        m_intro.SetActive(false);
        m_hud.SetActive(true);
        Game.Instance.ResetGame();
        Game.Instance.SpawnIsland();
        GoldManager.Instance.ResetGold();
        Time.timeScale = 1.0f;
        Music.Instance.PlayGameMusic();
    }

    public void SinkEndGame(string winner)
    {
        Game.Instance.PutOutFires();
        Music.Instance.PlayEndGameMusic();
        m_duelWinner.text = "Duel Winner: " + winner;

        if (Game.Instance.GetPlayer1().goldCount > Game.Instance.GetPlayer2().goldCount)
        {
            m_goldWinner.text = "Gold Winner: Player One";
        }
        else
        {
            m_goldWinner.text = "Gold Winner: Player Two";
        }

        m_sinkEndMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void IslandEndGame(string winner)
    {
        Game.Instance.PutOutFires();
        Music.Instance.PlayEndGameMusic();
        m_islandWinner.text = winner + " Found the\nDread Pirate's Treasure!";
        m_islandEndMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void SetPlayerOneHP(int hitPoints)
    {
        m_playerOneHP.text = "Hit Points: " + hitPoints;
    }

    public void SetPlayerTwoHP(int hitPoints)
    {
        m_playerTwoHP.text = "Hit Points: " + hitPoints;
    }

    public void SetPlayerOneGold(int gold)
    {
        m_playerOneGold.text = "Gold: " + gold;
    }

    public void SetPlayerTwoGold(int gold)
    {
        m_playerTwoGold.text = "Gold: " + gold;
    }
}
