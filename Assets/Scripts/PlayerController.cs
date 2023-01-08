using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Character character;

    protected CharacterActions characterActions;

    // Start is called before the first frame update
    void Start()
    {
        characterActions = new CharacterActions();
        
        characterActions.Up.AddDefaultBinding( Key.UpArrow );
        characterActions.Up.AddDefaultBinding( InputControlType.DPadUp );

        characterActions.Down.AddDefaultBinding( Key.DownArrow );
        characterActions.Down.AddDefaultBinding( InputControlType.DPadDown );

        characterActions.Jump.AddDefaultBinding( Key.Space );
        characterActions.Jump.AddDefaultBinding( InputControlType.Action1 );
        
        characterActions.Jump.AddDefaultBinding( Key.Space );
        characterActions.Jump.AddDefaultBinding( InputControlType.Action2 );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (characterActions.Jump.IsPressed)
        {
            character.Jump();
        }
    }
}
