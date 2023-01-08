using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Character character;

    public int playerId;

    protected CharacterActions characterActions;

    protected void Awake()
    {
        character = GetComponent<Character>();
    }

    // Start is called before the first frame update
    void Start()
    {
        characterActions = new CharacterActions();

        if (playerId == 0)
        {
            characterActions.Up.AddDefaultBinding( Key.W );
            characterActions.Up.AddDefaultBinding( InputControlType.DPadUp );

            characterActions.Down.AddDefaultBinding( Key.S );
            characterActions.Down.AddDefaultBinding( InputControlType.DPadDown );
            
            characterActions.Sprint.AddDefaultBinding( Key.G );
            characterActions.Sprint.AddDefaultBinding( InputControlType.Action1 );

            characterActions.Jump.AddDefaultBinding( Key.H );
            characterActions.Jump.AddDefaultBinding( InputControlType.Action2 );
            
            characterActions.Attack.AddDefaultBinding( Key.J );
            characterActions.Attack.AddDefaultBinding( InputControlType.Action3 );
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (character.alive == false)
        {
            return;
        }
        
        if (characterActions.Jump.WasPressed)
        {
            character.Jump();
        }

        if (characterActions.Sprint.WasPressed)
        {
            character.Sprint();
        }
        
        if (characterActions.Up.WasPressed)
        {
            character.ChangeLaneUp();
        }
        
        if (characterActions.Down.WasPressed)
        {
            character.ChangeLaneDown();
        }

        if (characterActions.Attack.WasPressed)
        {
            character.Attack();
        }
    }
}
