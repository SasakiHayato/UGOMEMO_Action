using UnityEngine;

/// <summary>
/// クリックの対象にするためのインターフェース
/// </summary>
public interface IInputRayCastAddress
{
    int ID { get; set; }
    Define.ObjectType ObjectType { get; }

    ICharacterBehaviour Character{ get; }
    Vector2 Position { get; }
}

/// <summary>
/// 入力管理クラス
/// </summary>
public class InputManager : SingletonBehaviour<InputManager>, Define.Input.ICallback
{
    //================================================
    // public interface
    //================================================

    /// <summary>
    /// 各操作に対するコールバックインターフェースに仕込むためのインターフェース.
    /// Info. 使用者が気にすることはないので無視してください.
    /// </summary>
    public interface IInputReceiver { }

    //================================================
    // private class
    //================================================

    /// <summary>
    /// 入力の操作クラス
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
        /// 初期化
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
    /// アドレスの発行.
    /// Info. IInputRayCastAddressインターフェースを継承したクラスは初期化時にこのメソッドを呼んでください。
    /// </summary>
    /// <param name="info"></param>
    public void AllocateAddress(IInputRayCastAddress info)
    {
        info.ID = _address;
        _address++;
    }

    /// <summary>
    /// 操作する対象のセット
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
    /// 初期化
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
    /// シーン破棄時の処理
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
    /// クリックした際の処理
    /// </summary>
    /// <param name="address"></param>
    /// <param name="click_position"></param>
    void Define.Input.ICallback.OnClick(IInputRayCastAddress address, Vector2 click_position)
    {
        _onClickCallback?.OnClick(click_position);

        if (address != null) _isHitCallback?.OnHit(address, click_position);
    }

    /// <summary>
    /// クリックしている際の処理
    /// </summary>
    /// <param name="address"></param>
    /// <param name="current_position"></param>
    void Define.Input.ICallback.OnStay(IInputRayCastAddress address, Vector2 current_position)
    {
        _stayCallbck?.OnStay(current_position);
    }

    /// <summary>
    /// クリックし終わった際の処理
    /// </summary>
    /// <param name="address"></param>
    /// <param name="release_position"></param>
    void Define.Input.ICallback.OnRelease(IInputRayCastAddress address, Vector2 release_position)
    {
        _releaseCallback?.OnRelease(release_position);
    }

    /// <summary>
    /// フリックした際の処理
    /// </summary>
    /// <param name="address"></param>
    /// <param name="flick_derection"></param>
    /// <param name="distance"></param>
    void Define.Input.ICallback.OnFlicked(IInputRayCastAddress address, Vector2 flick_derection, float distance)
    {
        _flickedCallback?.OnFlicked(flick_derection, distance);
    }
}
