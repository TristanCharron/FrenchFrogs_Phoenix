using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WorldPlayerStats : MonoBehaviour {

    public Player playerToLookAt;

    [SerializeField] Player currentPlayer;
    float speed = 0.5f;
    bool isActive;
    Camera mainCamera;

    // Use this for initialization
    void Start () {
        HideStats();
        mainCamera = Camera.main;

    }
	
	// Update is called once per frame
	void Update () {
		if(isActive && playerToLookAt.currentType != PlayerType.AI)
        {
            transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, transform.rotation, speed * Time.deltaTime);
            //transform.LookAt(Mathf.Lerp(playerToLookAt.GetPlayerCamera().transform.eulerAngles);
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
