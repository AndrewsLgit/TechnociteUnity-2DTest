using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    #region Private

    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private int _maxAsteroids;
    private ProjectilePool _asteroidPool;
    private float _spawnInterval = 2f;
    private float _spawnTimer = 0f;
    private GameManager _gameManager;

    #endregion
    
    #region Main Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _asteroidPool = gameObject.GetComponent<ProjectilePool>();
        _gameManager = FindFirstObjectByType<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        SpawnAsteroid();
    }
    #endregion
    
    #region Utils

    private void SpawnAsteroid()
    {
        _spawnTimer += Time.deltaTime;
        int activeAsteroids = _asteroidPool.ActiveProjectileCount;
        if (_spawnTimer >= _spawnInterval && activeAsteroids <= _maxAsteroids)
        {
            GameObject asteroid = _asteroidPool.GetFirstAvailableProjectile();
            var randomPos = new Vector2(Random.Range(-_spawnPoint.localScale.x, _spawnPoint.localScale.x), Random.Range(-_spawnPoint.localScale.y, _spawnPoint.localScale.y));
            asteroid.transform.position = randomPos; //+ (Vector2)_spawnPoint.localScale/2;
            asteroid.SetActive(true);
            asteroid.GetComponent<AsteroidScript>().m_onAsteroidDestroyed.AddListener(_gameManager.OnScore);
            //Debug.Log($"Spawning asteroid: {asteroid.name}");
            _spawnTimer = 0f;
        }
    }
    
    #endregion
    
}
