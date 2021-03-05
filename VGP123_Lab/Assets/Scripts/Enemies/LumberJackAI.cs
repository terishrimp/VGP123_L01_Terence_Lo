using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberJackAI : MonoBehaviour
{
    [SerializeField] float playerInRangeCheckRadius = 7f;
    [SerializeField] Transform playerInRangeCheckPos;

    [SerializeField] LumberJackTree tree;

    bool isPlayerInRange;
    Animator animator;
    public bool IsPlayerInRange
    {
        get { return isPlayerInRange; }
        set
        {
            if (value == isPlayerInRange) return;
            isPlayerInRange = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject enemyListGo = GameObject.FindGameObjectWithTag("enemyList");
        if (enemyListGo == null)
        {
            enemyListGo = Instantiate(new GameObject("Enemy List"), new Vector3(0, 0, 0), Quaternion.identity);
            enemyListGo.tag = "enemyList";
        }
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] goInRangeList= Physics2D.OverlapCircleAll(playerInRangeCheckPos.position, playerInRangeCheckRadius);
        foreach(var go in goInRangeList)
        {
            isPlayerInRange = false;
            if (go.CompareTag("Player"))
            {
                isPlayerInRange = true;
                break;
            }
        }
        if (isPlayerInRange)
        {
            //set animation bool to true
        }
        else
        {
            //set animation bool to false
        }
    }

}
