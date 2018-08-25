using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentMeteorites : EnvironmentObject
{
    Vector3 limitBound;
	// Use this for initialization
	public override void Start () {
        base.Start();
        limitBound = EnvironmentController.GetInstance().maxBounds;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Vector3.forward * Time.deltaTime * randomSpeed;
        if(transform.position.z > limitBound.z)
        {
            EnvironmentController.GetInstance().RemoveObject(this);
        }
    }
}
