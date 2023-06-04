using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Enemy : CharacterBase,IInputRayCastAddress
{
    [SerializeField]
    protected int needDamageToDropWeapon = 100;
    [SerializeField]
    Transform []_weaponParentPoses = default;
    [SerializeField]
    WeaponBase[] _weapons = default;

    int currentHitDamageAmount = 0;
    int currentEquipWeaponID = 0;

    public int ID { get; set; }

    public Define.ObjectType ObjectType => Define.ObjectType.Enemy;

    public ICharacterBehaviour Character => this;

    public Vector2 Position => transform.position;

    private void Start()
    {
        InputManager.Instance.AllocateAddress(this);
    }

    protected override void AddDamage(int damage)
    {
        if(_hp > damage)
        {
            _hp -= damage;
            currentHitDamageAmount += damage;
        }
        else OnDead();
        
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
        _hp = 0;
        Destroy(gameObject);
        Debug.Log($"Enemy{ID} Dead");
    }
}
