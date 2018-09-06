using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MassObject : Mass {
    [SerializeField] StickingObject stickingObject;

    public override void Start()
    {
        base.Start();
        stickingObject.SetObjectStats(new ObjectStats());
        stickingObject.SetFirstStickingchild(this);
        stickingObject.SetMeshChild(core);
    }

    private void OnTriggerStay(Collider other)
    {
        StickingObject stickingObject = other.GetComponent<StickingObject>();
        if(stickingObject)
        {
            Attraction(stickingObject.rb);
        }
    }
}
