using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyAIController : MonoBehaviour
{
    public static EnemyAIController enemyAI;
    public float DistanceFromPlayer;
    [SerializeField]private GameObject Player;
    private Animator enemyAnimation;

    public bool isPlayerInRange = false;
    // Start is called before the first frame update
    void Start()
    {

        enemyAI = this;
        Player = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).gameObject;
        enemyAnimation = transform.GetChild(0).transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceChecker();
    }
    void distanceChecker()
    {
        DistanceFromPlayer = Vector3.Distance(transform.position, Player.transform.position);
        if (DistanceFromPlayer <= 30)
        {
            isPlayerInRange = true;
            enemyAnimation.SetBool("shoot", true);
        }

        if (DistanceFromPlayer > 20)
        {
            enemyAnimation.SetBool("shoot", false);
            isPlayerInRange = false;
        }
    }
}
