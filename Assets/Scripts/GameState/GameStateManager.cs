using System;
using UnityEngine;
using System.Collections.Generic;
using MultiplayerWithBindingsExample;
using TMPro;

class GameStateManager: SingletonBehaviour<GameStateManager>
{
    public GameState currentGameState = GameState.PreGame;

    public float distance = 0;
    public float totalDistance = 500;

    public TextMeshPro textDistance;
    
    public TextMeshPro textWin;

    public PlayerController[] playerControllers;
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
    }

    public void GameOver()
    {
        if (textWin)
        {
            textWin.gameObject.SetActive(true);    
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
            textWin.text = $"Player {highestScorePlayerIndex} wins !";
        }

        Time.timeScale = 0.0f;
    }

    public void Update()
    {
        if (currentGameState == GameState.InGame)
        {
            distance -= Time.deltaTime;

            if (distance < 0.0f)
            {
                distance = 0.0f;
                GameOver();
            }

            if (textDistance)
            {
                textDistance.text = $"Remain {distance.ToString("F0")}m";
            }
        }
    }
}

