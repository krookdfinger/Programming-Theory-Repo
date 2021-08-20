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


    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the animator for this animal
        animator = GetComponent<Animator>();

        // Stop walking animation
        animator.SetFloat("Speed_f", 0.0f);

        // Seed Random object
        Random.InitState((int)System.DateTime.Now.Ticks);

        currentAngle = transform.eulerAngles.y;

        // Set the next random action time
        SetNextRandomTime();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > nextRandomTime && touchingWall == 0)
            NextRandomAction();

        if (touchingWall > 0)
        {
            transform.Rotate(0f, currentAngle * Time.deltaTime, 0f);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }

        if (isMoving)
        {
            animator.SetFloat("Speed_f", 0.5f);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }

    }

    protected void NextRandomAction()
    {

        float action = Random.Range(0, 2);

        if (action == 0)
        {
            animator.SetBool("Eat_b", false);

            isMoving = true;

            // Move 0 - Forward, 1 - Left, 2 - Right
            float moveDirection = Random.Range(0, 3);
            if (moveDirection == 1)
            {
                currentAngle += 10.0f;
                transform.Rotate(0f, currentAngle * Time.deltaTime, 0f);
            }
            else if (moveDirection == 2)
            {
                currentAngle -= 10.0f;
                transform.Rotate(0f, currentAngle * Time.deltaTime, 0f);
            }

        }
        else
        {
            isMoving = false;
            animator.SetFloat("Speed_f", 0.0f);
            animator.SetBool("Eat_b", true);

        }

        SetNextRandomTime();
    }

    private void SetNextRandomTime()
    {
        float additionalTime = Random.Range(5, 20);
        nextRandomTime = Time.time + additionalTime;
    }

    public string Name
    {
        get
        {
            return _name;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            isMoving = false;
            animator.SetFloat("Speed_f", 0.0f);
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
        if (touchingWall > 0 && other.tag == "Wall")
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
        if (other.tag == "Wall")
        {
            touchingWall -= 1;
            isMoving = true;
        }

    }
}
