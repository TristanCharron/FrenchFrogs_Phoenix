using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentMeteorites : EnvironmentObject
{
    Vector3 limitBound;
    Vector3 direction;
	// Use this for initialization
	public override void Start () {
        base.Start();
        limitBound = EnvironmentController.GetInstance().maxBounds;
        direction = new Vector3(Random.Range(0.1f,1f),Random.Range(0.1f,1f),Random.Range(0.1f,1f));
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += direction * Time.deltaTime * randomSpeed;
        if(transform.position.z > limitBound.z)
        {
            EnvironmentController.GetInstance().RemoveObject(this);
        }
    }
}
