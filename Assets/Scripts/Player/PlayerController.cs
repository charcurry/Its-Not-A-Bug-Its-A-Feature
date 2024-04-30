using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private GameObject pCamera;
    private Rigidbody rb;
    private Camera pCameraComponent;
    private PlayerTrigger groundTrigger;
    private PlayerTrigger headTrigger;

    private float accelerationBaseValue;
    private float maxSpeedBaseValue;
    private float moveLeftRight;
    private float moveForwardBackward;
    private float jumpTimeStamp;

    private bool isGrounded;
    private bool isUnderObject;
    private bool isCrouching;
    private bool upPressed;
    private bool downPressed;
    private bool rightPressed;
    private bool leftPressed;
    private bool sprintPressed;
    private bool jumpPressed;
    private bool interactPressed;
    private bool crouchPressed;

    private double speedXZ;

    [Header("Properties")]
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float decelerationMultiplier;
    [SerializeField] private float jumpStrength;
    [SerializeField] private float extraGravity;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float crouchMultiplier;
    [SerializeField] private float playerHeight;
    [SerializeField] private float cameraHeight;
    [SerializeField] private float playerCrouchHeight;
    [SerializeField] private float cameraCrouchHeight;
    [SerializeField] private float headTriggerOffset;
    [SerializeField] private float interactDistance;
    [SerializeField] private float dynamicFOVRateOfChange;
    [SerializeField] private bool dynamicFOV;
    [SerializeField] private bool renderPlayerMesh;

    [HideInInspector] public bool canControl = true;
    [HideInInspector] public bool disableRegularForce = false;


    // Start is called before the first frame update
    void Start()
    {
        isGrounded = true;
        isUnderObject = false;
        isCrouching = false;

        rb = GetComponent<Rigidbody>();
        pCamera = transform.GetChild(0).gameObject;
        pCameraComponent = pCamera.GetComponent<Camera>();
        groundTrigger = transform.GetChild(1).GetComponent<PlayerTrigger>();
        headTrigger = transform.GetChild(2).GetComponent<PlayerTrigger>();

        headTrigger.transform.localPosition = new Vector3(0, (playerHeight - 1) + headTriggerOffset, 0);

        accelerationBaseValue = acceleration;
        maxSpeedBaseValue = maxSpeed;

        pCamera.transform.position = new Vector3(pCamera.transform.position.x, (transform.position.y - 1) + cameraHeight, pCamera.transform.position.z);
        transform.GetComponent<CapsuleCollider>().height = playerHeight;
        transform.GetComponent<CapsuleCollider>().center = new Vector3(0, (playerHeight / 2) - 1, 0);

        upPressed = false;
        downPressed = false;
        rightPressed = false;
        leftPressed = false;
        sprintPressed = false;
        jumpPressed = false;
        interactPressed = false;
        crouchPressed = false;

        jumpTimeStamp = -0.2f;

        if (!renderPlayerMesh)
            transform.GetComponent<MeshRenderer>().forceRenderingOff = true;
    }


    private void Update()
    {
        // Grabs player input and stores it
        if (canControl)
        {
            if (Input.GetKey(KeyCode.W))
                upPressed = true;

            if (Input.GetKey(KeyCode.S))
                downPressed = true;

            if (Input.GetKey(KeyCode.D))
                rightPressed = true;

            if (Input.GetKey(KeyCode.A))
                leftPressed = true;

            if (Input.GetKey(KeyCode.LeftShift))
                sprintPressed = true;

            if (Input.GetKeyDown(KeyCode.Space))
                jumpPressed = true;

            if (Input.GetKeyDown(KeyCode.E))
                interactPressed = true;

            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C))
                crouchPressed = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Calls on GroundTrigger to find out whether or not the player is grounded
        if (jumpTimeStamp + 0.2f < Time.time)
            isGrounded = groundTrigger.isObjectHere;

        // Calls on HeadTrigger to find out whether or not the player is under an object
        isUnderObject = headTrigger.isObjectHere;

        // Adds a constant downward force on the player
        rb.AddRelativeForce(new Vector3(0, -extraGravity * Time.deltaTime, 0));

        // Shoots a raycast out in the direction the player is looking
        if (Physics.Raycast(pCamera.transform.position, pCamera.transform.forward, out RaycastHit hit, interactDistance, ~(1 << 6)))
        {
            // Checks if the raycast hits an object with the Interactable parent script
            if (hit.collider.GetComponent<Interactable>())
            {
                if (interactPressed)
                    hit.collider.GetComponent<Interactable>().interaction();
            }

            if (interactPressed)
                Debug.Log(hit.transform.name);
        }

        interactPressed = false;

        // Set acceleration and max speed back to normal after sprint and crouch
        acceleration = accelerationBaseValue;
        maxSpeed = maxSpeedBaseValue;

        // If sprint is input, up both accelertion and speed
        if (sprintPressed && !isCrouching && (upPressed || leftPressed || rightPressed))
        {
            sprintPressed = false;
            maxSpeed = maxSpeed * sprintMultiplier;
            acceleration = acceleration * sprintMultiplier;
        }


        // If crouch is input, lower the camera and shorten the collider of the player
        if (isGrounded && crouchPressed && !jumpPressed || isUnderObject && isCrouching)
        {
            crouchPressed = false;
            isCrouching = true;
            // Reduces player height, camera height, and reduces speed
            pCamera.transform.position = new Vector3(pCamera.transform.position.x, Mathf.Lerp(pCamera.transform.position.y, (transform.position.y - 1) + cameraCrouchHeight, Time.deltaTime * 10), pCamera.transform.position.z);
            transform.GetComponent<CapsuleCollider>().height = playerCrouchHeight;
            transform.GetComponent<CapsuleCollider>().center = new Vector3(0, (playerCrouchHeight / 2) - 1, 0);
            maxSpeed = maxSpeed * crouchMultiplier;
            acceleration = acceleration * crouchMultiplier;
        }
        else
        {
            isCrouching = false;
            // Sets player and camera height back to the default
            pCamera.transform.position = new Vector3(pCamera.transform.position.x, Mathf.Lerp(pCamera.transform.position.y, (transform.position.y - 1) + cameraHeight, Time.deltaTime * 10), pCamera.transform.position.z);
            transform.GetComponent<CapsuleCollider>().height = playerHeight;
            transform.GetComponent<CapsuleCollider>().center = new Vector3(0, (playerHeight / 2) - 1, 0);
        }

        // Bug Fix
        if (sprintPressed && crouchPressed)
        {
            sprintPressed = false;
            crouchPressed = false;
        }

        // Resets force adding variables to zero for the next add force
        moveForwardBackward = 0;
        moveLeftRight = 0;

        // Turns WASD input into the add force variables 
        if (upPressed)
        {
            moveForwardBackward += 1;
            upPressed = false;
        }
        if (downPressed)
        {
            moveForwardBackward -= 1;
            downPressed = false;
        }
        if (rightPressed)
        {
            moveLeftRight += 1;
            rightPressed = false;
        }
        if (leftPressed)
        {
            moveLeftRight -= 1;
            leftPressed = false;
        }

        speedXZ = Math.Sqrt(Math.Pow(rb.velocity.x, 2) + Math.Pow(rb.velocity.z, 2));

        if (!disableRegularForce)
        {
            // Uses the add force variables to add force as well as capping the speed
            if (speedXZ < maxSpeed)
                rb.AddRelativeForce(new Vector3(moveLeftRight, 0, moveForwardBackward).normalized * Time.deltaTime * 1000 * acceleration);

            // Increases deceleration to prevent sliding
            rb.velocity = new Vector3 (rb.velocity.x * decelerationMultiplier, rb.velocity.y, rb.velocity.z * decelerationMultiplier);
        }

        // If the player is on the ground and pressed jump then add force to the y for a jump
        if (jumpPressed && isGrounded)
        {
            rb.AddForce(new Vector3(0.0f, jumpStrength, 0.0f), ForceMode.Impulse);
            jumpTimeStamp = Time.time;
        }

        jumpPressed = false;

        // Changes fov based on speed
        if (dynamicFOV)
            pCameraComponent.fieldOfView = Mathf.Lerp(pCameraComponent.fieldOfView, 60 + (15 * Mathf.Clamp01(((float)speedXZ - 2) / ((maxSpeedBaseValue * sprintMultiplier * 1.2f) - 2))), Time.deltaTime * dynamicFOVRateOfChange);
    
    }

    // Takes a Y rotation and sets the player's camera to that direction
    public void RecenterCamera(float yRotation = 0)
    {
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        pCamera.GetComponent<CameraController>().xRotation = 0;
    }

    // Gives player postition
    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
