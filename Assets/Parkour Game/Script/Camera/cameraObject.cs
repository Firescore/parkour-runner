using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraObject : MonoBehaviour
{
    public static cameraObject Cam;
    public Transform target;
    public float FastTime, SlowTime;

    private float lookSpeed = 10; 

    private void Start()
    {
        Cam = this;
    }
    private void Update()
    {

        if (playerMovement.playerM.timeTrigger)
            lookSpeed = FastTime;
        if (!playerMovement.playerM.timeTrigger)
            lookSpeed = SlowTime;

        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction).normalized;
            Quaternion lookAt = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * lookSpeed);
            transform.rotation = lookAt;
        }
        if (target == null && !playerMovement.playerM.isWallRunning)
        {
            Quaternion lookAt = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0,0,0), Time.deltaTime * lookSpeed);
            transform.rotation = lookAt;
        }
        
    }
}
