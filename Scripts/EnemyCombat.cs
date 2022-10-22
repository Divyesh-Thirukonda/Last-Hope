using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyCombat : MonoBehaviour
{
    public float lookRadius = 100f;
    public Transform target;
    public NavMeshAgent agent;
    public Animator anim;
    public float enemyMaxHealth;
    public float enemyHealth = 50f;
    public float enemyDamage = 20f;
    public float enemyAttackRange = 7.5f;
    public float enemyAttackCooldown = 1f;
    public float attackTimer = 0;
    private PlayerCombat PlayerCombat;
    private GameObject Player;
    public const float locomationAnimationSmoothTime = .1f;
    public GameObject enemyDrop;

    System.Random randDrop = new System.Random();
    

    void Start()
    {
        Player = GameObject.Find("player");
        PlayerCombat = Player.GetComponent<PlayerCombat>();

        enemyMaxHealth = enemyHealth;
    }

    void faceTarget() {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void Update()
    {
        target = Player.transform;
        float SpeedPercent = agent.velocity.magnitude / agent.speed;

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius) {
            agent.SetDestination(target.position);

            if(distance <= agent.stoppingDistance) {
                faceTarget();
            }
        }
        
        if (distance <= enemyAttackRange) {
            attackTimer += 1 * Time.deltaTime;
            if(attackTimer >= enemyAttackCooldown) {
                anim.SetFloat("SpeedPercent", 2);
                PlayerStats.PlayerHealth -= (20- (20*(PlayerStats.PlayerDefense/100)));
                attackTimer = 0;
                if (PlayerStats.PlayerHealth <= 0) {
                    PlayerArmor.playerArmor = PlayerArmor.Armors.none;
                    SceneManager.LoadScene(0);
                }
            }
        } else {
            //anim.SetFloat("SpeedPercent", SpeedPercent, locomationAnimationSmoothTime, Time.deltaTime);
            attackTimer += 1 * Time.deltaTime;
        }
    }

    // IEnumerator EAttackAnim()
    // {
    //     anim.SetBool("isAttackingEnemy", true);

    //     GameObject.Find("player").GetComponent<AdvancedMovement>().enemyHitAudio.Play();

    //     yield return new WaitForSeconds(enemyAttackCooldown);

    //     anim.SetBool("isAttackingEnemy", false);
    // }

    public void TakeDamage () {
        enemyHealth -= PlayerStats.PlayerDamage;
        if (enemyHealth <= 0) {
            Die();
        }
    }

    public GameObject possiblity0;
    public GameObject possiblity1;
    public GameObject possiblity2;
    

    public void Die() {
        GameObject[] enemyDrops = {possiblity0, possiblity1, possiblity2};
        enemyDrop = enemyDrops[randDrop.Next(0,3)];
        if (enemyDrop != null) {
            GameObject go = Instantiate(enemyDrop, this.gameObject.transform);
            go.gameObject.transform.parent = null;
            if (go.gameObject.name == "getGun(Clone)") {go.transform.position += new Vector3(0, 25, 0);go.SetActive(true);}
            go.name = go.name.Replace("(Clone)", "");
            if (go.gameObject.name == "Potion") {go.transform.position += new Vector3(0, 1, 0);}
            
        }
        GameObject.Find("player").GetComponent<AdvancedMovement>().CollectedAudio.Play();
        Destroy(this.gameObject);
        PlayerCombat.coins += 400;
    }
}