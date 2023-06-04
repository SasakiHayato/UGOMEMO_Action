using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Windows;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IInputOnClickCallback
{
    [SerializeField]
    float _teleportDuraration = 0.5f;
    [SerializeField]
    float _dropDownSpeedRate = 2f;
    [SerializeField]
    Ease _moveType = Ease.Linear;


    Rigidbody2D _rb = default;
    // Start is called before the first frame update
    void Start()
    {
        InputManager.Instance.SetOperator(this);
        TryGetComponent(out _rb);
    }
    void IInputOnClickCallback.OnClick(Vector2 click_position)
    {
        Move(click_position);
    }

    void Move(Vector2 target)
    {
        target = Vector2.up * Mathf.FloorToInt(target.y) + Vector2.right * target.x;
        var moveTime = target.y < transform.position.y ? _teleportDuraration / _dropDownSpeedRate : _teleportDuraration;
        _rb.DOMove(target, moveTime).SetEase(_moveType);
    }
}
