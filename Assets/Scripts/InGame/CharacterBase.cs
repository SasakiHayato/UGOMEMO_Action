using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour, ICharacterBehaviour, IDamage
{
    [SerializeField]
    protected int _hp = 100;

    public IDamage Damage => this;

    protected abstract void AddDamage(int damage);

    void IDamage.AddDamage(int amount)=> Damage.AddDamage(amount);

    protected abstract void OnDead();
}
