using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] Rigidbody2D _playerRigidBody;  // Reference to the Rigidbody2D component for applying physics-based movement
    [SerializeField] Animator _playerAnimator;      // Reference to the Animator component to handle player animations
    [SerializeField] Texture2D _crosshairTexture;
    [SerializeField] GameObject[] _bulletPrefab;
    [SerializeField] Transform _gun;
    [SerializeField] GameObject _player;
    [SerializeField] float _meleeDamage = 25f;      // Damage dealt by melee attack
    [SerializeField] LayerMask _enemyLayer;

    // New settings for additional raycasts
    [SerializeField] float _raycastDistanceLeft = 1.5f;  // Distance for the left raycast
    [SerializeField] float _raycastDistanceRight = 1.5f; // Distance for the right raycast
    [SerializeField] float _raycastDistanceMiddle = 1.5f; // Default range of melee attack for the main raycast
    [SerializeField] float _raycastAngleOffset = 30f;    // Angle offset for the side raycasts

    GameObject _bullet;
    WeaponStat _weaponStat;
    PlayerStats _playerStat;
    Vector2 _moveInput;          // Stores the input for player movement (horizontal and vertical axis)
    float _nextFireTime = 0f; // Tracks when the gun can shoot next
    float _lastHorizontal = 0f;  // Stores the last non-zero horizontal input value (for direction when idle)
    float _lastVertical = -1f;   // Stores the last non-zero vertical input value (default facing down)
    float _bulletSpeed;
    float _fireRate;
    float _gunDamage;
    float _playerMoveSpeed;
    bool _hasDealtMeleeDamage = false;
    bool _isDead;
    Scene currentScene;
    AudioSource[] gunshots;

    [SerializeField] bool _isPistol = true;
    bool _isPistolAcquired = true;

    [SerializeField] bool _isRocket = false;
    bool _isRocketAcquired = false;

    [SerializeField] bool _isRifle = false;
    bool _isRifleAcquired = false;

    [SerializeField] bool _isShotgun = false;
    bool _isShotgunAcquired = false;

    void Start()
    {
        _weaponStat = _gun.GetComponent<WeaponStat>();
        _playerStat = _player.GetComponent<PlayerStats>();
        _bulletSpeed = _weaponStat.GetAmmoSpeed();
        _fireRate = _weaponStat.GetWeaponFireRate();
        _gunDamage = _weaponStat.GetWeaponDamage();
        _playerMoveSpeed = _playerStat.GetPlayerMoveSpeed();
        _isDead = _playerStat.GetIsDead();
        // Change the cursor to the crosshair and hide the default system cursor
        Cursor.SetCursor(_crosshairTexture, Vector2.zero, CursorMode.Auto);
        currentScene = SceneManager.GetActiveScene();
        gunshots = GetComponents<AudioSource>();
    }

    void Update()
    {
        if (!_isDead)
        {
            // Check if the left mouse button is held down and the rifle is equipped
            if (_isRifle && _isRifleAcquired && Input.GetMouseButton(0))
            {
                FireRifle();
            }
            UpdatePlayerAnimationStat();
            UpdateStats();
            DetectEnemyWithRaycasts(); // Main raycast logic for melee attacks
        }
        _isDead = _playerStat.GetIsDead();
        // When player dies call respawnOnDeath after waiting 3 sec (for anim to play)
        if (_isDead)
        {
            Invoke("respawnOnDeath", 3);
        }
    }

    public void SetShotgunAqquire(bool isShotgunAcquired)
    {
        _isShotgunAcquired = isShotgunAcquired;
    }

    public void SetRifleAqquire(bool isRifleAcquired)
    {
        _isRifleAcquired = isRifleAcquired;
    }

    public void SetRocketAqquire(bool isRocketAcquired)
    {
        _isRocketAcquired = isRocketAcquired;
    }

    /**
    * Keep updating player's speed, direction for movement and idle
    */
    public void UpdatePlayerAnimationStat()
    {
        if (_playerAnimator != null)
        {
            if (_moveInput.sqrMagnitude > 0)
            {
                _lastHorizontal = _moveInput.x;
                _lastVertical = _moveInput.y;

                _playerAnimator.SetFloat("Horizontal", _lastHorizontal);
                _playerAnimator.SetFloat("Vertical", _lastVertical);
            }
            else
            {
                _playerAnimator.SetFloat("Horizontal", _lastHorizontal);
                _playerAnimator.SetFloat("Vertical", _lastVertical);
            }

            _playerAnimator.SetFloat("Speed", _moveInput.sqrMagnitude);
        }

        if (!_playerAnimator.IsInTransition(0))
        {
            AnimatorStateInfo currentState = _playerAnimator.GetCurrentAnimatorStateInfo(0);

            if (currentState.IsName("StabAnimationBlendTree") && currentState.normalizedTime >= 1.0f)
            {
                _playerAnimator.SetBool("isStabbing", false);
            }
            ResetShootingAnimation(currentState);
        }
    }

    void UpdateStats()
    {
        _bulletSpeed = _weaponStat.GetAmmoSpeed();
        _fireRate = _weaponStat.GetWeaponFireRate();
        _gunDamage = _weaponStat.GetWeaponDamage();
        _playerMoveSpeed = _playerStat.GetPlayerMoveSpeed();
    }

    /** 
    * Called on a regular interval by Unity for physics calculations.
    * Handles physics-based movement logic by applying velocity to the Rigidbody2D.
    */
    void FixedUpdate()
    {
        if(!_isDead)
        {
            Run();
        }
    }

    /**
    * Applies the player's movement input to the Rigidbody2D's velocity.
    * This results in the player moving in the direction of the input with a speed based on _playerMoveSpeed.
    */
    private void Run()
    {
        // Normalize the input to ensure diagonal movement isn't faster
        Vector2 playerVelocity = _moveInput.normalized * _playerMoveSpeed;

        // Apply the calculated velocity to the Rigidbody2D to move the player
        _playerRigidBody.velocity = playerVelocity;
    }

    /**
    * Called by the Unity Input System when movement input is received.
    * Stores the input value as a Vector2, which represents horizontal and vertical movement.
    * @param inputValue - The movement input provided by the Input System.
    */
    private void OnMove(InputValue inputValue)
    {
        // Capture and store the movement input as a Vector2 (x and y axes)
        _moveInput = inputValue.Get<Vector2>();
    }

    /**
    * Called by the Unity Input System when the stab input (F key) is pressed.
    * Triggers the stab animation once.
    * @param inputValue - The stab input provided by the Input System.
    */
    private void OnStab(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            _playerAnimator.SetBool("isStabbing", true);
            _hasDealtMeleeDamage = true;
        }
    }

    /**
    * Detect enemies using three raycasts (center, left, and right).
    * The raycasts are adjusted by angle and distance.
    */
    private void DetectEnemyWithRaycasts()
    {
        // Calculate the forward direction based on player facing
        Vector2 forwardDirection = new Vector2(_lastHorizontal, _lastVertical).normalized;

        if (forwardDirection == Vector2.zero)
            return; // If no movement, skip raycast

        // Cast the central ray
        RaycastHit2D centerHit = Physics2D.Raycast(transform.position, forwardDirection, _raycastDistanceMiddle, _enemyLayer);
        Debug.DrawRay(transform.position, forwardDirection * _raycastDistanceMiddle, Color.cyan);

        // Calculate left and right directions with the angle offset
        Vector2 leftDirection = Quaternion.Euler(0, 0, _raycastAngleOffset) * forwardDirection;
        Vector2 rightDirection = Quaternion.Euler(0, 0, -_raycastAngleOffset) * forwardDirection;

        // Cast the left and right rays
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, leftDirection, _raycastDistanceLeft, _enemyLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, rightDirection, _raycastDistanceRight, _enemyLayer);

        Debug.DrawRay(transform.position, leftDirection * _raycastDistanceLeft, Color.green);
        Debug.DrawRay(transform.position, rightDirection * _raycastDistanceRight, Color.red);

        // Apply damage if an enemy is detected in any of the raycasts
        if (_playerAnimator.GetBool("isStabbing"))
        {
            if (centerHit.collider != null) ApplyDamage(centerHit.collider);
            if (leftHit.collider != null) ApplyDamage(leftHit.collider);
            if (rightHit.collider != null) ApplyDamage(rightHit.collider);
        }
    }

    private void ApplyDamage(Collider2D enemyCollider)
    {
        // Check if the collider belongs to an enemy with the EnemyStats component
        EnemyStats enemyStats = enemyCollider.GetComponent<EnemyStats>();
        if (enemyStats != null && _hasDealtMeleeDamage)
        {
            enemyStats.TakeDamage(_meleeDamage);
            Debug.Log("Enemy hit! Damage applied: " + _meleeDamage);
            Debug.Log("Current enemy health: " + enemyStats.GetEnemyHealth());
            _hasDealtMeleeDamage = false;
        }
    }

    private void FireRifle()
    {
        if (Time.time >= _nextFireTime)
        {
            // Shooting logic specifically for the rifle
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            Vector2 direction = (mousePosition - transform.position).normalized;
            RotatePlayerSprite(direction);

            // Instantiate the rifle bullet
            _bullet = Instantiate(_bulletPrefab[0], _gun.position, Quaternion.identity);

            if (_weaponStat != null)
            {
                _weaponStat.SetWeaponDamage(10f);
                _weaponStat.SetWeaponFireRate(0.1f);
                _weaponStat.SetAmmoSpeed(25f);
                _gunDamage = _weaponStat.GetWeaponDamage();
                _bulletSpeed = _weaponStat.GetAmmoSpeed();
                _fireRate = _weaponStat.GetWeaponFireRate();

                Debug.Log("Rifle damage set to: " + _gunDamage);
                gunshots[1].Play();
            }
            else
            {
                Debug.LogWarning("WeaponStat component not found on the _gun object!");
            }

            // Get the Animator component of the bullet
            Animator bulletAnimator = _bullet.GetComponent<Animator>();
            if (bulletAnimator != null)
            {
                bulletAnimator.SetBool("isRocket", false);
            }

            // Set bullet velocity
            Rigidbody2D bulletRb = _bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.velocity = direction * _bulletSpeed;
            }

            _nextFireTime = Time.time + _fireRate;
        }
    }

    private void OnFire(InputValue inputValue)
    {
        if (!_isRifle)
        {
            if (Time.time >= _nextFireTime)
            {
                if (inputValue.isPressed)
                {
                    // Handle shooting logic
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = 0;

                    Vector2 direction = (mousePosition - transform.position).normalized;

                    RotatePlayerSprite(direction);

                    // Instantiate the bullet and get its Animator component
                    if (_isPistol && _isPistolAcquired)
                    {
                        _bullet = Instantiate(_bulletPrefab[0], _gun.position, Quaternion.identity);
                        // Set weapon damage for the rocket
                        if (_weaponStat != null)
                        {
                            _weaponStat.SetWeaponDamage(10f);
                            _weaponStat.SetWeaponFireRate(1f);
                            _weaponStat.SetAmmoSpeed(15f);
                            _gunDamage = _weaponStat.GetWeaponDamage();
                            _bulletSpeed = _weaponStat.GetAmmoSpeed();
                            _fireRate = _weaponStat.GetWeaponFireRate();

                            Debug.Log("Pistol damage set to: " + _gunDamage);
                            gunshots[0].Play();
                        }
                        else
                        {
                            Debug.LogWarning("WeaponStat component not found on the _gun object!");
                        }
                        
                    }
                    else if (_isRocket && _isRocketAcquired)
                    {
                        _bullet = Instantiate(_bulletPrefab[1], _gun.position, Quaternion.identity);

                        // Set weapon damage for the rocket
                        if (_weaponStat != null)
                        {
                            _weaponStat.SetWeaponDamage(25f);
                            _weaponStat.SetWeaponFireRate(1.5f);
                            _weaponStat.SetAmmoSpeed(18f);
                            _gunDamage = _weaponStat.GetWeaponDamage();
                            _bulletSpeed = _weaponStat.GetAmmoSpeed();
                            _fireRate = _weaponStat.GetWeaponFireRate();

                            Debug.Log("Rocket damage set to: " + _gunDamage);
                            gunshots[1].Play();
                        }
                        else
                        {
                            Debug.LogWarning("WeaponStat component not found on the _gun object!");
                        }

                        // Get the Animator component of the bullet
                        Animator bulletAnimator = _bullet.GetComponent<Animator>();
                        if (bulletAnimator != null)
                        {
                            bulletAnimator.SetBool("isRocket", true);
                        }
                    }
                    else if (_isShotgun && _isShotgunAcquired)
                    {
                        int numberOfBullets = 5; // Number of bullets to shoot in a spread
                        float spreadAngle = 15f; // Total angle spread between the bullets

                        // Loop to create multiple bullets
                        for (int i = 0; i < numberOfBullets; i++)
                        {
                            // Calculate the spread angle for each bullet
                            float angleOffset = spreadAngle * (i - (numberOfBullets - 1) / 2f); // Spread evenly around the base direction
                            Vector2 spreadDirection = Quaternion.Euler(0, 0, angleOffset) * direction; // Rotate the base direction to create spread

                            // Instantiate the bullet and set its position and rotation
                            _bullet = Instantiate(_bulletPrefab[0], _gun.position, Quaternion.identity); // Use _bulletPrefab[0] for shotgun

                            if (_weaponStat != null)
                            {
                                _weaponStat.SetWeaponDamage(10f);
                                _weaponStat.SetWeaponFireRate(1.2f);
                                _weaponStat.SetAmmoSpeed(15f);
                                _gunDamage = _weaponStat.GetWeaponDamage();
                                _bulletSpeed = _weaponStat.GetAmmoSpeed();
                                _fireRate = _weaponStat.GetWeaponFireRate();

                                Debug.Log("Shotgun damage set to: " + _gunDamage);
                                gunshots[1].Play();
                            }
                            else
                            {
                                Debug.LogWarning("WeaponStat component not found on the _gun object!");
                            }

                            // Get the Animator component of the bullet
                            Animator bulletAnimator = _bullet.GetComponent<Animator>();
                            if (bulletAnimator != null)
                            {
                                bulletAnimator.SetBool("isRocket", false);
                            }

                            // Set bullet velocity using the spread direction
                            Rigidbody2D bulletRb = _bullet.GetComponent<Rigidbody2D>();
                            if (bulletRb != null)
                            {
                                bulletRb.velocity = spreadDirection * _bulletSpeed; // Use the spread direction for bullet velocity
                            }
                        }
                    }

                    if (!_isShotgun)
                    {
                        // Set bullet velocity
                        Rigidbody2D bulletRb = _bullet.GetComponent<Rigidbody2D>();
                        if (bulletRb != null)
                        {
                            bulletRb.velocity = direction * _bulletSpeed;
                        }
                    }
                }
                _nextFireTime = Time.time + _fireRate;
            }
        }
    }

    private void RotatePlayerSprite(Vector2 direction)
    {
        float threshold = 0.001f; // Set a threshold to give vertical direction priority when needed

        // Prioritize shooting up or down when vertical movement is significant
        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x) + threshold)
        {
            if (direction.y > 0)
            {
                if (_isPistol)
                    _playerAnimator.SetBool("isShootingUp", true);
                else if(_isRocket)
                    _playerAnimator.SetBool("isRocketUp", true);
                else if(_isRifle)
                    _playerAnimator.SetBool("isRifleUp", true);
                else if (_isShotgun)
                    _playerAnimator.SetBool("isShotgunUp", true);
            }
            else if (direction.y < 0)
            {
                if (_isPistol)
                    _playerAnimator.SetBool("isShootingDown", true);
                else if (_isRocket)
                    _playerAnimator.SetBool("isRocketDown", true);
                else if (_isRifle)
                    _playerAnimator.SetBool("isRifleDown", true);
                else if (_isShotgun)
                    _playerAnimator.SetBool("isShotgunDown", true);
            }
        }
        else // Otherwise, prioritize horizontal movement
        {
            if (direction.x > 0)
            {
                if (_isPistol)
                    _playerAnimator.SetBool("isShootingRight", true);
                else if (_isRocket)
                    _playerAnimator.SetBool("isRocketRight", true);
                else if (_isRifle)
                    _playerAnimator.SetBool("isRifleRight", true);
                else if (_isShotgun)
                    _playerAnimator.SetBool("isShotgunRight", true);
            }
            else if (direction.x < 0)
            {
                if (_isPistol)
                    _playerAnimator.SetBool("isShootingLeft", true);
                else if (_isRocket)
                    _playerAnimator.SetBool("isRocketLeft", true);
                else if (_isRifle)
                    _playerAnimator.SetBool("isRifleLeft", true);
                else if (_isShotgun)
                    _playerAnimator.SetBool("isShotgunLeft", true);
            }
        }
    }

    private void ResetShootingAnimation(AnimatorStateInfo currentState)
    {
        // Reset pistol shooting animations
        if (_isPistol)
        {
            if (currentState.IsName("ShootRight") && currentState.normalizedTime >= 1.0f)
            {
                _playerAnimator.SetBool("isShootingRight", false);
            }
            if (currentState.IsName("ShootLeft") && currentState.normalizedTime >= 1.0f)
            {
                _playerAnimator.SetBool("isShootingLeft", false);
            }
            if (currentState.IsName("ShootUp") && currentState.normalizedTime >= 1.0f)
            {
                _playerAnimator.SetBool("isShootingUp", false);
            }
            if (currentState.IsName("ShootDown") && currentState.normalizedTime >= 1.0f)
            {
                _playerAnimator.SetBool("isShootingDown", false);
            }
        }

        // Reset rocket shooting animations
        else if (_isRocket)
        {
            if (currentState.IsName("RightRocket") && currentState.normalizedTime >= 1.0f)
            {
                _playerAnimator.SetBool("isRocketRight", false);
            }
            if (currentState.IsName("LeftRocket") && currentState.normalizedTime >= 1.0f)
            {
                _playerAnimator.SetBool("isRocketLeft", false);
            }
            if (currentState.IsName("UpRocket") && currentState.normalizedTime >= 1.0f)
            {
                _playerAnimator.SetBool("isRocketUp", false);
            }
            if (currentState.IsName("DownRocket") && currentState.normalizedTime >= 1.0f)
            {
                _playerAnimator.SetBool("isRocketDown", false);
            }
        }

        // Reset rocket shooting animations
        else if(_isRifle)
        {
            if (currentState.IsName("RightRifle") && currentState.normalizedTime >= 1.0f)
            {
                _playerAnimator.SetBool("isRifleRight", false);
            }
            if (currentState.IsName("LeftRifle") && currentState.normalizedTime >= 1.0f)
            {
                _playerAnimator.SetBool("isRifleLeft", false);
            }
            if (currentState.IsName("UpRifle") && currentState.normalizedTime >= 1.0f)
            {
                _playerAnimator.SetBool("isRifleUp", false);
            }
            if (currentState.IsName("DownRifle") && currentState.normalizedTime >= 1.0f)
            {
                _playerAnimator.SetBool("isRifleDown", false);
            }
        }

        // Reset rocket shooting animations
        else if(_isShotgun)
        {
            if (currentState.IsName("ShotgunRight") && currentState.normalizedTime >= 1.0f)
            {
                _playerAnimator.SetBool("isShotgunRight", false);
            }
            if (currentState.IsName("ShotgunLeft") && currentState.normalizedTime >= 1.0f)
            {
                _playerAnimator.SetBool("isShotgunLeft", false);
            }
            if (currentState.IsName("ShotgunUp") && currentState.normalizedTime >= 1.0f)
            {
                _playerAnimator.SetBool("isShotgunUp", false);
            }
            if (currentState.IsName("ShotgunDown") && currentState.normalizedTime >= 1.0f)
            {
                _playerAnimator.SetBool("isShotgunDown", false);
            }
        }
    }
    // Respawns player and reloads scene
    private void respawnOnDeath()
    {
        SceneManager.LoadScene(currentScene.name);
    }

    // Methods that set weapon the player wants to use
    public void setPistol(bool usePistol)
    {
        _isPistol = usePistol;
    }
    public void setRifle(bool useRifle)
    {
        _isRifle = useRifle;
    }
    public void setShotgun(bool useShotgun)
    {
        _isShotgun = useShotgun;
    }
    public void setRocket(bool useRocket)
    {
        _isRocket = useRocket;
    }

    // Methods that return if player has a weapon
    public bool hasPistol()
    {
        return _isPistolAcquired;
    }
    public bool hasRifle()
    {
        return _isRifleAcquired;
    }
    public bool hasShotgun()
    {
        return _isShotgunAcquired;
    }
    public bool hasRocket()
    {
        return _isRocketAcquired;
    }
}
