using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class FSMState: MonoBehaviour 
{
    [SerializeField]
    protected System.Enum enumID;

    public System.Enum EnumID { get { return enumID; } }

    protected bool isCurrentState;

    protected float TimeElapsed;

    protected FiniteStateMachine Owner;

    public virtual void UpdateState()
    {
        TimeElapsed += Time.deltaTime;
    }

    protected abstract void Awake();

    protected abstract void Start();

    public virtual void SetOwner(FiniteStateMachine Owner)
    {
        this.Owner = Owner;
    }

    public virtual IEnumerator EnterState()
    {
        isCurrentState = true;
        TimeElapsed = 0;
        yield break;
    }

    public virtual IEnumerator ExitState()
    {
        isCurrentState = false;
        TimeElapsed = 0;
        yield break;
    }

}
