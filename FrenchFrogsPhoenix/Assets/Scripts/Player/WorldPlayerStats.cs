using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WorldPlayerStats : MonoBehaviour {

    public Player playerToLookAt;

    [SerializeField] Player currentPlayer;
    bool isActive;

    // Use this for initialization
    void Start () {
        HideStats();
	}
	
	// Update is called once per frame
	void Update () {
		if(isActive && playerToLookAt.currentType != PlayerType.AI)
        {
            transform.LookAt(playerToLookAt.GetPlayerCamera().transform);
        }
	}

    public void ShowStats(Player playerRef)
    {
        if (playerRef.currentType == PlayerType.HUMAN && playerRef != currentPlayer)
        {
            playerToLookAt = playerRef;
            isActive = true;
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBounce);
        }
    }

    public void HideStats()
    {
        isActive = false;
        playerToLookAt = null;
        transform.DOScale(Vector3.zero, 0.5f);
    }
}
