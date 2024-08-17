using UnityEngine;
using UnityEngine.AI;

public class AnimalMovement : MonoBehaviour
{
    public enum AIState { Idle, Walking, Running }
    public AIState currentState = AIState.Idle;
    public float walkSpeed = 3.5f;
    public float wanderRadius = 10f;
    public float minWanderInterval = 3f;
    public float maxWanderInterval = 10f;

    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = walkSpeed;
        timer = Random.Range(minWanderInterval, maxWanderInterval);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = Random.Range(minWanderInterval, maxWanderInterval);
        }
        if (agent.velocity.magnitude > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(agent.desiredVelocity, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 360 * Time.deltaTime);
        }
    }

    Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}