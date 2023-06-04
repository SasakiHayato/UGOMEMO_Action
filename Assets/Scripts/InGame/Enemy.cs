using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBase
{
    [SerializeField]
    protected int needDamageToDropWeapon = 100;
    [SerializeField]
    WeaponBase[] _weapons = default;

    int currentHitDamageAmount = 0;
    int currentEquipWeaponID = 0;
    public override void AddDamage(int damage)
    {
        if(_hp > damage)
        {
            _hp -= damage;
            currentHitDamageAmount += damage;
        }
        OnDead();
        CheckDropWeapon();
    }

    void CheckDropWeapon()
    {
        if(currentHitDamageAmount >= needDamageToDropWeapon)
        {
            _weapons[currentEquipWeaponID]?.DropWeapon();
            currentEquipWeaponID++;
        }
    }

    protected override void OnDead()
    {
        throw new System.NotImplementedException();
    }
}
