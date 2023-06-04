using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponBase : MonoBehaviour
{
    [SerializeField]
    int _normalAttackPower = 50;
    [SerializeField]
    int _specialAttackPower = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void NormalAttack()
    {

    }
    public virtual void SpecialAttack()
    {

    }

    public virtual void DropWeapon()
    {

    }

    public void SetWeaponState(WeaponState state)
    {
        switch (state)
        {
            case WeaponState.InEnemy:
                break;
            case WeaponState.OnGround:
                break;
            case WeaponState.InPlayer:
                break;
        }
    }
}
