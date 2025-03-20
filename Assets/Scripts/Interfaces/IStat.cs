using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStat
{
    public bool TakeDamage(float _amount);

    public (float _maxHealth, float _currentHealth, float _ratio) GetHealthRatio();
}
