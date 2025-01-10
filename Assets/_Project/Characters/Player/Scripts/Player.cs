using Cinemachine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _zoneRadius = 5;
    [SerializeField] private float _moveSpeed = 1;
    [SerializeField] private float _rotateSpeed = 1;
    [SerializeField] private EnemyZoneTrigger _enemyTrigger;

    private CharacterController _controller;
    private Animator _animator;
    private CircleDrawer _circleDrawer;

    private Controller inputActions;
    private CancellationTokenSource _cancellationTokenSource;

    private Vector2 _moveVector = Vector2.one;

    private CinemachineVirtualCamera _virtualCamera;

    private BaseStateMachine _baseStateMachine;

    public List<IDamageTaker> NearestsEnemy = new List<IDamageTaker>();
    public Weapon Weapon;
    [HideInInspector] public bool IsAttack = false;
    [HideInInspector] public Transform NearestEnemyPosition;

    public int PlayerCameraPriority { get { return _virtualCamera.Priority; } }

    private void Awake()
    {
        _controller = transform.GetComponent<CharacterController>();
        _animator = transform.GetComponentInChildren<Animator>();
        _virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        _circleDrawer = GetComponentInChildren<CircleDrawer>();

        inputActions = new Controller();
        inputActions.Player.Move.performed += PlayerStartMove;
        inputActions.Player.Move.canceled += PlayerStopMove;
    }
    private void Start()
    {
        _circleDrawer.DrawCircle(_zoneRadius);
    }
    private void OnDisable()
    {
        InputDisable();
        CheckUniTaskOp();
    }
    private void OnDestroy()
    {
        InputDisable();
        CheckUniTaskOp();
    }
    public void Initialization(Context context)
    {
        _enemyTrigger.OnEnemyEnter += AddEnemyToList;
        _enemyTrigger.OnEnemyLeave += RemoveEnemyFromList;
        _enemyTrigger.Init(_zoneRadius);

        Weapon.Init(context.BulletPoolManager);

        _baseStateMachine = new BaseStateMachine();
        _baseStateMachine.AddState(new PlayerAttackStates(this, _baseStateMachine));
        _baseStateMachine.AddState(new PlayerCalmStates(this, _baseStateMachine));
        _baseStateMachine.ChangeState(typeof(PlayerCalmStates));
    }
    public void InputEnable()
    {
        inputActions.Enable();
    }
    public void InputDisable()
    {
        inputActions.Disable();
        _animator.SetBool("run", false);
    }
    public void CalmStateEnable()
    {
        _baseStateMachine.ChangeState(typeof(PlayerCalmStates));
        NearestsEnemy.Clear();
    }
    private void PlayerStartMove(InputAction.CallbackContext context)
    {
        _moveVector = context.ReadValue<Vector2>();
        if (_cancellationTokenSource == null)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            PlayerMoveTask(_cancellationTokenSource.Token).Forget();
        }
    }
    private void PlayerStopMove(InputAction.CallbackContext context)
    {
        if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = null;
            _animator.SetBool("run", false);
        }
    }
    private void Update()
    {
        if (IsAttack)
        {
            Vector3 dir = NearestEnemyPosition.position - transform.position;
            dir.y = 0;
            dir.Normalize();
            Vector3 enemyDirection = new Vector3(transform.position.x + dir.x, transform.position.y, transform.position.z + dir.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(enemyDirection - transform.position), _rotateSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 currentMoveToPos = new Vector3(transform.position.x + _moveVector.x, transform.position.y, transform.position.z + _moveVector.y);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(currentMoveToPos - transform.position), _rotateSpeed * Time.deltaTime);
        }
    }
    private async UniTaskVoid PlayerMoveTask(CancellationToken token)
    {
        _animator.SetBool("run", true);

        while (!token.IsCancellationRequested)
        {
            _controller.Move(new Vector3(_moveVector.x, 0, _moveVector.y) * _moveSpeed * Time.deltaTime);
            await UniTask.Yield();
        }

        _animator.SetBool("run", false);
    }
    private void CheckUniTaskOp()
    {
        if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = null;
        }
    }

    private void AddEnemyToList(IDamageTaker taker)
    {
        NearestsEnemy.Add(taker);
        if (NearestsEnemy.Count == 1)
        {
            _baseStateMachine.ChangeState(typeof(PlayerAttackStates));
        }
    }
    public void RemoveEnemyFromList(IDamageTaker taker)
    {
        if (NearestsEnemy.Contains(taker))
        {
            NearestsEnemy.Remove(taker);
        }
        if (NearestsEnemy.Count == 0)
        {
            _baseStateMachine.ChangeState(typeof(PlayerCalmStates));
        }
    }
}
