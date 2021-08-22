using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] private GameObject animal;
    protected Animator animator;
    protected float speed = 2.0f;
    protected string _name = "Animal";
    protected bool isMoving = false;
    private float currentAngle = 0.0f;

    private float nextRandomTime;
    private int touchingWall = 0;
    private bool isAutomated = true;
    private float turnDirection = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the animator for this animal
        animator = GetComponent<Animator>();
        
        // Seed Random object
        Random.InitState((int)System.DateTime.Now.Ticks);

        // Get the direction the animal is facing
        currentAngle = transform.eulerAngles.y;

        // Stop walking animation
        Stop();

        // Set the next random action time
        SetNextRandomTime();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Time.time > nextRandomTime && touchingWall == 0 && isAutomated == true)
            
            NextRandomAction();

        if (touchingWall > 0)
        {
            transform.Rotate(0f, currentAngle * Time.fixedDeltaTime, 0f);
        }

        if (isMoving)
        {
            transform.Translate( Time.deltaTime * speed * Vector3.forward);
        }

    }

    private void NextRandomAction()
    {

        float action = Random.Range(0, 2);

        if (action == 0)
        {

            Walk();

            // Move 0 - Forward, 1 - Left, 2 - Right
            float moveDirection = Random.Range(0, 3);
            if (moveDirection == 1)
            {
                currentAngle += 10.0f;
            }
            else if (moveDirection == 2)
            {
                currentAngle -= 10.0f;
            }

        } 
        else
        {
            Eat();
        }

        SetNextRandomTime();
    }

    private void SetNextRandomTime()
    {
        float additionalTime = Random.Range(5, 20);
        nextRandomTime = Time.time + additionalTime;
    }


    // ENCAPSULATION
    public string Name
    {
        get
        {
            return _name;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        

        if (other.CompareTag("Wall"))
        {
            touchingWall += 1;

            if (touchingWall == 1)
            {
                animator.SetFloat("Speed_f", 0.5f);

                Vector3 posDifference = new Vector3(0f, 0f, 0f) - transform.position;
                Quaternion rotateTowards = Quaternion.LookRotation(posDifference);
               
                float rotateDir = Vector3.Cross(transform.forward, posDifference).y;
                if (rotateDir >= 0)
                    turnDirection = 1;
                else if (rotateDir < 0)
                    turnDirection = -1;

                currentAngle = Quaternion.Angle(transform.rotation, rotateTowards) * turnDirection;

                Debug.Log(gameObject.name + " " + currentAngle);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            touchingWall -= 1;
        }
    }

    // ABSTRACTION
    public void AutomateOn()
    {
        isAutomated = true;
    }

    // ABSTRACTION
    public void AutomateOff()
    {
        isAutomated = false;
    }

    // ABSTRACTION
    public void Walk()
    {
        animator.SetBool("Eat_b", false);
        animator.SetFloat("Speed_f", 0.5f);
        isMoving = true;

    }

    // ABSTRACTION
    public void Eat()
    {
        animator.SetFloat("Speed_f", 0.0f);
        animator.SetBool("Eat_b", true);
        isMoving = false;
    }

    // ABSTRACTION
    public void Stop()
    {
        animator.SetFloat("Speed_f", 0f);
        isMoving = false;
    }
}
