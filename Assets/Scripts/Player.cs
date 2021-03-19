using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Variables
    // Player
    public float movementSpeed;
    Animation animate;
    private bool moving;
    public float attackTimer;
    private float currentAttackTimer;
    private bool attacking;
    private bool followingEnemy;

    private float damage;
    public float minDamage;
    public float maxDamage;
    private bool attacked;

    // PMR
    public GameObject playerMovePoint;
    private bool triggeringPMR;
    private Transform pmr;
    //private bool pmrSpawned;

    
    // Enemy
    private bool triggeringEnemy;
    private GameObject attackingEnemy;

    

    
    // Functions
    private void Start()
    {
        // Start is called before the first frame update
        pmr = Instantiate(playerMovePoint.transform, this.transform.position, Quaternion.identity);
        pmr.GetComponent<BoxCollider>().enabled = false;
        animate = GetComponent<Animation>();
        currentAttackTimer = attackTimer;
    }


    private void Update()
    {
        // Update is called once per frame
        // Player movement
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float hitDistance = 0.0f;

        if(playerPlane.Raycast(ray, out hitDistance))
        {
            Vector3 mousePosition = ray.GetPoint(hitDistance);
            //Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            if(Input.GetMouseButtonDown(0))
            {
                moving = true;
                triggeringPMR = false;
                pmr.transform.position = mousePosition;
                pmr.GetComponent<BoxCollider>().enabled = true;

                if(Physics.Raycast(ray,out hit))
                {
                    //print(hit.collider.gameObject.name);
                    if (hit.collider.tag == "Enemy")
                    {
                        attackingEnemy = hit.collider.gameObject;
                        followingEnemy = true;
                    }
                }
                else
                {
                    attackingEnemy = null;
                    followingEnemy = false;
                }
            }
        }


        if (moving)
        {
            Move();
        }
        else
        {
            if (attacking)
            {
                Attack();
            }
            else
            {
                Idle();
            }
        }

        if(triggeringPMR)
        {
            moving = false;
        }

        if(triggeringEnemy)
        {
            Attack();
        }

        if(attacked)
        {
            currentAttackTimer -= 1 * Time.deltaTime;
        }

        if(currentAttackTimer <= 0)
        {
            currentAttackTimer = attackTimer;
            attacked = false;
        }
    }

    public void Idle()
    {
        animate.CrossFade("idle");
    }

    public void Move()
    {
        //transform.position = Vector3.MoveTowards(transform.position, pmr.transform.position, movementSpeed);
        if (followingEnemy)
        {

            transform.position = Vector3.MoveTowards(transform.position, attackingEnemy.transform.position, movementSpeed);
            this.transform.LookAt(attackingEnemy.transform);

        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pmr.transform.position, movementSpeed);
            this.transform.LookAt(pmr.transform);
        }
        animate.CrossFade("walk");

    }

    public void Attack()
    {
        if (!attacked)
        {
            damage = Random.Range(minDamage, maxDamage);
            print(damage);
            attacked = true;
        }
        animate.CrossFade("attack");
        transform.LookAt(attackingEnemy.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PMR")
        {
            triggeringPMR = true;
        }
        else if(other.tag == "Enemy")
        {
            //print(attacking);
            triggeringEnemy = true;
            //_enemy = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "PMR")
        {
            triggeringPMR = false;
        }

        if (other.tag == "Enemy")
        {
            triggeringEnemy = false;
            //_enemy = null;
        }
    }
}
