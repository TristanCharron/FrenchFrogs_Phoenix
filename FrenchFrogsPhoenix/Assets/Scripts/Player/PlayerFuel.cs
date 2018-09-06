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

    [SerializeField] float maxFuel;
    [SerializeField] float criticalFuel;
    [SerializeField] float fuelRegen;
    [SerializeField] float timerBeforeRegen = 1;
    [SerializeField] AnimationCurve regenCurve;

    public float FuelRatio {
        get {
            return (CurrentFuel / maxFuel);
        }
    }
    public float CurrentFuel { private set; get; }
    public bool IsActive { private set; get; }

    Player player;

    float didntUseFuelTimer = 0;

    private void Update()
    {
        RegenFuel();
    }

    void Awake ()
    {
        CurrentFuel = maxFuel;
        player = GetComponent<Player>();
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }

    void RegenFuel()
    {
        if (didntUseFuelTimer > timerBeforeRegen)
        {
            AddFuel(fuelRegen * Time.deltaTime * regenCurve.Evaluate(FuelRatio));
        }
        else
        {
            didntUseFuelTimer += Time.deltaTime;
        }
    }

    public void AddFuel(float fuel)
    {
        SetFuel(CurrentFuel + fuel);
    }

    public void RemoveFuel(float fuel)
    {
        didntUseFuelTimer = 0;
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
        EventManager.Invoke<float>(EventConst.GetUpdatePlayerFuel(player.ID), CurrentFuel / maxFuel);
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
