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

   // [SerializeField] Player AIPlayerPrefab;

    [SerializeField] float radiusSpwan = 50;

    List<Player> playerList;

    public Player LocalPlayer { get; private set; }

	// Use this for initialization
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
            player.Spawn(type,ID);
            playerList.Add(player);

            if (type == PlayerType.HUMAN)
                SetCameraToPlayer(player);

            return player;
        }
        else
        {
            Debug.LogError("Player Prefab is null");
            return null;
        }
        
    }

    void SetCameraToPlayer(Player player)
    {
        CameraFlightFollow cameraFollow = Instantiate(playerCameraPrefab, transform, true);
        PlayerFlightControl flight = player.GetComponent<PlayerFlightControl>();
        cameraFollow.SetPlayerFlightControl(flight);
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
