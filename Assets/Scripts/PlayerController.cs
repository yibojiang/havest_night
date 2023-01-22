using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;
using TMPro;

public class PlayerDeviceBindingSource : DeviceBindingSource
{
    protected int deviceId;
    public PlayerDeviceBindingSource(int inDeviceId, InputControlType control) : base(control)
    {
        deviceId = inDeviceId;
    }

    public override float GetValue(InputDevice inputDevice)
    {
        if (deviceId < InputManager.Devices.Count)
        {
            return base.GetValue(InputManager.Devices[deviceId]);
        }

        return base.GetValue(inputDevice);
    }

    public override bool GetState(InputDevice inputDevice)
    {
        if (deviceId < InputManager.Devices.Count)
        {
            return base.GetState(InputManager.Devices[deviceId]);
        }

        return base.GetState(inputDevice);
    }
}
public class PlayerController : Controller
{
    public int playerId;

    protected CharacterActions characterActions;

    public int score;

    public TextMeshPro textComponent;
    public TextMeshPro scoreTextComponent;

    public float respawnTimer = 0.0f;
    public float respawnDuration = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        characterActions = new CharacterActions();

        if (playerId == 0)
        {
            characterActions.Up.AddDefaultBinding(Key.W);
            characterActions.Down.AddDefaultBinding(Key.S);
            characterActions.Sprint.AddDefaultBinding(Key.D);
            characterActions.Jump.AddDefaultBinding(Key.F);
            characterActions.Attack.AddDefaultBinding(Key.G);
        }
        else if (playerId == 1)
        {
            characterActions.Up.AddDefaultBinding(Key.UpArrow);
            characterActions.Down.AddDefaultBinding(Key.DownArrow);
            characterActions.Sprint.AddDefaultBinding(Key.RightArrow);
            characterActions.Jump.AddDefaultBinding(Key.Period);
            characterActions.Attack.AddDefaultBinding(Key.Slash);
        }
        else
        {
            var deviceId = playerId - 2;
            characterActions.Up.AddBinding(new PlayerDeviceBindingSource(deviceId, InputControlType.DPadUp));
            characterActions.Down.AddBinding(new PlayerDeviceBindingSource(deviceId, InputControlType.DPadDown));
            characterActions.Sprint.AddBinding(new PlayerDeviceBindingSource(deviceId, InputControlType.DPadRight));
            characterActions.Jump.AddBinding(new PlayerDeviceBindingSource(deviceId, InputControlType.Action1));
            characterActions.Attack.AddBinding(new PlayerDeviceBindingSource(deviceId, InputControlType.Action2));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (textComponent)
        {
            textComponent.text = $"{playerId + 1}P";
        }

        if (scoreTextComponent)
        {
            scoreTextComponent.text = $"{score}";
        }
        
        if (status == PlayerStatus.Dead)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer < 0.0f)
            {
                SpawnPlayer(playerId % LaneManager.instance.Lanes.Length);
                status = PlayerStatus.Alive;
                respawnTimer = respawnDuration;
                character.Invincible();
            }
            
            if (scoreTextComponent)
            {
                scoreTextComponent.text = $"{respawnTimer.ToString("F1")}";
            }
        }
        
        if (status == PlayerStatus.Unborn)
        {
            // Hit any key to spawn the player character
            if (characterActions.Up.WasPressed || characterActions.Down.WasPressed ||
                characterActions.Sprint.WasPressed || characterActions.Jump.WasPressed ||
                characterActions.Attack.WasPressed)
            {
                // Set the player lane base on the player id
                SpawnPlayer(playerId % LaneManager.instance.Lanes.Length);
                status = PlayerStatus.Alive;
            }
        }
        else if (status == PlayerStatus.Alive)
        {
            if (character.alive == false)
            {
                respawnTimer = respawnDuration;
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
    
    protected override void SpawnPlayer(int inLaneId)
    {
        if (GameStateManager.instance.currentGameState == GameState.PreGame)
        {
            GameStateManager.instance.GameStart();
        }
        var laneId = inLaneId % LaneManager.instance.Lanes.Length;
        Vector3 lanePosition = LaneManager.instance.Lanes[laneId].collider.transform.position;
        Vector3 cameraPosition = Camera.main.transform.position;
        GameObject newPlayer = Instantiate(characterPrefab, new Vector3(cameraPosition.x + 1f, lanePosition.y + 0.5f, lanePosition.z), Quaternion.identity);
        character = newPlayer.GetComponent<Character>();
        character.currentLane = laneId;
        character.GetComponent<SpriteRenderer>().color = playerColor;
        
        GameObject playerText = Instantiate(characterTextPrefab, Vector3.zero, Quaternion.identity);
        playerText.transform.SetParent(newPlayer.transform);
        playerText.transform.localPosition = new Vector3(0, 2.5f, -2.0f);
        playerText.transform.localScale = new Vector3(1, 1, 1);
        
        textComponent = playerText.GetComponent<TextMeshPro>();
        textComponent.text = $"{inLaneId + 1}P";
        textComponent.fontSize = 5;
        textComponent.color = playerColor;
        
        GameObject scoreText = Instantiate(characterTextPrefab, Vector3.zero, Quaternion.identity);
        scoreText.transform.SetParent(newPlayer.transform);
        scoreText.transform.localPosition = new Vector3(0, 2, -2.0f);
        scoreText.transform.localScale = new Vector3(1, 1, 1);
        
        scoreTextComponent = scoreText.GetComponent<TextMeshPro>();
        scoreTextComponent.text = $"{score}";
        scoreTextComponent.fontSize = 5;
        scoreTextComponent.color = Color.white;

        character.playerController = this;
    }

    public override void GetScore(int inScore)
    {
        score += inScore;
    }
}
