using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Rigidbody character;
    [SerializeField] private Animator animator;

    [Header("Animator Parameters")]
    [SerializeField] private string speedAnimParam = "Speed";
    [SerializeField] private string jumpAnimParam = "Jump";
    [SerializeField] private string groundedAnimParam = "Grounded";
    [SerializeField] private string freeFallAnimParam = "FreeFall";
    [SerializeField] private string consecutiveJumpAnimParam = "ConsecutiveJump";

    [Header("Input Names")]
    [SerializeField] private string moveXInputName = "Horizontal";
    [SerializeField] private string moveZInputName = "Vertical";
    [SerializeField] private string orbitYInputName = "Mouse X";
    [SerializeField] private string jumpInputName = "Jump";
    [SerializeField] private string runInputName = "Fire1";

    private Vector3 inputMove = Vector3.zero;
    private Vector3 smoothInputMove = Vector3.zero;
    private float inputOrbit = 0f;
    private float smoothInputOrbit = 0f;
    private bool inputJump = false;
    private bool inputRun = false;

    [Header("Movement and Orbit")]
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float inAirSpeedFactor = 0.5f;
    [SerializeField] private float lookAngularSpeed = 30f;
    [SerializeField] private float inputOrbitScale = 10f;
    [SerializeField] private float followSpeed = 4f;
    [SerializeField] private float turnAngularSpeed = 6f;
    [SerializeField] private float jumpImpulse = 5f;
    [SerializeField] private float movementLerpSpeed = 17.5f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector3 groundCheckCenter = Vector3.zero;
    [SerializeField] private float groundCheckRadius = 0.175f;

    private Vector3 cameraOffset;
    private bool jumpedThisFrame;
    private bool cooldownMet = false;
    private bool isGrounded;
    private bool isFreeFalling;
    private float speed;

    private float jumpTime = 0;

    private void Awake()
    {
        cameraOffset = cam.transform.InverseTransformVector(character.position - cam.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        ReadInputs();
        isGrounded = CheckGround();
        isFreeFalling = CheckFreeFalling();
        jumpedThisFrame = (inputJump && isGrounded && cooldownMet);
        UpdateAnimatorParameters();
        Jump();
        //Orbit(Time.fixedDeltaTime);
        //Move(Time.fixedDeltaTime);

    }

    private void ReadInputs()
    {

        inputMove.x = Input.GetAxis(moveXInputName);
        inputMove.z = Input.GetAxis(moveZInputName);
        // Clamps magnitutde of movement to 1
        inputMove = Vector3.ClampMagnitude(inputMove, 1);

        smoothInputMove = Vector3.Lerp(smoothInputMove, inputMove, Time.deltaTime * runSpeed);

        inputOrbit = Input.GetAxis(orbitYInputName) * inputOrbitScale;
        smoothInputOrbit = Mathf.Lerp(smoothInputOrbit, inputOrbit, Time.deltaTime * lookAngularSpeed);

        inputJump = Input.GetButtonDown(jumpInputName);

        inputRun = Input.GetButton(runInputName);
    }

    private void UpdateAnimatorParameters()
    {
        animator.SetFloat(speedAnimParam, speed);
        animator.SetBool(jumpAnimParam, jumpedThisFrame);
        animator.SetBool(groundedAnimParam, isGrounded);
        animator.SetBool(freeFallAnimParam, isFreeFalling);
        animator.SetBool(consecutiveJumpAnimParam, isGrounded && jumpedThisFrame);


    }

    private bool CheckGround()
    {
        return Physics.CheckSphere(character.position + groundCheckCenter, groundCheckRadius, groundLayer);
    }

    private bool CheckFreeFalling()
    {
        return Mathf.Abs(character.velocity.y) > runSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawSphere(character.position + groundCheckCenter, groundCheckRadius);
    }

    private void FixedUpdate()
    {
        //Jump();
        Orbit(Time.fixedDeltaTime);
        Move(Time.fixedDeltaTime);
    }

    private void Jump()
    {
        if (inputJump && !isGrounded)
        {
            //Debug.Log("jump, but not grounded!");
        }

        cooldownMet = ((Time.time - jumpTime) > 1.2f);

        //Fix later: extr
        if (jumpedThisFrame)
        // if(jumpedThisFrame)
        {
            // UpdateAnimatorParameters();
            //Debug.Log("jump!");
            character.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);
            jumpTime = Time.time;

            //Debug.Log("velocity before " + character.velocity);
            Vector3 velocity = inAirSpeedFactor * speed * character.transform.forward;
            character.velocity += velocity;
            //Debug.Log("velocity after " + character.velocity);
            //character.AddForce(velocity, ForceMode.VelocityChange);
            return;
        }
        else if(jumpedThisFrame)
        {
            //Debug.Log("jump still on cooldown");
        }
    }

    private void Orbit(float delta)
    {
        Vector3 position = character.position - cam.transform.TransformVector(cameraOffset);
        cam.transform.position = Vector3.Lerp(cam.transform.position, position, delta * followSpeed);
        cam.transform.RotateAround(character.position, Vector3.up, smoothInputOrbit);

    }

    private void Move(float delta)
    {
        float targetSpeed = smoothInputMove.magnitude * (inputRun ? runSpeed : walkSpeed);
        speed = Mathf.Lerp(speed, targetSpeed, delta * movementLerpSpeed);
        // Debug.Log(speed);

        //if (jumpedThisFrame)
        //{
        //    Debug.Log("velocity before " + character.velocity);
        //    Vector3 velocity = inAirSpeedFactor * speed * character.transform.forward;
        //    character.velocity += velocity;
        //    Debug.Log("velocity after " + character.velocity);
        //    //character.AddForce(velocity, ForceMode.VelocityChange);
        //    return;
        //}

        if (inputMove == Vector3.zero)
        {
            return;
        }

        if (!isGrounded)
        {
            delta *= inAirSpeedFactor;
        }

        Vector3 direction = cam.transform.TransformVector(smoothInputMove);
        direction.y = 0;
        direction.Normalize();
        character.transform.forward = Vector3.Slerp(character.transform.forward, direction, turnAngularSpeed * delta);

        Vector3 position = character.position + delta * speed * character.transform.forward;
        character.MovePosition(position);

    }

}

