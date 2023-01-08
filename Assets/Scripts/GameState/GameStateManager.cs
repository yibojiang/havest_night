using System;
using UnityEngine;
using System.Collections.Generic;
using MultiplayerWithBindingsExample;

class GameStateManager: SingletonBehaviour<GameStateManager>
{
    private GameState currentGameState = GameState.PreGame;
    public void Start()
    {
        currentGameState = GameState.PreGame;
    }
}

