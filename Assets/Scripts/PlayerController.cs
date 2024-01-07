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

    private float gravity = 9.81f;

    //Player interaction component
    PlayerInteraction playerInteraction;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        //Get interaction component
        playerInteraction = GetComponentInChildren<PlayerInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        Interact();

        //Toggle relationship panel
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject relationshipPanel = UIManager.Instance.relationshipListingManager.gameObject;
            relationshipPanel.SetActive(!relationshipPanel.activeSelf);
        }
    }

    //Handles interaction, send input to player interaction
    public void Interact()
    {
        //Tool interaction
        if (Input.GetButtonDown("Fire1"))
        {
            //Interact
            playerInteraction.Interact();
        }

        //Item interaction
        if (Input.GetButtonDown("Fire2"))
        {
            //Interact
            playerInteraction.ItemInteract();
        }

        //Keep items from hand
        if (Input.GetButtonDown("Fire3"))
        {
            //Interact
            playerInteraction.ItemKeep();
        }
    }

    //Handles movement
    public void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //Direction in a normalised vector
        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 velocity = dir * moveSpeed * Time.deltaTime;

        if (controller.isGrounded)
        {
            velocity.y = 0;
        }
        velocity.y -= Time.deltaTime * gravity;

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

        animator.SetFloat("Speed", dir.magnitude);

    }
}
