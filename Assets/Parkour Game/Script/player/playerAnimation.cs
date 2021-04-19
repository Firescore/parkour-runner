using UnityEngine;

public class playerAnimation : MonoBehaviour
{
    public Animator playerAnime;
    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            playerAnime.SetTrigger("shoot");
        }

        if (playerMovement.playerM.isZiplineGrabbed)
        {
            playerAnime.SetBool("hang", true);
        }
        if (!playerMovement.playerM.isZiplineGrabbed)
        {
            playerAnime.SetBool("hang", false);
        }
    }
}
