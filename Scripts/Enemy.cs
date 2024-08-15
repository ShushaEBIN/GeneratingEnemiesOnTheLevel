using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _timeOfLife = 5f;

    private Vector3 _direction;

    public event Action<Enemy> Died;

    public void GetDirection(Vector3 direction)
    {
        _direction = direction;
    }

    private void OnEnable()
    {
        StartCoroutine();
    }

    private void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime);
    }

    private void StartCoroutine()
    {
        StartCoroutine(Count());
    }

    private IEnumerator Count()
    {
        yield return new WaitForSeconds(_timeOfLife);

        Died?.Invoke(this);
    }
}