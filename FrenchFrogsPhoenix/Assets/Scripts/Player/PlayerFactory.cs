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

    [SerializeField]
    Player PlayerPrefab;

    [SerializeField]
    Player AIPlayerPrefab;

    List<Player> PlayerList;

	// Use this for initialization
	void Start () {
        PlayerList = new List<Player>();

        SpawnPlayer(PlayerType.HUMAN, Vector3.zero, Quaternion.identity);

        StartCoroutine(SpawnDelay());
     
       
     
    }

    IEnumerator SpawnDelay()
    {
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(0.5f);
            SpawnPlayer(PlayerType.AI, UnityEngine.Random.insideUnitSphere * 4, Quaternion.identity);
        }
        yield break;
    }
	

    public void SpawnPlayer(PlayerType type, Vector3 position, Quaternion rotation)
    {
        if(PlayerPrefab && AIPlayerPrefab)
        {
            Player player = Instantiate(type == PlayerType.HUMAN ? PlayerPrefab : AIPlayerPrefab, transform, true);
            player.transform.SetPositionAndRotation(position, rotation);
            player.Spawn(type,DateTime.Now.ToString());
            PlayerList.Add(player);

        }
        else
        {
            Debug.LogError("Player Prefab is null");
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

    public void RemovePlayers()
    {
       for(int i = 0; i < PlayerList.Count;i++)
        {
            Destroy(PlayerList[i].gameObject);
        }

        PlayerList.Clear();

    }
}
