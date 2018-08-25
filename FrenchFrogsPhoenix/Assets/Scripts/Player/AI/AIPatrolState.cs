using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AIPatrolInputPattern
{
    public float x;
    public float y;

    public AIPatrolInputPattern(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}


public class AIPatrolState : AIPlayerFSMState
{
    float timeElapsed = 0;
    float currentX = 0;
    float destX = 0;
    float currentY = 0;
    float destY = 0;

    AIPatrolInputPattern[] AIPatrolPatternsArray;

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
            new AIPatrolInputPattern(-0.5f,1),
            new AIPatrolInputPattern(0.5f,1),
            new AIPatrolInputPattern(0.5f,-1),
            new AIPatrolInputPattern(-0.5f,-1),
            new AIPatrolInputPattern(-0.5f,1),
            new AIPatrolInputPattern(1,0),
            new AIPatrolInputPattern(-1,0),

        };


    }

    public override void UpdateState()
    {
        if(AIPlayer.input != null)
        {
            timeElapsed += Time.deltaTime;

            if(timeElapsed > 2f)
            {
                timeElapsed = 0;
                AIPatrolInputPattern pattern = AIPatrolPatternsArray[Random.Range(0, AIPatrolPatternsArray.Length)];
                destX = pattern.x;
                destY = pattern.y;
            }

            currentX = Mathf.MoveTowards(currentX, destX, Time.deltaTime * 2);
            currentY = Mathf.MoveTowards(currentY, destY, Time.deltaTime * 2);

            AIPlayer.input.PressLeftStick(currentX, currentY);
            AIPlayer.input.PressRightStick(currentX, currentY);
        }


           
    }



}
