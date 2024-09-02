using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;

    private Transform _target;

    public event Action<Enemy> Died;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
    }    

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<Hero>(out Hero component))
        {
            Died?.Invoke(this);
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}