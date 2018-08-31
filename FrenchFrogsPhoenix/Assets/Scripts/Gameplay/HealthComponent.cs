using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour {

    public FloatUnityEvent OnDamageTaken = new FloatUnityEvent();
    public UnityEvent OnDeathEvent = new UnityEvent();

    [SerializeField] float maxHp;
    [SerializeField] float hpRegen;
    [SerializeField] float armor;

    float currentHp;
    float invisibilityTime = 0.5f;
    bool isInvincible;

    Coroutine invinciblityCoroutine;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        currentHp = maxHp;
        isInvincible = false;
    }

    public void Update()
    {
        Heal(hpRegen * Time.deltaTime);
    }

    public void Damage(DamageData damageData)
    {
        if (isInvincible || this == damageData.owner)
            return;

        currentHp -= damageData.damage;
        OnDamageTaken.Invoke(currentHp/maxHp);

        invinciblityCoroutine = StartCoroutine(InvincibilityDelay());

        if (currentHp <= 0)
        {
            Debug.Log("je meur..s");
            OnDeathEvent.Invoke();
            StopCoroutine(invinciblityCoroutine);
        }
    }

    public void Heal(float healingAmmount)
    {
        currentHp = Mathf.Clamp(currentHp+healingAmmount, 0, maxHp);
    }

    IEnumerator InvincibilityDelay()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invisibilityTime);
        isInvincible = false;
    }
}

public class FloatUnityEvent:UnityEvent<float>{}