﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AIPatrolInputPattern
{
    public float x;
    public float y;
    public float Rx;
    public float Ry;

    public AIPatrolInputPattern(float x, float y, float Rx, float Ry)
    {
        this.x = x;
        this.y = y;
        this.Rx = Rx;
        this.Ry = Ry;
    }
}


public class AIPatrolState : AIPlayerFSMState
{
    Transform ChasedObject = null;

    StickingObject CachedStickingObject = null;

    Player CachedPlayer = null;

    AIPatrolInputPattern currentPattern;

    protected override void Awake()
    {
        enumID = AIPlayerStates.PATROL;
       
    }

    protected override void Start()
    {
        timeElapsed = 0;
        currentX = 0;
        currentY = 0;
        destX = Random.Range(-1, 1);
        destY = Random.Range(-1, 1);



        AIPatrolPatternsArray = new AIPatrolInputPattern[]
        {
            new AIPatrolInputPattern(0,1,0,0),
            new AIPatrolInputPattern(1,0,0,0),
            new AIPatrolInputPattern(1,1,0,0),
            new AIPatrolInputPattern(1,1,0,0),
            new AIPatrolInputPattern(-1,0,0,0),
            new AIPatrolInputPattern(0,-1,0,0),
            new AIPatrolInputPattern(-1,-1,0,0),
        };


    }



 

    public override void UpdateState()
    {
        if(AIPlayer.input != null)
        {
            if (CachedTransform == null)
                CachedTransform = AIPlayer.transform;


            timeElapsed += Time.deltaTime;

            if(timeElapsed > 7f)
            {
                timeElapsed = 0;
                currentPattern = AIPatrolPatternsArray[Random.Range(0, AIPatrolPatternsArray.Length)];
                destX = currentPattern.x;
                destY = currentPattern.y;
            }

            currentX = Mathf.MoveTowards(currentX, destX, Time.deltaTime * 2);
            currentY = Mathf.MoveTowards(currentY, destY, Time.deltaTime * 2);

            AIPlayer.input.PressLeftStick(currentX, currentY);
            AIPlayer.input.PressRightStick(currentPattern.Rx, currentPattern.Ry);

          
        }


           
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
                Owner.SetChasedObject(ChasedObject.gameObject);
                Owner.ChangeFSMState(AIPlayerStates.CHASE);
            }

        }
        else if (CachedPlayer)
        {
            if (CachedPlayer != AIPlayer && AIPlayer.Type != CachedPlayer.Type)
            {
                ChasedObject = CachedPlayer.gameObject.transform;
                Owner.SetChasedObject(ChasedObject.gameObject);
                Owner.ChangeFSMState(AIPlayerStates.CHASE);
            }

        }


    }



}
