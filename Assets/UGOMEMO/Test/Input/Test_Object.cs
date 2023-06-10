using UnityEngine;

public class Test_Object : MonoBehaviour, IInputRayCastAddress
{
    [SerializeField] ObjectType _objectType;
    [SerializeField] float _destroyInterval;

    float _timer;

    private void Start()
    {
        InputManager.Instance.AllocateAddress(this);
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > _destroyInterval)
        {
            Destroy(gameObject);
        }
    }

    int IInputRayCastAddress.ID { get; set; }

    ObjectType IInputRayCastAddress.ObjectType => _objectType;

    Vector2 IInputRayCastAddress.Position => transform.position;

    ICharacterBehaviour IInputRayCastAddress.Character => null;
}
