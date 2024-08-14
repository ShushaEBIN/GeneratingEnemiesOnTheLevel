using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _timeOfLife = 5f;

    private bool _isCounting = false;

    public event Action<Enemy> Died;

    public void Reset()
    {
        _isCounting = false;
    }

    private void Update()
    {
        if (_isCounting == false)
        {
            _isCounting = true;

            StartCoroutine();
        }

        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
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