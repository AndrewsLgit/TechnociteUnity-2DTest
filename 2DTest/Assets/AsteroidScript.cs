using System;
using UnityEngine;
using UnityEngine.Events;

public class AsteroidScript : MonoBehaviour
{
    #region Public

    public UnityEvent m_onPlayerHit;
    public UnityEvent m_onAsteroidDestroyed;
    #endregion
    
    #region Private
    
    private Transform _playerTransform;
    private SimpleInput _playerInput;
    private LaserScript _laser;
    private Rigidbody2D _rigidbody;
    private int _thrust = 10;
    
    #endregion

    #region Main Methods

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _laser = gameObject.GetComponent<LaserScript>();
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _playerInput = _playerTransform.GetComponent<SimpleInput>();
    }

    // Update is called once per frame
    void Update()
    {
        LookAtPosition(_playerInput.GetPlayerPosition());
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Laser"))
        {
            Debug.Log($"Hit by laser: {collision.gameObject.name}");
            //Destroy(this.gameObject);
            Deactivate();
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Hit by player: {collision.gameObject.name}");
            m_onPlayerHit.Invoke();
        }
    }

    private void OnEnable()
    {
        //LookAtPosition(_playerInput.GetPlayerPosition());
    }

    #endregion

    #region Utils

    private void Move()
    {
        //TODO: Make Asteroid move towards the player location
        _rigidbody.AddForce(gameObject.transform.up * (Time.deltaTime * _thrust));
    }

    private void ReceiveDamage()
    {
        
    }
    
    private void LookAtPosition(Vector2 targetPos)
    {
        Vector2 direction = (targetPos) - (Vector2)gameObject.transform.position;
        //Vector2 direction = _playerInput.GetPlayerPosition() - (Vector2)gameObject.transform.position;
        gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        //_playerTransform.transform.rotation = _playerRotation;
        //Debug.Log($"Direction {direction}, Position {targetPos}");
    }

    private void Deactivate()
    {
        m_onAsteroidDestroyed.Invoke();
        m_onAsteroidDestroyed.RemoveAllListeners();
        gameObject.SetActive(false);
    }

    #endregion
}
