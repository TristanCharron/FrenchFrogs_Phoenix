using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {

        EventManager.Subscribe<System.Enum>("OnChangeGameFSM", (nextState)=>{
            if(nextState.ToString() == GameFSMStates.MAINMENU.ToString())
            {
                Debug.Log("START MENU");
            }
        });
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
