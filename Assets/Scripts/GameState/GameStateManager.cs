using System;
using UnityEngine;
using System.Collections.Generic;
using Cinemachine;
using MultiplayerWithBindingsExample;
using TMPro;
using UnityEngine.UI;

enum PlayerIconType
{
    Normal,
    Sad,
    Hurt
}

class GameStateManager: SingletonBehaviour<GameStateManager>
{
    public GameState currentGameState = GameState.PreGame;

    public float distance = 0;
    public float totalDistance = 500;
    
    public TextMeshProUGUI textDistance;
    public TextMeshProUGUI textWin;

    public TextMeshProUGUI[] textResultPlayerScores;
    public Image[] imgPlayerIcon;
    public GameObject panelResult;
    public Sprite[] playerNormalIcons;
    public Sprite[] playerSadIcons;
    public Sprite[] playerHurtIcons;

    public PlayerController[] playerControllers;

    public ScrollingCamera scrollingCamera;

    public HarvestMachine harvestMachine;

    public float cameraSpeed = 4.5f;

    public GameObject easyGroup;
    public GameObject normalGroup;
    public GameObject hardGroup;
    public GameObject menu;
    
    public void Start()
    {
        currentGameState = GameState.PreGame;
        if (textWin)
        {
            textWin.gameObject.SetActive(false);    
        }
        
        if (textDistance)
        {
            textDistance.gameObject.SetActive(false);
        }
        
        if (panelResult)
        {
            panelResult.SetActive(false);
        }
        
        scrollingCamera.cameraSpeed = 0.0f;
    }

    public void UpdatePlayerScore(int playerId, int score)
    {
        if (playerId < textResultPlayerScores.Length)
        {
            textResultPlayerScores[playerId].text = score.ToString();
        }
    }

    public void UpdatePlayerIcon(int playerId, int score)
    {
        
    }

    public void GameStart()
    {
        currentGameState = GameState.InGame;
        if (textWin)
        {
            textWin.gameObject.SetActive(false);    
        }

        if (textDistance)
        {
            textDistance.gameObject.SetActive(true);
        }

        scrollingCamera.cameraSpeed = cameraSpeed;

        if (harvestMachine)
        {
            harvestMachine.ShowUp();
        }

        if (menu)
        {
            menu.GetComponent<Animator>().SetTrigger("Hide");
        }
    }

    public void GameOver()
    {
        if (textWin)
        {
            textWin.gameObject.SetActive(true);    
        }
        
        if (panelResult)
        {
            panelResult.SetActive(true);
            for (int i = 0; i < playerControllers.Length; i++)
            {
                if (playerControllers[i].status == PlayerStatus.Unborn)
                {
                    textResultPlayerScores[i].transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    textResultPlayerScores[i].transform.parent.gameObject.SetActive(true);
                    textResultPlayerScores[i].text = $"{playerControllers[i].score}";
                }
                
            }
        }
        
        currentGameState = GameState.PostGame;

        int highestScorePlayerIndex = 0;
        for (int i = 0; i < playerControllers.Length; i++)
        {
            if (playerControllers[i].score > playerControllers[highestScorePlayerIndex].score)
            {
                highestScorePlayerIndex = i;
            }
        }

        if (textWin)
        {
            textWin.text = $"Player {playerControllers[highestScorePlayerIndex].playerId + 1} wins !\n'Space' to Restart";
        }

        scrollingCamera.cameraSpeed = 0.0f;
    }

    public void Update()
    {
        if (currentGameState == GameState.InGame)
        {
            distance -= scrollingCamera.cameraSpeed * Time.deltaTime;

            if (distance < 0.0f)
            {
                distance = 0.0f;
                GameOver();
            }

            if (distance < totalDistance * 0.3)
            {
                easyGroup.SetActive(false);
                normalGroup.SetActive(false);
                hardGroup.SetActive(true);
            }
            else if (distance < totalDistance * 0.7)
            {
                easyGroup.SetActive(false);
                normalGroup.SetActive(true);
                hardGroup.SetActive(false);
            }
            else
            {
                easyGroup.SetActive(true);
                normalGroup.SetActive(false);
                hardGroup.SetActive(false);
            }

            if (textDistance)
            {
                textDistance.text = $"{distance.ToString("F0")}m LEFT";
            }
        }

        if (currentGameState == GameState.PostGame)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Time.timeScale = 1.0f;
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }
}

