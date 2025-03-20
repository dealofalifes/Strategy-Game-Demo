using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public void PrepareDamage();

    public Vector2Int GetPosition();

    public Vector2Int GetSize();

    public void TakeDamage(float damage);

    public void Dead();
}
