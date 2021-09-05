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
    private float rotateToAngle = 0.0f;
    private GameObject randomTarget;
    private AudioSource audioSource;
    private float nextRandomTime;
    private int touchingWall = 0;
    private bool isAutomated = true;
    private float turnDirection = 0;

    private float minX = 11f;
    private float maxX = -15.6f;
    private float minZ = -3.24f;
    private float maxZ = 12.6f;

    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the animator for this animal
        animator = GetComponent<Animator>();

        // Set Audio Source
        audioSource = GetComponent<AudioSource>();

        // Seed Random object
        Random.InitState((int)System.DateTime.Now.Ticks);

        // Get the direction the animal is facing
        rotateToAngle = transform.eulerAngles.y;

        // Create Random Target game object
        randomTarget = new GameObject("RandomTarget");

        randomTarget.transform.SetPositionAndRotation(transform.position, transform.rotation);

        // Stop walking animation
        Stop();

        // Set next random action for the animal
        NextRandomAction();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Time.time > nextRandomTime && touchingWall == 0 && isAutomated == true)
            NextRandomAction();

        //Debug.Log(this._name + ": " + transform.rotation.y + ", " + randomTarget.transform.rotation.y);

        if (isMoving)
        { 
            if (Mathf.Abs(transform.rotation.y - randomTarget.transform.rotation.y) > 1 || touchingWall > 0 )
            transform.Rotate(0f, rotateToAngle * Time.fixedDeltaTime, 0f);

            transform.Translate( Time.deltaTime * speed * Vector3.forward);
        }

    }

    private void NextRandomAction()
    {

        float action = Random.Range(0, 3);

        if (action == 0)
        {
            // Move
            float randomX = Random.Range(minX, maxX);
            float randomZ = Random.Range(minZ, maxZ);

            RotateTowardsTarget(randomX, randomZ);

            Walk();

        }
        else if (action == 1)
        {
            // Make Sound
            audioSource.Play();
        }
        else
        {
            // Eat
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


    // ABSTRACTION
    private void RotateTowardsTarget(float x = 0f, float z = 0f)
    {

        randomTarget.transform.position = new Vector3(x, randomTarget.transform.position.y, z);

        Vector3 posDifference = randomTarget.transform.position - transform.position;
        Quaternion rotateTowards = Quaternion.LookRotation(posDifference);

        float rotateDir = Vector3.Cross(transform.forward, posDifference).y;
        if (rotateDir >= 0)
            turnDirection = 1;
        else if (rotateDir < 0)
            turnDirection = -1;

        randomTarget.transform.rotation = rotateTowards;

        rotateToAngle = Quaternion.Angle(transform.rotation, rotateTowards) * turnDirection;

       
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Wall"))
        {
            touchingWall += 1;

            if (touchingWall == 1)
            {
                animator.SetFloat("Speed_f", 0.5f);

                RotateTowardsTarget(0f, 0f);

            }
        }

        if (other.CompareTag("Player"))
        {
            Stop();
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
