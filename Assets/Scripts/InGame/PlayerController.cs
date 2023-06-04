using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Windows;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IInputOnClickCallback,IInputIsHitCallback
{
    [SerializeField]
    float _teleportDuraration = 0.5f;
    [SerializeField]
    int _defaultDamage = 10;
    [SerializeField]
    float _dropDownSpeedRate = 2f;
    [SerializeField]
    Ease _moveType = Ease.Linear;

    WeaponBase _weapon = null;
    Rigidbody2D _rb = default;
    // Start is called before the first frame update
    void Start()
    {
        InputManager.Instance.SetOperator(this);
        TryGetComponent(out _rb);
    }
    async void IInputOnClickCallback.OnClick(Vector2 click_position)
    {
       await Move(click_position);
    }

    async UniTask Move(Vector2 target)
    {
        target = Vector2.up * Mathf.FloorToInt(target.y) + Vector2.right * target.x;
        var moveTime = target.y < transform.position.y ? _teleportDuraration / _dropDownSpeedRate : _teleportDuraration;
        await _rb.DOMove(target, moveTime).SetEase(_moveType);
    }

    async void IInputIsHitCallback.OnHit(IInputRayCastAddress address, Vector2 click_position)
    {
        Debug.Log(address.ObjectType);
        if (address.ObjectType == Define.ObjectType.Enemy) 
        {
            await Move(click_position);
            var damage = _weapon == null ? _defaultDamage : _weapon.NormalAttack();
            address.Character.Damage.AddDamage(damage);
        }
    }
}
