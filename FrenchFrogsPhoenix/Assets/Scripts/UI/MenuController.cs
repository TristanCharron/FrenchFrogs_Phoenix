using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class MenuController : MonoBehaviour {

    [SerializeField] Text startGameTxt;
    //TextMeshProUGUI MenuText;

    bool isActive = false;

	// Use this for initialization
	void Awake () {

        EventManager.Subscribe<System.Enum>("OnChangeGameFSM", (nextState)=>{

            bool isActive = nextState.ToString() == GameFSMStates.MAINMENU.ToString();
                SetActive(isActive);
            });

        EventManager.Subscribe<InputActionEventData>(RewiredInputProvider.EVT_INPUT_PRESS_DOWN, (input) => {

            if(isActive)
            {
                if (input.actionName == "Fire")
                {
                    Debug.Log("START GAME");
                    SetActive(false);
                    EventManager.Invoke<GameFSMStates>(GameFSM.EVT_ON_CHANGE_GAME_STATE, GameFSMStates.GAMEPLAY);
                }
            }
           
        });

   


    }


    private void SetActive(bool active)
    {
        isActive = active;
    }



}
