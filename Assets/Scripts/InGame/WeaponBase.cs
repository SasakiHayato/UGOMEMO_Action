using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;


public class WeaponBase : MonoBehaviour
{
    [SerializeField]
    int _normalAttackPower = 50;
    [SerializeField]
    int _specialAttackPower = 100;
    [SerializeField]
    float _dropPosY = 0;
    [SerializeField]
    float _dropAngle = 0;
    [SerializeField]
    float _dropDuraration = 0.5f;

    [SerializeField]
    UnityEvent _onDrop = null;

    public virtual int NormalAttack()
    {
        return _normalAttackPower;
    }
    public virtual int SpecialAttack()
    {
        return _specialAttackPower;
    }

    public virtual void DropWeapon()
    {
        _onDrop?.Invoke();
        SetWeaponState(WeaponState.OnGround);
    }

    public void SetWeaponState(WeaponState state)
    {
        switch (state)
        {
            case WeaponState.InEnemy:
                break;
            case WeaponState.OnGround:
                ReleaseHoldPos();
                transform.DOMoveY(_dropPosY, _dropDuraration);
                transform.DORotate(Vector3.forward * _dropAngle, _dropDuraration);
                break;
            case WeaponState.InPlayer:
                break;
        }
    }

    public void SetHoldPos(Transform parent)
    {
        transform.position = Vector3.zero;
        transform.SetParent(parent);
    }

    public void ReleaseHoldPos()
    {
        transform.SetParent(null);
    }
}
