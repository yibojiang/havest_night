using System;
using UnityEngine;
using System.Collections.Generic;
using Cinemachine;
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
        
        scrollingCamera.cameraSpeed = 0.0f;
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
            textWin.text = $"Player {playerControllers[highestScorePlayerIndex].playerId} wins !\n'Space' to Restart";
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
                textDistance.text = $"Remain {distance.ToString("F0")}m";
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

