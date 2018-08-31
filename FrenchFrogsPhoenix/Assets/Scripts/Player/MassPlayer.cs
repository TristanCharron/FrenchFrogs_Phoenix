using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassPlayer : Mass {

    bool isActive;

	// Use this for initialization
	public override void Start () {
        base.Start();
	}

    private void OnTriggerStay(Collider other)
    {
        StatsNode stickingObject = other.GetComponent<StatsNode>();
        if (stickingObject && isActive)
        {
           // Attraction(stickingObject);
        }
    }
}
