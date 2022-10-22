using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyDrive : MonoBehaviour
{
    public float lookRadius = 100f;
    public Transform target;
    public NavMeshAgent agent;
    public Animator anim;
    public float enemyAttackRange = 7.5f;
    public float attackTimer = 0;
    private GameObject Player;
    public const float locomationAnimationSmoothTime = .1f;
    

    void Start()
    {
        Player = GameObject.Find("player");
    }

    void faceTarget() {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void Update()
    {
        float SpeedPercent = agent.velocity.magnitude / agent.speed;

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius) {
            agent.SetDestination(target.position);

            if(distance <= agent.stoppingDistance) {
                faceTarget();
            }
        }
        
        if (distance <= enemyAttackRange) {
            anim.SetFloat("SpeedPercent", 2, locomationAnimationSmoothTime, Time.deltaTime);
        } else {
            anim.SetFloat("SpeedPercent", SpeedPercent, locomationAnimationSmoothTime, Time.deltaTime);
            attackTimer += 1 * Time.deltaTime;
        }
    }
}