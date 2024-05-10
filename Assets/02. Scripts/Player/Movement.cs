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
    [SerializeField]
    private float _taserSpeedMultiplier = 0.5f;


    private Animator _animator;

    Vector2 _direction;
    private float _taserDuration;
    private float _lastTasered = 0f;
    private float _stopDuration;
    private float _lastStopped = 0f;

    public void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    // When you press WASD or move the left joystick, save the direction you are pointing to
    public void Move(InputAction.CallbackContext ctx)
    {
        if (GameTimer.instance.GameRunning)
        {
            if (ctx.started || ctx.performed)
            {
                _direction = ctx.ReadValue<Vector2>();
            }
            else if (ctx.canceled)
            {
                _direction = default;
            }
        } else
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
        if (Time.time - _lastStopped < _stopDuration)
        {
            return;
        }
        Vector3 v3Direction = new Vector3(_direction.x, 0f, _direction.y);
        if (Time.time - _lastTasered < _taserDuration) v3Direction = v3Direction * _taserSpeedMultiplier;
        _rigidBody.MovePosition(transform.position + v3Direction * _speed * Time.fixedDeltaTime);
        var lookat = transform.position + v3Direction;
        transform.LookAt(lookat);
    }

    // Each frame send data to the animator depending on the desired direction and the vertical velocity
    private void Update()
    {
        bool grounded = Physics.Raycast(transform.position, Vector3.down, 2f);

        if (_animator != null)
        {
            if (Time.time - _lastStopped < _stopDuration)
            {
                _animator.SetFloat("Speed", 0f);
            }
            else
            {
                _animator.SetFloat("Speed", _direction.magnitude);
            }
            _animator.SetFloat("SpeedY", _rigidBody.velocity.y);
            _animator.SetBool("Grounded", grounded);
        }
    }

    public void GetTasered(float time)
    {
        _lastTasered = Time.time;
        _taserDuration = time;
    }

    public void Stop(float time)
    {
        _lastStopped = Time.time;
        _stopDuration = time;
    }
}
