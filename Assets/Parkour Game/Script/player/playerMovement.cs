using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class playerMovement : MonoBehaviour
{
    public static playerMovement playerM;
    public float _speed = 10;
    public float _jumpHeight = 3f;
    public float slowDownFactor = 0.05f;
    public float slowDownLength = 2f;

    public Transform groundCheck;

    public LayerMask ground;

    public Transform ZiplineEnd;
    public bool isGrounded = false, timeTrigger = false, isZiplineGrabbed = false, isZiplineGrabReach = false, isJumping = false;

    /// <summary>
    [Header("---------------Wall Jump---------------")]
    public LayerMask WhatIsWall;
    public float wallRunForce, maxWallRunTime, maxWallSpeed;
    bool isWallRight, isWallLeft;
    public bool isWallRunning;
    public float maxWallRunCameraTilt, minWallRunCameraTilt, wallRunCameraTilt;
    public Transform oriantation;


    private void WallRunInput()
    {
        if (isWallRight) StartWallRun();
        if (isWallLeft) StartWallRun();
    }
    private void StartWallRun()
    {
        rb.useGravity = false;
        isWallRunning = true;
        if (rb.velocity.magnitude <= maxWallSpeed)
        {
            rb.AddForce(oriantation.forward * wallRunForce * Time.deltaTime);

            if (isWallRight)
                rb.AddForce(oriantation.right * wallRunForce / 5 * Time.deltaTime);
            else
                rb.AddForce(-oriantation.right * wallRunForce / 5 * Time.deltaTime);

        }
    }

    private void StopWallRun()
    {
        rb.useGravity = true;
        isWallRunning = false;
    }

    private void CheckForWall()
    {
        isWallRight = Physics.Raycast(transform.position, oriantation.right, 4f, WhatIsWall);
        isWallLeft = Physics.Raycast(transform.position, -oriantation.right, 4f, WhatIsWall);

        if (!isWallLeft && !isWallRight) StopWallRun();
    }
    /// </summary>

    Rigidbody rb;
    void Awake()
    {
        playerM = this;
        rb = GetComponent<Rigidbody>();
        
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }
    void FixedUpdate()
    {
        slowMotion();
        ZiplineRun();
        CheckForWall();
        WallRunInput();
        Jump();
        Time.timeScale += (1f / slowDownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.5f, ground);
        raycastChecker(groundCheck.position, groundCheck.forward, 1.5f);

        if (!isWallRunning)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0f, transform.position.y, transform.position.z), 5 * Time.deltaTime);
    }

    private void Jump()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        float rotateX = rot.y;
        if (wallRunCameraTilt < maxWallRunCameraTilt && isWallRunning && isWallRight)
            wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * 2;

        if (wallRunCameraTilt > minWallRunCameraTilt && isWallRunning && isWallLeft)
            wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * 2;


        /* if (wallRunCameraTilt > 0 && !isWallLeft || !isWallRight)
         {
             wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * 2;
         }


         if (wallRunCameraTilt < 0 && !isWallRight || !isWallLeft)
         {
             wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * 2;
         }*/

        if (wallRunCameraTilt > 0 && !isWallRunning)
        {
            wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * 2;
        }


        if (wallRunCameraTilt < 0 && !isWallRunning)
        {
            wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * 2;
        }



        transform.localRotation = Quaternion.Euler(transform.localRotation.y, rotateX, wallRunCameraTilt);
        oriantation.transform.localRotation = Quaternion.Euler(0, rotateX, 0);


        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * _jumpHeight * Time.deltaTime, ForceMode.Impulse);
        }

        if (isWallRunning)
        {
            rb.AddForce(Vector3.up * _jumpHeight * 1.5f);

            //if (isWallLeft || isWallRight) rb.AddForce(-oriantation.up * _jumpHeight * 1f);
            if (isWallRight) rb.AddForce(oriantation.right * _jumpHeight * 3.2f);
            if (isWallLeft) rb.AddForce(-oriantation.right * _jumpHeight * 3.2f);

            //rb.AddForce(transform.forward * _jumpHeight * 1f);
            rb.velocity = Vector3.zero;
        }

    }

    void slowMotion()
    {
        if (!isGrounded && !timeTrigger && !isZiplineGrabbed)
        {
            Time.timeScale = slowDownFactor;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            timeTrigger = true;
        }
        else if (isGrounded)
        {
            Time.timeScale = 1;
            timeTrigger = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("zipline"))
        {
            if (!isZiplineGrabbed && !isZiplineGrabReach)
            {
                isZiplineGrabbed = true;
                ZiplineEnd = other.gameObject.transform.GetChild(1);
            }

        }
    }
    void ZiplineRun()
    {

        if (isZiplineGrabbed)
        {
            if (transform.position != ZiplineEnd.position)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
                Time.timeScale = 1;
                timeTrigger = false;
                transform.position = Vector3.MoveTowards(transform.position, ZiplineEnd.position, (_speed * 2) * Time.deltaTime);
            }

        }

        if (ZiplineEnd != null)
        {
            if (transform.position == ZiplineEnd.position)
            {
                rb.isKinematic = false;
                rb.useGravity = true;

                isZiplineGrabbed = false;
                isZiplineGrabReach = true;
                ZiplineEnd = null;
            }
        }

        if (isGrounded)
        {
            StartCoroutine(grabR());
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
    IEnumerator grabR()
    {
        yield return new WaitForSeconds(0.2f);
        isZiplineGrabReach = false;
    }
    void raycastChecker(Vector3 targetPosition, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPosition, direction);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, length, ground) && !isZiplineGrabbed )
        {
            rb.AddForce(Vector3.up * 30 * Time.deltaTime, ForceMode.Impulse);
            isGrounded = true;
        }
        if (!Physics.Raycast(ray, out hit, length, ground) && !isZiplineGrabbed)
        {
            transform.Translate(transform.forward * _speed * Time.fixedDeltaTime);
        }
        }

}
