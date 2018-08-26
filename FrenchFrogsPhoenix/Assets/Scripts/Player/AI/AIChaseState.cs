using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChaseState : AIPlayerFSMState
{

    Transform ChasedObject = null;

    StickingObject CachedStickingObject = null;
    Player CachedPlayer = null;


 


    public override void UpdateState()
    {
        if (AIPlayer.input != null)
        {
            if (CachedTransform == null)
                CachedTransform = AIPlayer.transform;



            timeElapsed += Time.deltaTime;

            if (timeElapsed > 5)
            {
                Owner.ChangeFSMState(AIPlayerStates.PATROL);
                timeElapsed = 0;
            }


            if (Owner.ChasedObject != null)
            {
                Vector3 relativePos = Owner.ChasedObject.transform.position - CachedTransform.position;

                float dotForward = Vector3.Dot(relativePos, CachedTransform.forward);
                float dotRight = Vector3.Dot(relativePos, CachedTransform.right);
                float dotUp= Vector3.Dot(relativePos, CachedTransform.up);

                currentX = Mathf.MoveTowards(currentX, dotUp, Time.deltaTime * 2);
                currentY = Mathf.MoveTowards(currentY, dotRight, Time.deltaTime * 2);

                AIPlayer.input.PressLeftStick(0, dotForward);
                AIPlayer.input.PressRightStick(dotRight, dotUp);
            }
       

        }
    }

    protected override void Awake()
    {
        enumID = AIPlayerStates.CHASE;
    }

    protected override void Start()
    {
        timeElapsed = 0;


    }

    protected void OnTriggerEnter(Collider collision)
    {

        CachedStickingObject = collision.gameObject.GetComponent<StickingObject>();
        CachedPlayer = collision.gameObject.GetComponent<Player>();

        if (CachedStickingObject)
        {
            if (!CachedStickingObject.IsSticked)
            {
                ChasedObject = CachedStickingObject.gameObject.transform;
            }

        }
        else if (CachedPlayer)
        {
            if (CachedPlayer != AIPlayer)
            {
                ChasedObject = CachedPlayer.gameObject.transform;
            }

        }

    }

  

}
