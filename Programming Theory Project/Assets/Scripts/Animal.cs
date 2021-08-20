using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] private GameObject animal;
    private Animator animator;
    private float nextRandomTime;
    protected float speed = 2.0f;
    protected string _name = "Animal";
    private bool isMoving = false;
    private float currentAngle = 0.0f;
    private float changeAngle = 0.0f;
    private int touchingWall = 0;
    private bool isAutomated = true;

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
            Walk();
            transform.Rotate(0f, currentAngle * Time.deltaTime, 0f);
        }

        if (isMoving)
        {
            transform.Translate( Time.deltaTime * speed * Vector3.forward);
        }

    }

    protected void NextRandomAction()
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
            Stop();
            touchingWall += 1;

            float moveDirection = Random.Range(0, 2);

            if (moveDirection == 0)
                changeAngle = 10.0f;
            else
                changeAngle = -10.0f;

            Debug.Log(name + " touched " + other.name);
            Debug.Log("touching " + touchingWall + " walls");
            Debug.Log("Current Angle: " + currentAngle);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (touchingWall > 0 && other.CompareTag("Wall"))
        {
            currentAngle += changeAngle;
            if (currentAngle < 0)
                currentAngle += 360;
            else if (currentAngle > 360)
                currentAngle -= 360;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            touchingWall -= 1;
            Walk();
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

        animator.SetFloat("Speed_f", 0.5f);
        animator.SetBool("Eat_b", false);
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
