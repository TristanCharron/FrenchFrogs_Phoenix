using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class MenuController : MonoBehaviour {

    [SerializeField] Text startGameTxt;
    //TextMeshProUGUI MenuText;

    PlayerInput input;

    bool isActive = false;

    // Use this for initialization
    void Awake() {

        input = new PlayerInput(0);
        input.SetActive(true);

        EventManager.Subscribe<System.Enum>("OnChangeGameFSM", (nextState) => {

            bool isActive = nextState.ToString() == GameFSMStates.MAINMENU.ToString();
            SetActive(isActive);
        });

        input.FireButton.AddEvent(() =>
        {
            if (isActive)
            {
                 Debug.Log("START GAME");
                 SetActive(false);
                 EventManager.Invoke<GameFSMStates>(GameFSM.EVT_ON_CHANGE_GAME_STATE, GameFSMStates.GAMEPLAY);
            }


        });

   


    }

    private void Update()
    {
        input.Update();
    }

    private void SetActive(bool active)
    {
        isActive = active;
    }



}
