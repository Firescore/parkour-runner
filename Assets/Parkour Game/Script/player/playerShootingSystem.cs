using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShootingSystem : MonoBehaviour
{
    public Transform shootPoint;
    public GameObject bullet;
    public LineRenderer line;
    public float _force = 50;
    public float leserMaxLength = 10;

    private GameObject bulletPrefeb;

    void Update()
    {
        raycastChecker(shootPoint.position, shootPoint.forward, leserMaxLength);
        if (Input.GetKeyDown(KeyCode.F))
        {
            bulletPrefeb = Instantiate(bullet, shootPoint.position, Quaternion.identity);
            bulletPrefeb.GetComponent<Rigidbody>().AddForce(shootPoint.forward * _force, ForceMode.Impulse);
            Destroy(bulletPrefeb, 3);
        }
    }

    void raycastChecker(Vector3 targetPosition,Vector3 direction,float length)
    {

        Ray ray = new Ray(targetPosition, direction);
        RaycastHit hit;
        Vector3 endPos = targetPosition + (length * direction);

        if(Physics.Raycast(ray,out hit, length))
        {
            endPos = hit.point;
            line.enabled = true;
            line.SetPosition(0, targetPosition);
            line.SetPosition(1, endPos);
        }
        else
        {
            line.enabled = false;
        }
        
    }
}
