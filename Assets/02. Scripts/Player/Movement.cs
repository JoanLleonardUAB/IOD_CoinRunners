using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The physics component, which moves the player")]
    private Rigidbody _rigidBody = default;

    [SerializeField]
    [Tooltip("The speed (in m/s) at which the players will move")]
    private float _speed = 1f;

    private Animator _animator;
    private PlayerStatusEffects _statusEffects;

    Vector2 _direction;

    public void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _statusEffects = GetComponent<PlayerStatusEffects>();
        if (_statusEffects == null)
            Debug.LogWarning("PlayerStatusEffects component missing!");
    }

    // When you press WASD or move the left joystick, save the direction you are pointing to
    public void Move(InputAction.CallbackContext ctx)
    {
        if (GameTimer.instance.GameRunning)
        {
            if (ctx.started || ctx.performed)
            {
                Vector2 input = ctx.ReadValue<Vector2>();
                if (_statusEffects != null && _statusEffects.ControlsInverted)
                    input = -input;
                _direction = input;
            }
            else if (ctx.canceled)
            {
                _direction = default;
            }
        }
        else
        {
            _direction = default;
        }
    }

    private void OnEnable()
    {
        GameTimer.OnTimerEnded += OnTimerEnded;
    }

    private void OnDisable()
    {
        GameTimer.OnTimerEnded -= OnTimerEnded;
    }

    private void OnTimerEnded()
    {
        _direction = default;
        var lookat = transform.position + Vector3.back;
        transform.LookAt(lookat);
    }

    // Each physics frame, move a little on the desired direction.
    // Then rotate to the direction
    private void FixedUpdate()
    {
        Vector3 v3Direction = new Vector3(_direction.x, 0f, _direction.y);
        float speed = _statusEffects != null ? _statusEffects.CurrentSpeed : _speed;
        _rigidBody.MovePosition(transform.position + v3Direction * speed * Time.fixedDeltaTime);
        var lookat = transform.position + v3Direction;
        transform.LookAt(lookat);
    }

    // Each frame send data to the animator depending on the desired direction and the vertical velocity
    private void Update()
    {
        bool grounded = Physics.Raycast(transform.position, Vector3.down, 2f);

        if (_animator != null)
        {
            _animator.SetFloat("Speed", _direction.magnitude);
            _animator.SetFloat("SpeedY", _rigidBody.velocity.y);
            _animator.SetBool("Grounded", grounded);
        }
    }
}
