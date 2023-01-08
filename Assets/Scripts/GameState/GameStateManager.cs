using UnityEngine;
using System.Collections.Generic;

class GameStateManager: MonoBehaviour
{
    public int PlayerNumber = 4;

    [SerializeField] private GameObject mPlayerPrefab;
    [SerializeField] private GameObject[] lanes;
    [SerializeField] private GameObject ScrollingCamera;
    private GameState mCurrentGameState = GameState.PreGame;
    private List<PlayerStatus> mPlayerStatus;
    

    public void Start()
    {
        mCurrentGameState = GameState.PreGame;
        mPlayerStatus = new List<PlayerStatus>();
        for (int i = 0; i < PlayerNumber; i++)
        {
            mPlayerStatus.Add(PlayerStatus.Unborn);
        }
    }

    public void Update()
    {
        switch (mCurrentGameState)
        {
            case GameState.PreGame:
                if (Input.GetKeyDown(KeyCode.W)
                    || Input.GetKeyDown(KeyCode.A)
                    || Input.GetKeyDown(KeyCode.S)
                    || Input.GetKeyDown(KeyCode.D))
                {
                    if(mPlayerStatus[0] != PlayerStatus.Unborn)
                    {
                        break;
                    }

                    GameObject newPlayer = Instantiate(mPlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    Vector3 lanePosition= lanes[0].transform.position;
                    Vector3 cameraPosition = ScrollingCamera.transform.position;
                    newPlayer.GetComponent<Character>().currentLane = 0;
                    newPlayer.transform.position = new Vector3(cameraPosition.x + 1f, lanePosition.y + 0.5f, lanePosition.z);
                    mPlayerStatus[0] = PlayerStatus.Alive;
                }
                break;
        }
    }
}

