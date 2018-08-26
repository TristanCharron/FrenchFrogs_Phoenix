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
               // bool shouldFire = false;

           
                Vector3 relativePos = Owner.ChasedObject.transform.position - CachedTransform.position;

                float dotForward = Vector3.Dot(relativePos, CachedTransform.forward);
                float dotRight = Vector3.Dot(relativePos, CachedTransform.right);
                float dotUp= Vector3.Dot(relativePos, CachedTransform.up);

                currentY = Mathf.MoveTowards(currentX, dotUp, Time.deltaTime * 100);
                currentX = Mathf.MoveTowards(currentY, dotRight, Time.deltaTime * 100);


                AIPlayer.input.PressLeftStick(0, dotForward);
                AIPlayer.input.PressRightStick(currentX, currentY);
            }
            else
            {
                Owner.ChangeFSMState(AIPlayerStates.PATROL);
                timeElapsed = 0;
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
        if (ChasedObject == null)
            return;

        CachedStickingObject = collision.gameObject.GetComponent<StickingObject>();

        CachedPlayer = collision.gameObject.GetComponent<Player>();

        if (CachedStickingObject)
        {
            if (!CachedStickingObject.IsSticked)
            {
                if(Vector3.Distance(CachedStickingObject.transform.position,CachedTransform.position) < Vector3.Distance(ChasedObject.position, CachedTransform.position))
                {
                    ChasedObject = CachedStickingObject.gameObject.transform;
                    timeElapsed = 0;
                }
               
            }

        }
        else if (CachedPlayer)
        {
            if (CachedPlayer != AIPlayer && AIPlayer.Type != CachedPlayer.Type)
            {
                if (Vector3.Distance(CachedPlayer.transform.position, CachedTransform.position) < Vector3.Distance(ChasedObject.position, CachedTransform.position))
                {
                    ChasedObject = CachedPlayer.gameObject.transform;
                    timeElapsed = 0;
                }
                    
            }

        }
        
    }

  

}
