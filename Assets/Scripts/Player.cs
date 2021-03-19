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
    private bool attacking;

    // PMR
    public GameObject playerMovePoint;
    private bool triggeringPMR;
    private Transform pmr;
    //private bool pmrSpawned;

    
    // Enemy
    private bool triggeringEnemy;
    private GameObject _enemy;

    

    
    // Functions
    private void Start()
    {
        // Start is called before the first frame update
        pmr = Instantiate(playerMovePoint.transform, this.transform.position, Quaternion.identity);
        pmr.GetComponent<BoxCollider>().enabled = false;
        animate = GetComponent<Animation>();
    }


    private void Update()
    {
        // Update is called once per frame
        // Player movement
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
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
    }

    public void Idle()
    {
        animate.CrossFade("idle");
    }

    public void Move()
    {
        //transform.position = Vector3.MoveTowards(transform.position, pmr.transform.position, movementSpeed);
        transform.position = Vector3.MoveTowards(transform.position, pmr.transform.position, movementSpeed);
        this.transform.LookAt(pmr.transform);

        animate.CrossFade("walk");

    }

    public void Attack()
    {
        animate.CrossFade("attack");
        transform.LookAt(_enemy.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PMR")
        {
            triggeringPMR = true;
        }
        else if(other.tag == "Enemy")
        {
            print(attacking);
            triggeringEnemy = true;
            _enemy = other.gameObject;
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
            _enemy = null;
        }
    }
}
