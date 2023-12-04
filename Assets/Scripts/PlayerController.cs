using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Movement components
    private CharacterController controller;
    private Animator animator;

    private float moveSpeed = 4f;

    [Header("Movement system")]
    public float walkSpeed = 4f;
    public float runSpeed = 8f;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //Direction in a normalised vector
        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 velocity = dir * moveSpeed * Time.deltaTime;

        //Check sprint
        if (Input.GetButton("Sprint"))
        {
            moveSpeed = runSpeed;
            animator.SetBool("Running", true);
        }
        else
        {
            moveSpeed = walkSpeed;
            animator.SetBool("Running", false);
        }

        //Check movement
        if(dir.magnitude >= 0.1f)
        {
            //Look towards the direction
            transform.rotation =  Quaternion.LookRotation(dir);

            //Move
            controller.Move(velocity);
        }

        animator.SetFloat("Speed", velocity.magnitude);

    }
}
