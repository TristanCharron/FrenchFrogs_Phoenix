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
    private float _MaxFuel;

    [SerializeField]
    private float _CriticalFuel;

    [SerializeField, Range(0,2)]
    private float _FuelComsumptionRate;
    public float FuelComsumptionRate { private set; get; }

    public float CurrentFuel { private set; get; }

    public bool IsActive { private set; get; }

    [SerializeField]
    public Player player;

	
    // Use this for initialization
	void Awake () {
        CurrentFuel = _MaxFuel;
    }

    private void SetActive(bool isActive)
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
       CurrentFuel = Mathf.Clamp(fuel, 0, _MaxFuel);

        return GetFuelState();
    }

    private FuelStates GetFuelState()
    {
        if(CurrentFuel <= 0)
        {
            return FuelStates.EMPTY;
            
        }
        if(CurrentFuel < _CriticalFuel)
        {
            return FuelStates.CRITICAL;
        }
        else
        {
            return FuelStates.NORMAL;
        }
    }

   
}
