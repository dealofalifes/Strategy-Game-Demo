using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
    private float _MaxHealth;
    private float _CurrentHealth;
    private float _RegenerationTimer; //HP5, every 5 seconds owner will be healed +%10 of their max Health

    public Stats(float _maxHP)
    {
        _MaxHealth = _maxHP;
        _CurrentHealth = _maxHP;
        _RegenerationTimer = 0;
    }

    public bool TakeDamage(float _amount)
    {
        _CurrentHealth -= _amount;

        if (_CurrentHealth <= 0)
        {
            Debug.Log("Target is destroyed!");
            return true;
        }

        return false;
    }

    public float GetCurrentHealth()
    {
        return _CurrentHealth;
    }

    public float GetMaxHealth()
    {
        return _MaxHealth;
    }
}
