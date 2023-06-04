using UnityEngine;

/// <summary>
/// �N���b�N�̑Ώۂɂ��邽�߂̃C���^�[�t�F�[�X
/// </summary>
public interface IInputRayCastAddress
{
    int ID { get; set; }
    Define.ObjectType ObjectType { get; }

    ICharacterBehaviour Character{ get; }
    Vector2 Position { get; }
}

/// <summary>
/// ���͊Ǘ��N���X
/// </summary>
public class InputManager : SingletonBehaviour<InputManager>, Define.Input.ICallback
{
    //================================================
    // public interface
    //================================================

    /// <summary>
    /// �e����ɑ΂���R�[���o�b�N�C���^�[�t�F�[�X�Ɏd���ނ��߂̃C���^�[�t�F�[�X.
    /// Info. �g�p�҂��C�ɂ��邱�Ƃ͂Ȃ��̂Ŗ������Ă�������.
    /// </summary>
    public interface IInputReceiver { }

    //================================================
    // private class
    //================================================

    /// <summary>
    /// ���͂̑���N���X
    /// </summary>
    private class Controller : MonoBehaviour
    {
        //================================================
        // variable
        //================================================

        private float _timer;
        private Vector3 _clickPosition;

        private Define.Input.ICallback _callback;
        private IInputRayCastAddress _currentAddress;

        //================================================
        // public method
        //================================================

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="callback"></param>
        public void Initialize(Define.Input.ICallback callback)
        {
            _callback = callback;
        }

        //================================================
        // unity method
        //================================================

        private void Update()
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                _timer = 0;
                _clickPosition = transform.position;

                _callback.OnClick(_currentAddress, transform.position);
            }

            if (Input.GetMouseButton(0))
            {
                _timer += Time.deltaTime;
                _callback.OnStay(_currentAddress, transform.position);
            }

            if (Input.GetMouseButtonUp(0))
            {
                Vector2 direction = transform.position - _clickPosition;
                _callback.OnRelease(_currentAddress, transform.position);

                if (_timer < Define.Input.FLICK_ATTRIBUTE_TIME && direction.magnitude > Define.Input.FLICK_ATTRIBUTE_DISTANCE)
                    _callback.OnFlicked(_currentAddress, direction.normalized, direction.magnitude);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IInputRayCastAddress address)) 
                _currentAddress = address;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_currentAddress == null) return;

            if (collision.gameObject.TryGetComponent(out IInputRayCastAddress address))
                if (_currentAddress.ID == address.ID) _currentAddress = null;
        }
    }

    //================================================
    // variable
    //================================================

    private int _address;
    
    private IInputOnClickCallback _onClickCallback;
    private IInputIsHitCallback _isHitCallback;
    private IInputStayCallback _stayCallbck;
    private IInputReleaseCallback _releaseCallback;
    private IInputFlickedCallback _flickedCallback;

    //================================================
    // public method
    //================================================

    /// <summary>
    /// �A�h���X�̔��s.
    /// Info. IInputRayCastAddress�C���^�[�t�F�[�X���p�������N���X�͏��������ɂ��̃��\�b�h���Ă�ł��������B
    /// </summary>
    /// <param name="info"></param>
    public void AllocateAddress(IInputRayCastAddress info)
    {
        info.ID = _address;
        _address++;
    }

    /// <summary>
    /// ���삷��Ώۂ̃Z�b�g
    /// </summary>
    /// <param name="callback"></param>
    public void SetOperator(IInputReceiver callback)
    {
        _onClickCallback = callback as IInputOnClickCallback;
        _isHitCallback = callback as IInputIsHitCallback;
        _stayCallbck = callback as IInputStayCallback;
        _releaseCallback = callback as IInputReleaseCallback;
        _flickedCallback = callback as IInputFlickedCallback;
    }

    //================================================
    // protected method
    //================================================

    /// <summary>
    /// ������
    /// </summary>
    protected override void Initialize()
    {
        GameObject trigger_obj = new GameObject("Input_Trigger");

        var rb = trigger_obj.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        var collider = trigger_obj.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = Define.Input.RADIUS;

        var controller = trigger_obj.AddComponent<Controller>();
        controller.Initialize(this);

        Object.DontDestroyOnLoad(trigger_obj);
    }

    /// <summary>
    /// �V�[���j�����̏���
    /// </summary>
    protected override void UnLoad()
    {
        _address = 0;
        SetOperator(null);
    }

    //================================================
    // Define.Input.ICallback interface
    //================================================

    /// <summary>
    /// �N���b�N�����ۂ̏���
    /// </summary>
    /// <param name="address"></param>
    /// <param name="click_position"></param>
    void Define.Input.ICallback.OnClick(IInputRayCastAddress address, Vector2 click_position)
    {
        _onClickCallback?.OnClick(click_position);

        if (address != null) _isHitCallback?.OnHit(address, click_position);
    }

    /// <summary>
    /// �N���b�N���Ă���ۂ̏���
    /// </summary>
    /// <param name="address"></param>
    /// <param name="current_position"></param>
    void Define.Input.ICallback.OnStay(IInputRayCastAddress address, Vector2 current_position)
    {
        _stayCallbck?.OnStay(current_position);
    }

    /// <summary>
    /// �N���b�N���I������ۂ̏���
    /// </summary>
    /// <param name="address"></param>
    /// <param name="release_position"></param>
    void Define.Input.ICallback.OnRelease(IInputRayCastAddress address, Vector2 release_position)
    {
        _releaseCallback?.OnRelease(release_position);
    }

    /// <summary>
    /// �t���b�N�����ۂ̏���
    /// </summary>
    /// <param name="address"></param>
    /// <param name="flick_derection"></param>
    /// <param name="distance"></param>
    void Define.Input.ICallback.OnFlicked(IInputRayCastAddress address, Vector2 flick_derection, float distance)
    {
        _flickedCallback?.OnFlicked(flick_derection, distance);
    }
}
