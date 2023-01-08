using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    protected Character character;

    public int playerId;

    protected CharacterActions characterActions;

    public GameObject playerPrefab;

    public PlayerStatus status = PlayerStatus.Unborn;

    public Color playerColor = Color.white;

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
        else if (playerId == 1)
        {
            characterActions.Up.AddDefaultBinding( Key.UpArrow );
            characterActions.Up.AddDefaultBinding( InputControlType.DPadUp );

            characterActions.Down.AddDefaultBinding( Key.DownArrow );
            characterActions.Down.AddDefaultBinding( InputControlType.DPadDown );
            
            characterActions.Sprint.AddDefaultBinding( Key.Comma );
            characterActions.Sprint.AddDefaultBinding( InputControlType.Action1 );

            characterActions.Jump.AddDefaultBinding( Key.Period );
            characterActions.Jump.AddDefaultBinding( InputControlType.Action2 );
            
            characterActions.Attack.AddDefaultBinding( Key.Slash );
            characterActions.Attack.AddDefaultBinding( InputControlType.Action3 );
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (status == PlayerStatus.Unborn)
        {
            // Hit any key to spawn the player character
            if (characterActions.Up.WasPressed || characterActions.Down.WasPressed ||
                characterActions.Sprint.WasPressed || characterActions.Jump.WasPressed ||
                characterActions.Attack.WasPressed)
            {
                SpawnPlayer();
                status = PlayerStatus.Alive;
            }
        }
        else if (status == PlayerStatus.Alive)
        {
            if (character.alive == false)
            {
                status = PlayerStatus.Dead;
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

    void SpawnPlayer()
    {
        // set the player lane base on the player id
        var laneId = playerId;
        Vector3 lanePosition = LaneManager.instance.Lanes[laneId].collider.transform.position;
        Vector3 cameraPosition = Camera.main.transform.position;
        GameObject newPlayer = Instantiate(playerPrefab, new Vector3(cameraPosition.x + 1f, lanePosition.y + 0.5f, lanePosition.z), Quaternion.identity);
        character = newPlayer.GetComponent<Character>();
        character.currentLane = laneId;
        character.GetComponent<SpriteRenderer>().color = playerColor;
    }
}
