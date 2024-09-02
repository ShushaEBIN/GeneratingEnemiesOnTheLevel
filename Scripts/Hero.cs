using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _speed;

    private int _currentWaypoint = 0;

    private void Update()
    {
        if (transform.position == _waypoints[_currentWaypoint].position)
        {
            _currentWaypoint = (_currentWaypoint + 1) % _waypoints.Length;
        }

        transform.position = Vector3.MoveTowards(transform.position, _waypoints[_currentWaypoint].position, _speed * Time.deltaTime);
    }
}