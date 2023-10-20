using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Scripting.APIUpdating;

public class AIController : MonoBehaviour
{
    public NavMeshAgent agent;
    public float startWaitTime = 4f;
    public float timeToRotate = 2;
    public float speedWalk = .8f;
    public float speedRun = 2;

    public float viewRadius = 15;
    public float viewAngle = 180;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1f;
    public int edgeIterations = 4;
    public float edgeDistance = .5f;

    public Transform[] waypoints;
    int m_CurrentWaypointIndex;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;

    float m_WaitTime;
    float m_TimeToRotate;
    bool m_PlayerInRange;
    bool m_PlayerNear;
    bool m_IsPatrol;
    bool m_CaughtPlayer;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_PlayerInRange = false;
        m_PlayerNear = false;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;

        m_CurrentWaypointIndex = 0;
        agent = GetComponent<NavMeshAgent>();

        agent.isStopped = false;
        agent.speed = speedWalk;

        Debug.Log(m_CurrentWaypointIndex);
        Debug.Log(waypoints.Length);

        agent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        EnvironmentView();

        if (!m_IsPatrol)
        {            
            Chasing();
        }
        else
        {
            Patroling();
        }
    }

    void CaughPlayer()
    {
        animator.Play("AttackState");
        m_CaughtPlayer = true;
    }

    void Move(float speed)
    {
        animator.Play("WalkState");
        agent.isStopped = false;
        agent.speed = speed;
    }

    void Stop()
    {
        animator.Play("IdleState");
        agent.isStopped = true;
        agent.speed = 0;
    }

    private void Chasing()
    {
        animator.Play("WalkState");
        Debug.Log("Chasing");
        m_PlayerNear = false;
        playerLastPosition = Vector3.zero;

        if (!m_CaughtPlayer)
        {
            Move(speedRun);
            agent.SetDestination(m_PlayerPosition);
        }
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(speedWalk);
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                agent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }

    private void Patroling()
    {
        //animator.Play("IdleState");
        Debug.Log("Patroling");
        if (m_PlayerNear)
        {
            if (m_TimeToRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false;
            playerLastPosition = Vector3.zero;
            agent.SetDestination(waypoints[m_CurrentWaypointIndex].position);

            if (agent.remainingDistance < agent.stoppingDistance)
            {
                if (m_WaitTime <= 0)
                {
                    NexPoint();
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_TimeToRotate -= Time.deltaTime;
                }
            }
        }        
    }

    void NexPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    void LookingPlayer(Vector3 player)
    {
        agent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 1)
        {
            if (m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                agent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void EnvironmentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_PlayerInRange = true;
                    m_IsPatrol = false;
                }
                else
                {
                    m_PlayerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                m_PlayerInRange = false;
            }
            if (m_PlayerInRange)
            {
                m_PlayerPosition = player.transform.position;
            }
        }
    }
}
