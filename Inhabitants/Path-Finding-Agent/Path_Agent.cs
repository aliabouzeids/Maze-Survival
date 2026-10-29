using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Path_Agent : MonoBehaviour
{


    public Vector3 destination;
    public float offset;
    NavMeshAgent n_agent;
    Animator animator;
    void Awake()
    {
        n_agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (n_agent != null && Is_Near_This(destination, offset))
        {
            n_agent.SetDestination(transform.position);
            n_agent.ResetPath();
            n_agent.isStopped = true;
            n_agent.velocity = Vector3.zero;
            if (animator.GetBool("walk")) animator.SetBool("walk", false);
        }

    }
    public void Set_Destination(Vector3 _destination)
    {
        if (n_agent == null) return;
        n_agent.isStopped = false;
        n_agent.SetDestination(_destination);
        animator.SetBool("walk", true);
    }


    public bool Is_Near_This(Vector3 point, float _offset)
    {
        Vector3 position = transform.position + new Vector3(0f, 0.5f, 0);
        bool is_near = Vector3.Distance(position, point) <= _offset;
        return is_near;
    }
}