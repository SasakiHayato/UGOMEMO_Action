using UnityEngine;

/// <summary>
/// �N���b�N�����ۂ̃R�[���o�b�N�C���^�[�t�F�[�X
/// </summary>
public interface IInputOnClickCallback : InputManager.IInputReceiver
{
    void OnClick(Vector2 click_position);
}

/// <summary>
/// �N���b�N�������ɑΏۂɂ��������ۂ̃R�[���o�b�N�C���^�[�t�F�[�X
/// </summary>
public interface IInputIsHitCallback : InputManager.IInputReceiver
{
    void OnHit(IInputRayCastAddress address, Vector2 click_position);
}

/// <summary>
/// �N���b�N���Ă���ۂ̃R�[���o�b�N�C���^�[�t�F�[�X
/// </summary>
public interface IInputStayCallback : InputManager.IInputReceiver
{
    void OnStay(Vector2 current_position);
}

/// <summary>
/// �N���b�N���I������ۂ̃R�[���o�b�N�C���^�[�t�F�[�X
/// </summary>
public interface IInputReleaseCallback : InputManager.IInputReceiver
{
    void OnRelease(Vector2 release_position);
}

/// <summary>
/// �t���b�N�����ۂ̃R�[���o�b�N�C���^�[�t�F�[�X
/// </summary>
public interface IInputFlickedCallback : InputManager.IInputReceiver
{
    void OnFlicked(Vector2 flick_direction, float distance);
}
