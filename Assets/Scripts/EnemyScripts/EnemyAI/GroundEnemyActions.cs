using UnityEngine;

public class GroundEnemyActions : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Wandering,
        ChasingPlayer,
        ChasingAfterShot,
        Attacking,
        Dead
    }

    [Header("Enemy Movement")]
    [SerializeField] private Rigidbody2D _enemyRigidBody;
    [SerializeField] private Animator _enemyAnimator;
    [SerializeField] private float _raycastDistance = 1f;
    [SerializeField] private LayerMask _obstacleLayer;

    [Header("Idle and Movement Timers")]
    [Range(1f, 10f)][SerializeField] private float idleTimeMin = 1f;
    [Range(1f, 10f)][SerializeField] private float idleTimeMax = 3f;
    [Range(1f, 10f)][SerializeField] private float moveTimeMin = 2f;
    [Range(1f, 10f)][SerializeField] private float moveTimeMax = 5f;

    [Header("Player Detection")]
    [SerializeField] private float _detectionRadius = 3f;
    [SerializeField] private float _enemyAttackRate = 3f;
    [SerializeField] private float _enemyChaseTime = 5f;
    [SerializeField] private float _attackRange = 0.1f;

    // Components and References
    private Transform _playerTransform;
    private PlayerStats _playerStats;
    private EnemyStats _enemyStats;
    private EnemyCollision _enemyCollision;

    // State and Movement Variables
    private EnemyState _currentState;
    private Vector2 _movementDirection;
    private float _changeStateTimer = 0f;
    private float _currentStateDuration = 0f;
    private float _nextEnemyChaseTime = 0f;
    private float _enemyMoveSpeed;
    private float _enemyDamage;
    private bool _isDead;
    private bool _isEnemyShot;
    private bool _isChasingPlayer;

    // Attack cooldown
    private float _attackCooldown = 1.5f;
    private float _attackTimer = 0f;

    void Start()
    {
        InitializeComponents();
        _currentState = EnemyState.Idle;
    }

    void Update()
    {
        if (_isDead)
        {
            HandleDeadState();
        }
        else
        {
            UpdateStatusFlags();
            HandleStateTransitions();
            UpdateAnimation();
        }
    }

    void FixedUpdate()
    {
        if (_isDead)
        {
            StopEnemyMovement();
        }
        else
        {
            HandleMovement();
        }
    }

    // Initialization methods
    private void InitializeComponents()
    {
        // Find the player by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerStats = player.GetComponent<PlayerStats>();
            _playerTransform = player.transform;

            if (_playerStats == null)
            {
                Debug.LogError("PlayerStats component not found on Player!");
            }
        }
        else
        {
            Debug.LogError("Player object not found!");
        }

        // Get the EnemyStats component attached to this enemy
        _enemyStats = GetComponent<EnemyStats>();
        if (_enemyStats != null)
        {
            _enemyDamage = _enemyStats.GetEnemyDamage();
            _isDead = _enemyStats.GetIsDead();
        }
        // Get the EnemyCollision component attached to this enemy
        _enemyCollision = GetComponent<EnemyCollision>();
        _enemyMoveSpeed = _enemyStats.GetEnemySpeed();  
    }

    // Update methods
    private void UpdateStatusFlags()
    {
        _isDead = _enemyStats.GetIsDead();
        _isEnemyShot = _enemyCollision.GetEnemyShot();
    }

    private void HandleStateTransitions()
    {
        switch (_currentState)
        {
            case EnemyState.Idle:
            case EnemyState.Wandering:
                HandleIdleOrWanderingState();
                break;

            case EnemyState.ChasingPlayer:
                HandleChasingPlayerState();
                break;

            case EnemyState.ChasingAfterShot:
                HandleChasingAfterShotState();
                break;

            case EnemyState.Attacking:
                HandleAttackingState();
                break;
        }
    }

    private void HandleIdleOrWanderingState()
    {
        UpdateStateTimer();
        DetectPlayer();

        if (_isChasingPlayer)
        {
            TransitionToState(EnemyState.ChasingPlayer);
        }
        else if (_isEnemyShot)
        {
            StartChasingAfterShot();
        }
        else if (_changeStateTimer >= _currentStateDuration)
        {
            ChangeIdleOrWanderingState();
        }
    }

    private void HandleChasingPlayerState()
    {
        DetectPlayer();

        if (!_isChasingPlayer)
        {
            TransitionToState(EnemyState.Idle);
        }
        else if (IsPlayerWithinAttackRange())
        {
            StartAttacking();
        }
    }

    private void HandleChasingAfterShotState()
    {
        if (Time.time >= _nextEnemyChaseTime)
        {
            StopChasingAfterShot();
        }
        else if (IsPlayerWithinAttackRange())
        {
            StartAttacking();
        }
    }

    private void HandleAttackingState()
    {
        if (!IsPlayerWithinAttackRange())
        {
            StopAttacking();
            TransitionToState(EnemyState.ChasingPlayer);
        }
        else
        {
            PerformAttack();
        }
    }

    private void HandleDeadState()
    {
        TransitionToState(EnemyState.Dead);
    }

    // FixedUpdate methods
    private void HandleMovement()
    {
        switch (_currentState)
        {
            case EnemyState.Idle:
                StopEnemyMovement();
                break;

            case EnemyState.Wandering:
                AutoMove();
                break;

            case EnemyState.ChasingPlayer:
            case EnemyState.ChasingAfterShot:
                MoveTowardsPlayer();
                break;

            case EnemyState.Attacking:
                StopEnemyMovement();
                break;
        }
    }

    private void StopEnemyMovement()
    {
        _enemyRigidBody.velocity = Vector2.zero;
    }

    // State transition methods
    private void TransitionToState(EnemyState newState)
    {
        _currentState = newState;
        _changeStateTimer = 0f;
    }

    private void ChangeIdleOrWanderingState()
    {
        if (_currentState == EnemyState.Idle)
        {
            TransitionToState(EnemyState.Wandering);
            ChooseRandomDirection();
            _currentStateDuration = Random.Range(moveTimeMin, moveTimeMax);
        }
        else if (_currentState == EnemyState.Wandering)
        {
            TransitionToState(EnemyState.Idle);
            _currentStateDuration = Random.Range(idleTimeMin, idleTimeMax);
        }
    }

    private void StartChasingAfterShot()
    {
        TransitionToState(EnemyState.ChasingAfterShot);
        _nextEnemyChaseTime = Time.time + _enemyChaseTime;
    }

    private void StopChasingAfterShot()
    {
        _enemyCollision.SetEnemyShot(false);
        _isEnemyShot = false;
        TransitionToState(EnemyState.Idle);
    }

    private void StartAttacking()
    {
        TransitionToState(EnemyState.Attacking);
        _enemyAnimator.SetBool("isZombieAttack", true);
        _attackTimer = 0f;
    }

    private void StopAttacking()
    {
        _enemyAnimator.SetBool("isZombieAttack", false);
    }

    // Movement methods
    private void AutoMove()
    {
        if (IsObstacleInPath())
        {
            ChooseRandomDirection();
        }
        else
        {
            _enemyRigidBody.velocity = _movementDirection * _enemyMoveSpeed;
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 directionToPlayer = GetDirectionToPlayer();
        _enemyRigidBody.velocity = directionToPlayer * _enemyMoveSpeed;

        UpdateMovementAnimation(directionToPlayer);
    }

    private Vector2 GetDirectionToPlayer()
    {
        return (_playerTransform.position - transform.position).normalized;
    }

    private void ChooseRandomDirection()
    {
        int randomDirection = Random.Range(0, 8);

        switch (randomDirection)
        {
            case 0: _movementDirection = Vector2.up; break;
            case 1: _movementDirection = Vector2.down; break;
            case 2: _movementDirection = Vector2.left; break;
            case 3: _movementDirection = Vector2.right; break;
            case 4: _movementDirection = new Vector2(1, 1).normalized; break;   // Diagonal up-right
            case 5: _movementDirection = new Vector2(-1, 1).normalized; break;  // Diagonal up-left
            case 6: _movementDirection = new Vector2(1, -1).normalized; break;  // Diagonal down-right
            case 7: _movementDirection = new Vector2(-1, -1).normalized; break; // Diagonal down-left
        }
    }

    private bool IsObstacleInPath()
    {
        RaycastHit2D hit = Physics2D.Raycast(_enemyRigidBody.position, _movementDirection, _raycastDistance, _obstacleLayer);
        Debug.DrawRay(_enemyRigidBody.position, _movementDirection * _raycastDistance, Color.red);
        return hit.collider != null;
    }

    // Detection methods
    private void DetectPlayer()
    {
        float distanceToPlayer = GetDistanceToPlayer();

        if (distanceToPlayer <= _detectionRadius)
        {
            _isChasingPlayer = true;
        }
        else
        {
            _isChasingPlayer = false;

            if (_currentState == EnemyState.ChasingPlayer || _currentState == EnemyState.Attacking)
            {
                StopAttacking();
                TransitionToState(EnemyState.Idle);
            }
        }
    }

    private bool IsPlayerWithinAttackRange()
    {
        return GetDistanceToPlayer() <= _attackRange;
    }

    private float GetDistanceToPlayer()
    {
        return Vector2.Distance(transform.position, _playerTransform.position);
    }

    // Attack methods
    private void PerformAttack()
    {
        if (_attackTimer > 0f)
        {
            _attackTimer -= Time.deltaTime;
        }
        else
        {
            ApplyDamageToPlayer();
            _attackTimer = _attackCooldown;
        }
    }

    private void ApplyDamageToPlayer()
    {
        _playerStats.TakeDamage(_enemyDamage);
        Debug.Log("Player got hit! Damage applied: " + _enemyDamage);
        Debug.Log("Current player health: " + _playerStats.GetPlayerHealth());
    }

    // Animation methods
    private void UpdateAnimation()
    {
        if (_currentState == EnemyState.Attacking)
        {
            HandleAttackAnimation();
        }
        else
        {
            UpdateMovementAnimation(_enemyRigidBody.velocity);
        }
    }

    private void HandleAttackAnimation()
    {
        AnimatorStateInfo currentStateInfo = _enemyAnimator.GetCurrentAnimatorStateInfo(0);

        if (currentStateInfo.IsName("ZombieAttack") && currentStateInfo.normalizedTime >= 1.0f)
        {
            _enemyAnimator.SetBool("isZombieAttack", false);
        }
    }

    private void UpdateMovementAnimation(Vector2 movement)
    {
        if (movement.sqrMagnitude > 0.1f)
        {
            _enemyAnimator.SetFloat("E_Horizontal", movement.x);
            _enemyAnimator.SetFloat("E_Vertical", movement.y);
            _enemyAnimator.SetFloat("E_Speed", movement.magnitude);
        }
        else
        {
            _enemyAnimator.SetFloat("E_Speed", 0);
        }
    }

    // Timer methods
    private void UpdateStateTimer()
    {
        _changeStateTimer += Time.deltaTime;
    }
}
