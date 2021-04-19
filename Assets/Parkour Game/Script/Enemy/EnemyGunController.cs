using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunController : MonoBehaviour
{
    public GameObject Gun;
    public GameObject Bullet;

    private Transform target;
    public Transform LocalTarget;
    public Transform ShootPoint;
    public Transform bone;

    public float shootForce = 50;

    public bool isGunActive = false;
    private bool isShootAvailable = false;
    // Start is called before the first frame update
    void Awake()
    {
        isGunActive = false;
        target = GameObject.Find("Player").transform.GetChild(1).transform;
    }

    // Update is called once per frame
    void Update()
    {
        GunAppearance();
        if (target == null)
            return;
    }
    private void LateUpdate()
    {
        Vector3 targetPosiion = target.position;
        AimAtTarget(bone, targetPosiion);
    }
    void AimAtTarget(Transform bone,Vector3 targetPos)
    {
        Vector3 aimDirection = ShootPoint.forward;
        Vector3 targetDirection = targetPos - ShootPoint.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        bone.rotation = aimTowards * bone.rotation;
    }
    void AimAtLocalTarget(Transform bone, Vector3 targetPos)
    {
        Vector3 aimDirection = ShootPoint.forward;
        Vector3 targetDirection = targetPos - ShootPoint.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        bone.rotation = aimTowards * bone.rotation;
    }
    public void Shoot()
    {
        if (!isShootAvailable)
        {
            GameObject bullet = Instantiate(Bullet, ShootPoint.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().AddForce(ShootPoint.forward * shootForce, ForceMode.Impulse);
            Destroy(bullet, 1.5f);
        }
    }

    
    void GunAppearance()
    {
        if (isGunActive)
        {
            Gun.SetActive(true);
        }
        else if (!isGunActive) { 
            Gun.SetActive(false);
        }
    }
    public void activeGun()
    {
        if (!isGunActive)
        {
            isGunActive = true;
        }
    }
    public void deactiveGun()
    {
        if (isGunActive)
        {
            isGunActive = false;
        }
    }

    public void ResetShoot()
    {
        if (isShootAvailable)
        {
            isShootAvailable = false;
        }
        if (!isShootAvailable)
            return;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(ShootPoint.position, ShootPoint.position + ShootPoint.forward * 50f);

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(LocalTarget.position, 0.05f);
    }
}
