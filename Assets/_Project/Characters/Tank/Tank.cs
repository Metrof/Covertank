using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Tank : MonoBehaviour
{
    public UnityAction OnTankDestroy;

    [SerializeField] private float _zoneRadius = 5;

    [SerializeField] private float _damageIfPlayerFarPerSec = 0.1f;
    [SerializeField] private float _timeBetweenDamage = 0.5f;

    public CinemachineVirtualCamera VirtualCamera;

    private CircleDrawer _circleDrawer;
    private Quaternion _defaultRotation = Quaternion.identity;
    private Quaternion _defaultWeaponRotation = Quaternion.identity;

    private BaseStateMachine _baseStateMachine;

    public Weapon Weapon;
    public PeriodicDamage PeriodicDamage;
    public PlayerZoneTrigger PlayerTrigger;
    public EnemyZoneTrigger EnemyTrigger;
    public EventBus EventBus;
    public NavMeshAgent MeshAgent;
    public TankHealth Health;

    [HideInInspector] public int NextMovePoint;
    [HideInInspector] public List<IDamageTaker> NearestsEnemy = new List<IDamageTaker>();
    [HideInInspector] public List<Vector3> Path;

    private void Awake()
    {
        _circleDrawer = GetComponentInChildren<CircleDrawer>();
    }
    private void Start()
    {
        VirtualCamera.transform.parent = null;
        _circleDrawer.DrawCircle(_zoneRadius);
        _defaultRotation = transform.rotation;
        _defaultWeaponRotation = Weapon.transform.rotation;
    }
    private void OnDestroy()
    {
        OnTankDestroy?.Invoke();
    }
    public void Initialization(Context context)
    {
        PeriodicDamage = new PeriodicDamage(_timeBetweenDamage, _damageIfPlayerFarPerSec);
        PeriodicDamage.OnDamageTake += Health.TakeDamage;
        PlayerTrigger.OnPlayerEnter += () => PeriodicDamage.ChangeState(true);
        PlayerTrigger.OnPlayerLeave += () => PeriodicDamage.ChangeState(false);
        PlayerTrigger.Init(_zoneRadius);

        EnemyTrigger.OnEnemyEnter += AddEnemyToList;
        EnemyTrigger.OnEnemyLeave += RemoveEnemyFromList;
        EnemyTrigger.Init(_zoneRadius);

        EventBus = context.EventBus;
        Health.Initialize(EventBus);
        Health.OnDead += Death;

        Weapon.Init(context.BulletPoolManager);

        _baseStateMachine = new BaseStateMachine();
        _baseStateMachine.AddState(new TankAttackState(this, _baseStateMachine));
        _baseStateMachine.AddState(new TankDeathState(this, _baseStateMachine));
        _baseStateMachine.AddState(new TankMoveState(this, _baseStateMachine));
        _baseStateMachine.AddState(new TankOnBaseState(this, _baseStateMachine));
        _baseStateMachine.ChangeState(typeof(TankOnBaseState));
    }
    public void SetPath(List<Vector3> path)
    {
        Path = path;
    }
    public void StartMove()
    {
        _baseStateMachine.ChangeState(typeof(TankMoveState));
    }
    public void ComeOnTheBase()
    {
        _baseStateMachine.ChangeState(typeof(TankOnBaseState));
        transform.rotation = _defaultRotation;
        Weapon.transform.rotation = _defaultWeaponRotation;
    }
    private void Death()
    {
        _baseStateMachine.ChangeState(typeof(TankDeathState));
    }
    private void AddEnemyToList(IDamageTaker taker)
    {
        NearestsEnemy.Add(taker);
        if (NearestsEnemy.Count == 1)
        {
            _baseStateMachine.ChangeState(typeof(TankAttackState));
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
            _baseStateMachine.ChangeState(typeof(TankMoveState));
        }
    }
}
