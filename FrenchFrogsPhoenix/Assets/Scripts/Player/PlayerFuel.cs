using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FuelStates
{
    NORMAL = 0,
    CRITICAL = 1,
    EMPTY = 2
}

public class PlayerFuel : MonoBehaviour {

    [SerializeField]
    private float maxFuel;

    [SerializeField]
    private float criticalFuel;

    [SerializeField]
    private float fuelComsumptionRate;

    public float CurrentFuel { private set; get; }

    public bool IsActive { private set; get; }

    Player player;


    private void Update()
    {
        RemoveFuel(fuelComsumptionRate * Time.deltaTime);
    }
    // Use this for initialization
    void Awake () {
        CurrentFuel = maxFuel;
        player = GetComponent<Player>();
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }

    public void AddFuel(float fuel)
    {
        SetFuel(CurrentFuel + fuel);
    }

    public void RemoveFuel(float fuel)
    {
        SetFuel(CurrentFuel - fuel);
    }

    public FuelStates SetFuel(float fuel)
    {
        if(IsActive)
            CurrentFuel = Mathf.Clamp(fuel, 0, maxFuel);

        InvokeFuelEvent();
        return GetFuelState();
    }

    void InvokeFuelEvent()
    {
        if(player.currentType == PlayerType.HUMAN)
            EventManager.Invoke<float>("UpdatePlayerFuel", CurrentFuel / maxFuel);
    }

    private FuelStates GetFuelState()
    {
        if(CurrentFuel <= 0)
        {
            return FuelStates.EMPTY;
        }
        if(CurrentFuel < criticalFuel)
        {
            return FuelStates.CRITICAL;
        }
        else
        {
            return FuelStates.NORMAL;
        }
    }
}
