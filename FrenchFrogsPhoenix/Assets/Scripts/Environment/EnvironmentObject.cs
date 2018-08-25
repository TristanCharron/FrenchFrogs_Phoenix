using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnvironmentObject : MonoBehaviour {

    [SerializeField] float randomSpeed = Random.Range(1,10);
    Vector3 startPosition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartMovement()
    {
        transform.DOLocalMove(new Vector3(startPosition.x + Random.Range(-5,5),startPosition.y + Random.Range(-5, 5), startPosition.z + Random.Range(-5, 5)) ,randomSpeed).OnComplete(ResetMovement);
    }

    public void ResetMovement()
    {
        transform.DOLocalMove(startPosition,randomSpeed).OnComplete(StartMovement);
    }
        
}
