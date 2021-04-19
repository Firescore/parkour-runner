using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShootingSystem : MonoBehaviour
{
    public static playerShootingSystem pSS;
    public Transform shootPoint;
    public GameObject bullet;
    
    public float _force = 50;
    public float leserMaxLength = 10;

    private GameObject bulletPrefeb;
    private bool action = false;


    //Aim Veriables//
    [Header("Aim Veriables")]
    public LayerMask EnemyMask;

    public new Transform camera;
    public GameObject aimObject;
    public float AimRange = 10;
    public float lookSpeed = 10;
    public bool enemyIsInRange = false;
    //End

    private void Start()
    {
        pSS = this;
    }
    void Update()
    {

        raycastChecker(shootPoint.position, shootPoint.forward, leserMaxLength);
        if (Input.GetMouseButtonDown(0))
        {
            bulletPrefeb = Instantiate(bullet, shootPoint.position, Quaternion.identity);
            bulletPrefeb.GetComponent<Rigidbody>().AddForce(shootPoint.forward * _force, ForceMode.Impulse);
            Destroy(bulletPrefeb, 3);
        }
        aimBot();
        if(aimObject!=null)
            cameraObject.Cam.target = aimObject.transform;
    }

    void aimBot()
    {
        enemyIsInRange = Physics.CheckSphere(transform.position, AimRange, EnemyMask);
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, AimRange, out hit, AimRange,EnemyMask))
        {
            if (aimObject == null)
            {
                aimObject = hit.transform.GetChild(1).transform.gameObject;
            }
        }
    }

    void raycastChecker(Vector3 targetPosition,Vector3 direction,float length)
    {

        Ray ray = new Ray(targetPosition, direction);
        RaycastHit hit;
        Vector3 endPos = targetPosition + (length * direction);

        if (Physics.Raycast(ray, out hit, length))
        {
            endPos = hit.point;
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AimRange);
    }
}
