using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RewiredConsts;

public class AIChaseState : AIPlayerFSMState
{
    Transform chasedObject = null;

    StickingObject cachedStickingObject = null;
    Player cachedPlayer = null;
    float timeBeforePatrol = 5;
    PlayerAIAim aim;

    public override void UpdateState()
    {
        base.UpdateState();

        if (AIPlayer.input != null)
        {
            if (CachedTransform == null)
                CachedTransform = AIPlayer.transform;

            TimeElapsed += Time.deltaTime;

            if (TimeElapsed > timeBeforePatrol)
            {
                Owner.ChangeFSMState(AIPlayerStates.PATROL);
                TimeElapsed = 0;
            }

            if (Owner.ChasedObject != null)
            {
                ChaseObject();

            }
            else
            {
                Owner.ChangeFSMState(AIPlayerStates.PATROL);
                TimeElapsed = 0;
            }
        }
    }

    private void ChaseObject()
    {
        Vector3 relativePos = Owner.ChasedObject.transform.position - CachedTransform.position;

        float dotForward = Vector3.Dot(relativePos, CachedTransform.forward);
        float dotRight = Vector3.Dot(relativePos, CachedTransform.right);
        float dotUp = Vector3.Dot(relativePos, CachedTransform.up);

        ///if (Owner.ChasedObject.GetComponent<Player>())
        //{
            if (TimeElapsed % 10 == 0)
                Input.SetButton(Action.Fire, true);
        //}

        currentY = Mathf.MoveTowards(currentX, dotUp, Time.deltaTime * 100);
        currentX = Mathf.MoveTowards(currentY, dotRight, Time.deltaTime * 100);

        Input.SetAxis(Action.MoveVertical, dotForward);
        Input.SetAxis(Action.MoveHorizontal, currentX);
        Input.SetAxis(Action.CameraVertical, currentY);
    }

    protected override void Awake()
    {
        enumID = AIPlayerStates.CHASE;
    }

    protected override void Start()
    {
        aim = GetComponent<PlayerAIAim>();
    }

    protected void AimTarget()
    {
        Collider collider = aim.currentTarget;

        if (chasedObject == null || collider == null)
            return;

        cachedStickingObject = collider.gameObject.GetComponent<StickingObject>();

        cachedPlayer = collider.gameObject.GetComponent<Player>();

        if (cachedStickingObject)
        {
            if (!cachedStickingObject.IsSticked)
            {
                if(Vector3.Distance(cachedStickingObject.transform.position,CachedTransform.position) < Vector3.Distance(chasedObject.position, CachedTransform.position))
                {
                    chasedObject = cachedStickingObject.gameObject.transform;
                    TimeElapsed = 0;
                }   
            }
        }
        else if (cachedPlayer)
        {
            if (cachedPlayer != AIPlayer && AIPlayer.Type != cachedPlayer.Type)
            {
                if (Vector3.Distance(cachedPlayer.transform.position, CachedTransform.position) < Vector3.Distance(chasedObject.position, CachedTransform.position))
                {
                    chasedObject = cachedPlayer.gameObject.transform;
                    TimeElapsed = 0;
                }
                    
            }
        }
    }
}
