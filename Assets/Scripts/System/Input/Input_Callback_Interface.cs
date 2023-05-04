using UnityEngine;

/// <summary>
/// クリックした際のコールバックインターフェース
/// </summary>
public interface IInputOnClickCallback : InputManager.IInputReceiver
{
    void OnClick(Vector2 click_position);
}

/// <summary>
/// クリックした時に対象にあたった際のコールバックインターフェース
/// </summary>
public interface IInputIsHitCallback : InputManager.IInputReceiver
{
    void OnHit(IInputRayCastAddress address, Vector2 click_position);
}

/// <summary>
/// クリックしている際のコールバックインターフェース
/// </summary>
public interface IInputStayCallback : InputManager.IInputReceiver
{
    void OnStay(Vector2 current_position);
}

/// <summary>
/// クリックし終わった際のコールバックインターフェース
/// </summary>
public interface IInputReleaseCallback : InputManager.IInputReceiver
{
    void OnRelease(Vector2 release_position);
}

/// <summary>
/// フリックした際のコールバックインターフェース
/// </summary>
public interface IInputFlickedCallback : InputManager.IInputReceiver
{
    void OnFlicked(Vector2 flick_direction, float distance);
}
