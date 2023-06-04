using UnityEngine;

/// <summary>
/// �V�[���̗����グ�A�j�����ɃC�x���g��ǉ����邽�߂̃C���^�[�t�F�[�X
/// </summary>
public interface ILoader
{
    /// <summary>
    /// �V�[�������グ���̏���
    /// </summary>
    System.Collections.IEnumerator OnLoad();

    /// <summary>
    /// �V�[���j�����̏���
    /// </summary>
    void UnLoad();
}


/// <summary>
/// �eScene�Ǘ��̊��N���X
/// </summary>
public abstract class SceneBase : MonoBehaviour, ILoader
{
    //=================================================
    // protected method
    //=================================================

    /// <summary>
    /// �V�[�������グ���̏���.
    /// UnityEngine.Start()
    /// </summary>
    protected abstract System.Collections.IEnumerator Setup();

    /// <summary>
    /// �}�C�t���[���̏���.
    /// UnityEngine.Update()
    /// </summary>
    protected virtual void UpdateEvent() { }

    /// <summary>
    /// �V�[���j�����̏���.
    /// UnityEngine.OnDestroy()
    /// </summary>
    protected virtual void OnDestroyEvent() { }

    //=================================================
    // ILoader interface
    //=================================================

    System.Collections.IEnumerator ILoader.OnLoad()
    {
        yield return Setup();
    }

    void ILoader.UnLoad()
    {
        OnDestroyEvent();
    }

    //=================================================
    // Unity method
    //=================================================

    private void Awake()
    {
        var game_local_data = new GameLocalData() as ILoader;
        var sound_manager = new SoundManager() as ILoader;
        var input_manager = new InputManager() as ILoader;
        var scene = this as ILoader;

        StartCoroutine(game_local_data.OnLoad());
        StartCoroutine(sound_manager.OnLoad());
        StartCoroutine(input_manager.OnLoad());
        StartCoroutine(scene.OnLoad());
    }

    private void Update()
    {
        UpdateEvent();
    }
}
