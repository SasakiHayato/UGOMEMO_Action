using UnityEngine;

public class Test_InputUser : MonoBehaviour, IInputOnClickCallback, IInputIsHitCallback, IInputStayCallback, IInputReleaseCallback, IInputFlickedCallback
{
    [SerializeField] bool _isViewLogOnClick;
    [SerializeField] bool _isViewLogOnHit;
    [SerializeField] bool _isViewLogOnStay;
    [SerializeField] bool _isViewLogOnRelease;
    [SerializeField] bool _isViewLogOnFlicked;

    void Start()
    {
        InputManager.Instance.SetOperator(this);
    }

    void IInputOnClickCallback.OnClick(Vector2 click_position)
    {
        if (!_isViewLogOnClick) return;
        Debug.Log($"Click / Position:{click_position}");
    }

    void IInputIsHitCallback.OnHit(IInputRayCastAddress address, Vector2 click_position)
    {
        if (!_isViewLogOnHit) return;
        Debug.Log($"Hit / Address:{address.ObjectType} / Position:{click_position}");
    }

    void IInputStayCallback.OnStay(Vector2 current_position)
    {
        if (!_isViewLogOnStay) return;
        Debug.Log($"Stay / Position{current_position}");
    }

    void IInputReleaseCallback.OnRelease(Vector2 release_position)
    {
        if (!_isViewLogOnRelease) return;
        Debug.Log($"Release / Position:{release_position}");
    }

    void IInputFlickedCallback.OnFlicked(Vector2 flick_direction, float distance)
    {
        if (!_isViewLogOnFlicked) return;
        Debug.Log($"Flick / Direction:{flick_direction} / Distance:{distance}");
    }
}
