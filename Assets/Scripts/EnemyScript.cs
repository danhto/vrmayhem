using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    // walking speed
    public float speed = 0.5f;

    // angular speed
    public float angularSpeed = 1;

    // distance at which the zombie will chase the player
    public float chasingDistance = 20;

    // rigid body component
    Rigidbody rb;

    // animator
    Animator anim;

    // player
    PlayerContoller player;

    // available states
    enum State { idle, attacking, dead };
    
    // current state
    State currentState = State.idle;

    void Awake()
    {
        //get component
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        // get player
        player = FindObjectOfType<PlayerContoller>();

        if (player == null) Debug.LogError("There needs to be a PlayerController in the scene");

        // search for the player at an interval of 0.5 seconds
        InvokeRepeating("LookForPlayer", 0, 0.5f);
    }

    void LookForPlayer()
    {
        // only look if the zombie is idle
        if (currentState != State.idle) return;
        
        // Check distance
        if(Vector3.Distance(player.transform.position, transform.position) <= chasingDistance)
        {
            // change state to attacking
            currentState = State.attacking;

            // activate attack animation
            anim.SetBool("sawPlayer", true);

            // cancel the looking for player invokation
            CancelInvoke();
        }
    }

    void FixedUpdate()
    {
        // only chase if we are attacking!
        if (currentState != State.attacking) return;

        // direction of the movement
        Vector3 dir = player.transform.position - transform.position;
        dir.Normalize();

        // velocity
        Vector3 vel = dir * speed;

        // set rb velocity
        rb.velocity = vel;

        // instant rotation of the transform: transform.LookAt(player.transform.position);

        // rotation with angular speed
        
        // "flat difference" between player and the enemy
        Vector3 flatDiff = player.transform.position - transform.position;
        flatDiff.y = 0;

        // rotation needed, in Quaternion
        Quaternion targetRotation = Quaternion.LookRotation(flatDiff, Vector3.up);

        // angular rotation velocity vector
        Vector3 eulerAngleVelocity = new Vector3(0, angularSpeed, 0);

        // delta rotation (v = d / t ==> d = v * t)
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.fixedDeltaTime);

        // rigid body rotation
        rb.MoveRotation(targetRotation * deltaRotation);
    }

    void OnTriggerEnter(Collider other)
    {
        // check if a bullet has hit us
        if(other.CompareTag("Bullet"))
        {
            // change the state to dead
            currentState = State.dead;

            // activate death animation
            anim.SetBool("isAlive", false);

            // disable collider
            GetComponent<Collider>().enabled = false;
            rb.isKinematic = true;
        }
    }
}
