using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

public class CharacterActions : PlayerActionSet
{
    public PlayerAction Up;
    public PlayerAction Down;
    public PlayerAction Jump;
    public PlayerAction Sprint;
    public PlayerAction Attack;

    public CharacterActions()
    {
        Up = CreatePlayerAction("Move Up");
        Down = CreatePlayerAction("Move Down");
        Jump = CreatePlayerAction("Jump");
        Sprint = CreatePlayerAction("Sprint");
        Attack = CreatePlayerAction("Attack");
    }
}
