using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;





public abstract class FSMState: MonoBehaviour 
{
    [SerializeField]
    protected System.Enum enumID;

    public System.Enum EnumID { get { return enumID; } }

    public abstract void UpdateState();

    protected abstract void Awake();

    protected abstract void Start();

    public virtual IEnumerator EnterState()
    {
        yield break;
    }

    public virtual IEnumerator ExitState()
    {
        yield break;
    }

}
