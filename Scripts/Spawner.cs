using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy _prefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _repeatRate;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxCapacity = 10;

    private ObjectPool<Enemy> _pool;

    private void Awake()
    {
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
        StartCoroutine(GetEnemy());
    }

    private IEnumerator GetEnemy()
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

        enemy.transform.position = SetSpawnPoint();
        enemy.GetDirection(SetDirection());
        enemy.gameObject.SetActive(true);
    }

    private Vector3 SetSpawnPoint()
    {
        int firstSpawnPoint = 0;
        int spawnPoint = Random.Range(firstSpawnPoint, _spawnPoints.Length);

        return _spawnPoints[spawnPoint].transform.position;
    }

    private Vector3 SetDirection()
    {
        float minDegrees = 0f;
        float maxDegrees = 360f;
        float randomAngle = Random.Range(minDegrees, maxDegrees);

        return new Vector3(Mathf.Cos(randomAngle), minDegrees, Mathf.Sin(randomAngle));
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