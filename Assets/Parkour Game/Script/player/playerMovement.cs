using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class playerMovement : MonoBehaviour
{
    public float _speed = 10;
    public float _jumpHeight = 3f;
    public Transform groundCheck;
    public LayerMask ground;

    Rigidbody rb;
    private bool isGrounded = false;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.5f, ground);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * _jumpHeight * Time.deltaTime, ForceMode.Impulse);
        }

        raycastChecker(groundCheck.position, groundCheck.forward, 1.5f);
    }

    void raycastChecker(Vector3 targetPosition, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPosition, direction);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, length))
        {
            rb.AddForce(Vector3.up * 30 * Time.deltaTime, ForceMode.Impulse);
        }
        else if(!Physics.Raycast(ray, out hit, length))
        {
            transform.Translate(transform.forward * _speed * Time.deltaTime);
        }
    }
}
