using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �V�[���̗����グ�A�j�����ɃC�x���g��ǉ����邽�߂̃C���^�[�t�F�[�X
/// </summary>
public interface ILoader
{
    /// <summary>
    /// �V�[�������グ���̏���
    /// </summary>
    void OnLoad();

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
    // variable
    //=================================================

    private List<ILoader> _loadList;

    //=================================================
    // protected method
    //=================================================

    /// <summary>
    /// �V�[�������グ���̏���.
    /// UnityEngine.Start()
    /// </summary>
    protected abstract void Setup();

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

    void ILoader.OnLoad()
    {
        Setup();
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
        _loadList = new List<ILoader>();

        new GameLocalData();
        _loadList.Add(new InputManager());
        _loadList.Add(this);
    }

    private void Start()
    {
        _loadList.ForEach(l => l.OnLoad());
    }

    private void Update()
    {
        UpdateEvent();
    }

    private void OnDestroy()
    {
        _loadList.ForEach(l => l.UnLoad());
    }
}
