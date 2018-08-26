using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class FiniteStateMachine : MonoBehaviour 
{

    [SerializeField]
    protected string Name;

    [SerializeField]
    bool isDebug;

    protected Dictionary<System.Enum, FSMState> FSMStatesDictionnary;

    protected FSMState CurrentFSMState, PreviousFSMState;

    protected bool isChangingState = false;

    // Use this for initialization
    void Awake()
    {
        isChangingState = false;

    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        if (!isChangingState)
        {
            if (CurrentFSMState != null)
            {
                CurrentFSMState.UpdateState();
            }
        }
    }

   

    protected void AddFSMState(FSMState FSMState)
    {
        Enum currentMenuID = FSMState.EnumID;

        if (FSMStatesDictionnary == null)
            FSMStatesDictionnary = new Dictionary<System.Enum, FSMState>();

        if (!FSMStatesDictionnary.ContainsKey(currentMenuID))
        {
            FSMStatesDictionnary.Add(FSMState.EnumID, FSMState);
        }
        else
        {
            Debug.LogError("State " + currentMenuID.ToString() + " already exists in FSM. ");
        }
    }


    public void ChangeFSMState(System.Enum NextStateID)
    {
        StartCoroutine(ChangeFSMStateCoRoutine(FSMStatesDictionnary[NextStateID].EnumID));
    }

    //private IEnumerator ChangeMenuState(MenuOptionStateID nextMenuID)
    protected virtual IEnumerator ChangeFSMStateCoRoutine(System.Enum NextStateEnumID)
    {
        if (!FSMStatesDictionnary.ContainsKey(NextStateEnumID))
        {
            Debug.LogError("State " + NextStateEnumID.ToString() + " not found in FSM. ");
            yield break;
        }
        else
        {
            if (CurrentFSMState != null)
            {
                PreviousFSMState = CurrentFSMState;
                yield return CurrentFSMState.ExitState();
            }




            CurrentFSMState = FSMStatesDictionnary[NextStateEnumID];

            isChangingState = true;

            yield return CurrentFSMState.EnterState();

            isChangingState = false;

            EventManager.Invoke<System.Enum>("OnChange" + Name, CurrentFSMState.EnumID);
       
            yield break;
        }
    }


}

