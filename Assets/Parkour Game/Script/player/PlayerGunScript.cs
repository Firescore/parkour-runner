using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunScript : MonoBehaviour
{
    public GameObject Gun;
    public void HideGun()
    {
        Gun.SetActive(false);
    }
    public void UnHideGun()
    {
        Gun.SetActive(true);
    }
}
