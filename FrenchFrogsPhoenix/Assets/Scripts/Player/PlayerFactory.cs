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


    [SerializeField]
    Player PlayerPrefab;

    [SerializeField]
    Player AIPlayerPrefab;

    [SerializeField]
    float radiusSpwan = 50;

    List<Player> PlayerList;

    public Player LocalPlayer { get; private set; }

	// Use this for initialization
	void Start () {
        PlayerList = new List<Player>();

        LocalPlayer = SpawnPlayer(PlayerType.HUMAN, Vector3.zero, Quaternion.identity);

       
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
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.5f);
            SpawnPlayer(PlayerType.AI, UnityEngine.Random.insideUnitSphere * radiusSpwan, Quaternion.identity);
        }
        yield break;
    }
	

    public Player SpawnPlayer(PlayerType type, Vector3 position, Quaternion rotation)
    {
        if(PlayerPrefab && AIPlayerPrefab)
        {
            Player player = Instantiate(type == PlayerType.HUMAN ? PlayerPrefab : AIPlayerPrefab, transform, true);
            player.transform.SetPositionAndRotation(position, rotation);
            player.Spawn(type,DateTime.Now.ToString());
            PlayerList.Add(player);

            return player;

        }
        else
        {
            Debug.LogError("Player Prefab is null");
            return null;
        }
        
    }

    public void RemovePlayer(string ID)
    {
        for(int i = 0; i < PlayerList.Count; i++)
        {
            if(PlayerList[i].ID == ID)
            {
                Destroy(PlayerList[i].gameObject);
                PlayerList.RemoveAt(i);
                return;
            }
        }
    }

    public void RemovePlayersOfType(PlayerType type)
    {
        List<Player> newList = new List<Player>();

        for (int i = 0; i < PlayerList.Count; i++)
        {
            if(PlayerList[i].Type != type)
            {
                newList.Add(PlayerList[i]);
            }
            else
            {
                Destroy(PlayerList[i].gameObject);
            }
        }

        PlayerList = newList;

    }

    public void RemovePlayers()
    {
       for(int i = 0; i < PlayerList.Count;i++)
        {
            Destroy(PlayerList[i].gameObject);
        }

        PlayerList.Clear();

    }
}
