using System;
using com.ajc.input;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleInput : MonoBehaviour, GameInputSystem.IPlayerActions
{
    
    #region Public
    
    #endregion
    
    #region Private
    
    // Method #2 is not needed anymore after implementing the GameInputSystem class generated from out InputSystem_Actions
    /*private InputAction _moveAction;
    private InputAction _jumpAction;*/
    
    [SerializeField] 
    private Camera _mainCamera;
    [SerializeField]
    private Transform _playerTransform;
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _maxSpeed = 10f;
    [SerializeField]
    private LayerMask _layerMask;
    [SerializeField]
    private GameObject _thrusterSprite;
    [SerializeField]
    private GameObject _laserSprite;
    [SerializeField] 
    private Transform _weaponPoint;
    [SerializeField]
    private ProjectilePool _laserProjectilePool;
    
    private Ray _ray;
    private Rigidbody2D _playerRigidBody2D;
    private Quaternion _playerRotation;
    private Vector2 _selectedPos;
    private GameInputSystem _gameInputSystem;
    private Vector2 _movementInput;

    #endregion

    #region Main Methods
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Method #2
        /*_moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");*/

        _mainCamera = Camera.main;
        //_playerTransform = gameObject.transform;
        _selectedPos = _playerTransform.transform.position;
        _playerRigidBody2D = GetComponentInChildren<Rigidbody2D>();
        _thrusterSprite.SetActive(false);
        
        _gameInputSystem = new GameInputSystem();
        _gameInputSystem.Enable();
        _gameInputSystem.Player.SetCallbacks(this);
    }

    private void OnDestroy()
    {
        _gameInputSystem.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        var mousePositionInScreen = _gameInputSystem.Player.Look.ReadValue<Vector2>();
        _selectedPos = _mainCamera.ScreenToWorldPoint(new Vector3(mousePositionInScreen.x, mousePositionInScreen.y, _mainCamera.transform.position.z));
        //Debug.Log(_selectedPos);
        LookAtPosition(_selectedPos);
        
        Move();
        
        /*if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            //Do something
        }*/

        // Method #2
        /*if (_jumpAction.WasPressedThisFrame())
        {
            Debug.Log("Jump");
        }*/
        //_movementInput = _gameInputSystem.Player.Move.ReadValue<Vector2>();
    }
    
    #endregion

    #region Utils

    public Vector2 GetPlayerPosition()
    {
        var playerPosition = (Vector2)_playerTransform.position;
        //Debug.Log($"GetPlayerPosition: {playerPosition}");
        return playerPosition;
    }
    // Useless now that we use the input system above!
    /*public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("Move");
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // context.performed to log "Jump" each time space is pressed instead of every frame WHILE space is pressed
        if (context.performed) Debug.Log("Jump");
    }*/

    private void LookAtPosition(Vector2 targetPos)
    {
        Vector2 direction = targetPos - (Vector2)_playerTransform.transform.position;
        _playerRotation = Quaternion.LookRotation(Vector3.forward, direction);
        _playerTransform.transform.rotation = _playerRotation;
        //Debug.Log($"Hit {_playerRotation}");
    }

    private void Move()
    {
        //transform.Translate(_movementInput * (Time.deltaTime * _speed));
        if (_movementInput.magnitude < _maxSpeed)
        {
            _playerRigidBody2D.AddForce((_playerTransform.up * (_movementInput.y * _speed)));
            _playerRigidBody2D.AddForce((_playerTransform.right * (_movementInput.x * _speed)));
            
        }
        _thrusterSprite.SetActive(_movementInput.magnitude > 0);
        
        //_playerRigidBody2D.AddForce(_movementInput * (_speed * Time.deltaTime));
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //Debug.Log("OnLook");
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        //Debug.Log("OnAttack");
        if (context.phase == InputActionPhase.Performed)
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            LaserAttack();
        }
    }

    private void LaserAttack()
    {
        //GameObject laser = Instantiate(_laserSprite, _weaponPoint.position, _playerRotation);
        GameObject laser = _laserProjectilePool.GetFirstAvailableProjectile();
        laser.transform.position = _weaponPoint.position;
        laser.transform.rotation = _playerTransform.rotation;
        laser.SetActive(true);
        Debug.Log($"Shot {laser.name}");
    }

    public void OnDeath()
    {
        Debug.Log("Player death");
        //_gameInputSystem.Disable();
        gameObject.SetActive(false);
    }
    
    #endregion
}
