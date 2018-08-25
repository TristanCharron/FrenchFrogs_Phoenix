using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI MenuText;

    bool isActive = false;

	// Use this for initialization
	void Awake () {

        EventManager.Subscribe<System.Enum>("OnChangeGameFSM", (nextState)=>{

            bool isActive = nextState.ToString() == GameFSMStates.MAINMENU.ToString();
                SetActive(isActive);
            });

        EventManager.Subscribe<string>(KeyboardInputController.EVT_KEYBOARD_PRESS, (key) => {

            if(isActive)
            {
                if (key == "Fire")
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
        MenuText.gameObject.GetComponent<CanvasGroup>().alpha = active ? 1 : 0;
        isActive = active;
    }



}
