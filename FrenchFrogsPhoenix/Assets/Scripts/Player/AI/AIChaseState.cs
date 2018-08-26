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



            Debug.Log(9);

            if (Owner.ChasedObject != null)
            {
                Vector3 relativePos = Owner.ChasedObject.transform.position - CachedTransform.position;
                Quaternion rotation = Quaternion.LookRotation(relativePos);

                Debug.Log(rotation);

               

                currentX = Mathf.MoveTowards(currentX, destX, Time.deltaTime * 2);
                currentY = Mathf.MoveTowards(currentY, destY, Time.deltaTime * 2);

                AIPlayer.input.PressLeftStick(currentX, currentY);
                AIPlayer.input.PressRightStick(currentX, currentY);
            }
            else
            {
                timeElapsed += Time.deltaTime;

                if (timeElapsed > 2)
                {
                    Owner.ChangeFSMState(AIPlayerStates.PATROL);
                }
               
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
                timeElapsed = 0;
            }

        }
        else if (CachedPlayer)
        {
            if (CachedPlayer != AIPlayer)
            {
                ChasedObject = CachedPlayer.gameObject.transform;
                timeElapsed = 0;
            }

        }
    }

    protected void OnTriggerExit(Collider collision)
    {
        ChasedObject = null;
       

    }



}
