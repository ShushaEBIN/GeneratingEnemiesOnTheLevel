using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private SpawnPoint[] _spawnPoints;
    [SerializeField] private float _repeatRate;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxCapacity = 10;

    private ObjectPool<Enemy> _pool;
    private SpawnPoint _spawnPoint;
    private Enemy _prefab;

    private void Awake()
    {
        SetSpawnPoint();
        _prefab = _spawnPoint.Enemy;

        _pool = new ObjectPool<Enemy>(
        createFunc: () => Instantiate(_prefab),
        actionOnGet: (enemy) => SetParameters(enemy),
        actionOnRelease: (enemy) => enemy.gameObject.SetActive(false),
        actionOnDestroy: (enemy) => Delete(enemy),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxCapacity);
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        var wait = new WaitForSeconds(_repeatRate);

        while (enabled)
        {
            _pool.Get();

            yield return wait;
        }
    }

    private void SetParameters(Enemy enemy)
    {
        enemy.Died += SendToPool;

        SetSpawnPoint();
        _prefab = _spawnPoint.Enemy;
        enemy.transform.position = _spawnPoint.transform.position;
        enemy.SetTarget(_spawnPoint.Target);
        enemy.gameObject.SetActive(true);
    }

    private void SetSpawnPoint()
    {
        int firstSpawnPoint = 0;
        int spawnPoint = Random.Range(firstSpawnPoint, _spawnPoints.Length);

        _spawnPoint = _spawnPoints[spawnPoint];
    }

    private void SendToPool(Enemy enemy)
    {
        enemy.Died -= SendToPool;

        _pool.Release(enemy);
    }

    private void Delete(Enemy enemy)
    {
        enemy.Died -= SendToPool;

        Destroy(enemy);
    }
}