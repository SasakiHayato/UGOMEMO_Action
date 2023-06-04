using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    [SerializeField]
    protected int _hp = 100;
    public abstract void AddDamage(int damage);

    protected abstract void OnDead();
}
