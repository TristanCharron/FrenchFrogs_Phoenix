using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RewiredConsts;

public class AIChaseState : AIPlayerFSMState
{
    Transform ChasedObject = null;

    StickingObject CachedStickingObject = null;
    Player CachedPlayer = null;

    public override void UpdateState()
    {
        base.UpdateState();

        if (AIPlayer.input != null)
        {
            if (CachedTransform == null)
                CachedTransform = AIPlayer.transform;



            TimeElapsed += Time.deltaTime;

            if (TimeElapsed > 5)
            {
                Owner.ChangeFSMState(AIPlayerStates.PATROL);
                TimeElapsed = 0;
            }


            if (Owner.ChasedObject != null)
            {
               // bool shouldFire = false;

           
                Vector3 relativePos = Owner.ChasedObject.transform.position - CachedTransform.position;

                float dotForward = Vector3.Dot(relativePos, CachedTransform.forward);
                float dotRight = Vector3.Dot(relativePos, CachedTransform.right);
                float dotUp= Vector3.Dot(relativePos, CachedTransform.up);

                if (Owner.ChasedObject.GetComponent<Player>())
                {
                    if(TimeElapsed % 2 == 0)
                        Input.SetButton(Action.Fire, true);
                }

                currentY = Mathf.MoveTowards(currentX, dotUp, Time.deltaTime * 100);
                currentX = Mathf.MoveTowards(currentY, dotRight, Time.deltaTime * 100);

                Input.SetAxis(Action.MoveHorizontal, dotForward);
                Input.SetAxis(Action.CameraHorizontal, currentX);
                Input.SetAxis(Action.CameraVertical, currentY);

                //AIPlayer.input.PressLeftStick(0, dotForward);
                //AIPlayer.input.PressRightStick(currentX, currentY);
            }
            else
            {
                Owner.ChangeFSMState(AIPlayerStates.PATROL);
                TimeElapsed = 0;
            }
       

        }
    }

    protected override void Awake()
    {
        enumID = AIPlayerStates.CHASE;
    }

    protected override void Start()
    {
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
                    TimeElapsed = 0;
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
                    TimeElapsed = 0;
                }
                    
            }
        }
    }
}
