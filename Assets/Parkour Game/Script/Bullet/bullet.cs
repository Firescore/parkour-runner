using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        bulletHit(collision);
    }

    void bulletHit(Collision collision)
    {
        if (collision.gameObject.transform.GetComponent<Rigidbody>() != null)
        {
            playerShootingSystem.pSS.aimObject = null;
            cameraObject.Cam.target = null;
            collision.gameObject.layer = LayerMask.NameToLayer("Box");

    /*        if (collision.gameObject.transform.GetChild(0).transform.GetComponent<Animator>().enabled != false)
                collision.gameObject.transform.GetChild(0).transform.GetComponent<Animator>().enabled = false;*/

            if (collision.gameObject.GetComponent<EnemyAIController>() != null)
                collision.gameObject.GetComponent<EnemyAIController>().enabled = false;

            if (collision.gameObject.transform.GetChild(0).GetComponent<EnemyGunController>() != null)
                collision.gameObject.transform.GetChild(0).GetComponent<EnemyGunController>().enabled = false;

            collision.gameObject.transform.GetComponent<Rigidbody>().isKinematic = false;
            collision.gameObject.transform.GetComponent<Rigidbody>().useGravity = true;
            collision.gameObject.transform.GetComponent<Rigidbody>().AddForce(Vector3.forward * 40, ForceMode.Impulse);
            playerShootingSystem.pSS.aimObject = null;
            cameraObject.Cam.target = null;
            Destroy(collision.gameObject, 1f);
            Destroy(gameObject);
        }
        else
        {
            return;
        }
    }
}
