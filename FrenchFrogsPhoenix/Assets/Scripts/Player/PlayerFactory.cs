using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerType
{
    AI = 0,
    HUMAN = 1
}

public class PlayerFactory : MonoBehaviour {

    public const string EVT_ONLOCALPLAYERDEATH = "OnLocalPlayerDeath";

    [SerializeField] Player playerPrefab;
    [SerializeField] CameraFlightFollow playerCameraPrefab;

    [SerializeField] float radiusSpwan = 50;

    List<Player> playerList;

    public Player LocalPlayer { get; private set; }

	void Start () {
        playerList = new List<Player>();

        LocalPlayer = SpawnPlayer(PlayerType.HUMAN, Vector3.zero, Quaternion.identity, 0);

        EventManager.Subscribe<GameFSMStates>(GameFSM.EVT_ON_CHANGE_GAME_STATE, (CurrentState) =>
        {
            if(CurrentState == GameFSMStates.GAMEPLAY)
            {
                StartCoroutine(SpawnDelay());
            }
            if(CurrentState == GameFSMStates.GAMEOVER)
            {
                StopAllCoroutines();
                RemovePlayersOfType(PlayerType.AI);
            }
        });

        EventManager.Subscribe<Player>(Player.EVT_ON_PLAYER_DEATH, (player) =>
        {
            if(player == LocalPlayer)
            {
                EventManager.Invoke(EVT_ONLOCALPLAYERDEATH);
            }
        });
    }

    IEnumerator SpawnDelay()
    {
        for (int i = 1; i < 10; i++)
        {
            yield return new WaitForSeconds(0.5f);
            SpawnPlayer(PlayerType.AI, UnityEngine.Random.insideUnitSphere * radiusSpwan, Quaternion.identity, i);
        }
        yield break;
    }
	

    public Player SpawnPlayer(PlayerType type, Vector3 position, Quaternion rotation, int ID)
    {
        if(playerPrefab) // && AIPlayerPrefab)
        {
            //Player prefab = type == PlayerType.HUMAN ? playerPrefab : AIPlayerPrefab;
            Player player = Instantiate(playerPrefab, transform, true);
            player.transform.SetPositionAndRotation(position, rotation);

            InputBase input = GenerateInput(type, ID);       
            player.InitializePlayer(type, ID, input);

            if (type == PlayerType.HUMAN)
                SetupPlayer(player);
            else if (type == PlayerType.AI)
                SetupPlayerAI(player);

            
            playerList.Add(player);

            return player;
        }
        else
        {
            Debug.LogError("Player Prefab is null");
            return null;
        }
        
    }

    InputBase GenerateInput(PlayerType type, int ID)
    {
        InputBase input;
        if (type == PlayerType.HUMAN)
            input = new InputPlayer();
        else //if (type == PlayerType.AI)
            input = new InputAI();

        input.Init(ID);
        input.SetActive(false);

        return input;
    }

    void SetupPlayer(Player player)
    {
        CameraFlightFollow cameraFlight = Instantiate(playerCameraPrefab, transform, true);
        PlayerFlightControl flight = player.GetComponent<PlayerFlightControl>();

        player.CameraFlight = cameraFlight;
        player.hitScanner = cameraFlight.GetComponent<HitScanner>();
        player.hitScanner.player = player;

        cameraFlight.SetPlayerFlightControl(flight);
    }
    void SetupPlayerAI(Player player)
    {
        HitScanner hitScanner = player.gameObject.AddComponent<HitScanner>();
        player.hitScanner = hitScanner;
        hitScanner.player = player;

        AIPlayerFSM fsm = player.gameObject.AddComponent<AIPlayerFSM>();
        fsm.StartFSM(player);

        if(player.gameObject.GetComponent<PlayerAim>())
            Destroy(player.gameObject.GetComponent<PlayerAim>());

        player.gameObject.AddComponent<PlayerAIAim>();
    }

    //public void RemovePlayer(string ID)
    //{
    //    for(int i = 0; i < playerList.Count; i++)
    //    {
    //        if(playerList[i].ID == ID)
    //        {
    //            Destroy(playerList[i].gameObject);
    //            playerList.RemoveAt(i);
    //            return;
    //        }
    //    }
    //}

    public void RemovePlayersOfType(PlayerType type)
    {
        List<Player> newList = new List<Player>();

        for (int i = 0; i < playerList.Count; i++)
        {
            if(playerList[i].Type != type)
            {
                newList.Add(playerList[i]);
            }
            else
            {
                Destroy(playerList[i].gameObject);
            }
        }

        playerList = newList;

    }

    public void RemovePlayers()
    {
       for(int i = 0; i < playerList.Count;i++)
        {
            Destroy(playerList[i].gameObject);
        }

        playerList.Clear();

    }
}
