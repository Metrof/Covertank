using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float _attackZoneRadius = 5;

    public EnemyType Type = EnemyType.Solder;

    protected Vector3 _defaultPosition = Vector3.zero;
    protected Quaternion _defaultRotation = Quaternion.identity;

    protected CircleDrawer _circleDrawer;
    protected BaseStateMachine _baseStateMachine;
    protected EnemyUIBarPoolManager _enemyUIBarPoolManager;

    [HideInInspector] public IDamageTaker Target;

    public Collider Collider;
    public Rigidbody Rigidbody;
    public Weapon Weapon;
    public TankZoneTrigger TankTrigger;
    public EnemyHealth Health;
    public NavMeshAgent MeshAgent;
    public UIEnemyHealthBar HealthBar;

    private void Awake()
    {
        _circleDrawer = GetComponentInChildren<CircleDrawer>();
    }
    private void Start()
    {
        _circleDrawer.DrawCircle(_attackZoneRadius);
        TankTrigger.Init(_attackZoneRadius);

        _defaultPosition = transform.position;
        _defaultRotation = transform.rotation;
    }
    public virtual void Initialization(Context context, IDamageTaker target)
    {
        _enemyUIBarPoolManager = context.EnemyUIBarManager;

        TankTrigger.OnTankEnter += TankIsNear;
        TankTrigger.OnTankLeave += SearchState;
        TankTrigger.Init(_attackZoneRadius);
        Weapon.Init(context.BulletPoolManager);

        Health.Initialize(context.EventBus);
        Health.RestoreFullHP();
        Health.OnDead += Death;
        Health.OnHPChange += UpdateUIBar;

        Target = target;
    }
    public virtual void PlaceOnDefaultPosition()
    {
        Rigidbody.isKinematic = true;
        transform.SetPositionAndRotation(_defaultPosition, _defaultRotation);
        Collider.gameObject.transform.position = transform.position;
        Health.RestoreFullHP();
    }
    public virtual void CalmState()
    {
        TankTrigger.gameObject.SetActive(false);
    }
    public void TankIsNear()
    {
        _baseStateMachine.ChangeState(typeof(EnemyAttackState));
    }
    public virtual void SearchState()
    {
        TankTrigger.gameObject.SetActive(true);
        if (_enemyUIBarPoolManager != null && _enemyUIBarPoolManager.TryGetPoolItem(out var bar, transform.position, Quaternion.identity))
        {
            HealthBar = bar;
            HealthBar.SetFillAmount(1);
        }
    }
    protected void Death()
    {
        _baseStateMachine.ChangeState(typeof(EnemyDeadState));
    }
    protected void UpdateUIBar(float currentHP)
    {
        float fillAmount = currentHP / Health.MaxHealth;
        if (HealthBar != null)
        {
            HealthBar.SetFillAmount(fillAmount);
        }
    }
    public void PlaceOnMesh()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 1, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
        else
        {
            Debug.LogWarning("No valid NavMesh point found near " + transform.position);
        }
    }
}
public enum EnemyType
{
    Solder
}
