using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    // Variables
    public float movementSpeed;

    public GameObject playerMovePoint;
    private GameObject triggeringPMR;
    private Transform pmr;
    private bool pmrSpawned;

    private bool moving;



    // Update is called once per frame
    // Functions
    private void Start()
    {
        pmr = Instantiate(playerMovePoint.transform, this.transform.position, Quaternion.identity);
    }


    private void Update()
    {
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
                pmr.transform.position = mousePosition;
            }
        }


        if (moving)
        {
            Move();
        }
    }

    public void Move()
    {
        //transform.position = Vector3.MoveTowards(transform.position, pmr.transform.position, movementSpeed);
        if (pmr)
        {
            transform.position = Vector3.MoveTowards(transform.position, pmr.transform.position, movementSpeed);
            this.transform.LookAt(pmr.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PMR")
        {
            triggeringPMR = other.gameObject;
        }
    }
}
