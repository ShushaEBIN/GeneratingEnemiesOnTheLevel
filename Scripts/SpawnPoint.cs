using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Enemy _enemy;
    
    public Transform Target => _target;
    public Enemy Enemy => _enemy;
}