using NavMeshPlus.Extensions;
using UnityEngine;
using UnityEngine.AI;

public class NavigaionAgent : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private NavMeshAgent _agent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        _agent.SetDestination(_target.position);
    }
}
