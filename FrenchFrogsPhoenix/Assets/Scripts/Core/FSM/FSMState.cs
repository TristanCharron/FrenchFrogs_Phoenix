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

    public abstract void UpdateState();

    protected abstract void Awake();

    protected abstract void Start();

    public virtual IEnumerator EnterState()
    {
        isCurrentState = true;
        yield break;
    }

    public virtual IEnumerator ExitState()
    {
        isCurrentState = false;
        yield break;
    }

}
