using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberJackAI : MonoBehaviour
{
    [SerializeField] float playerInRangeCheckRadius = 7f;
    [SerializeField] Transform playerInRangeCheckPos;

    [SerializeField] LumberJackTree tree;

    bool isPlayerInRange = false;
    bool canBreakLog = false;
    Animator animator;
    const string animBreakLogString = "canBreakLog";
    public bool IsPlayerInRange
    {
        get { return isPlayerInRange; }
        set
        {
            if (value == isPlayerInRange) return;
            isPlayerInRange = value;
            Debug.Log(value);
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
        Collider2D[] goInRangeList = Physics2D.OverlapCircleAll(playerInRangeCheckPos.position, playerInRangeCheckRadius);
        IsPlayerInRange = false;
        foreach (var go in goInRangeList)
        {
            if (go.CompareTag("Player"))
            {
                IsPlayerInRange = true;
                break;
            }
        }
        if (IsPlayerInRange && tree.HasLogs) canBreakLog = true;
        else canBreakLog = false;

        animator.SetBool(animBreakLogString, canBreakLog);


    }
#pragma warning disable IDE0051 // Remove unused private members
    void BreakLog()
#pragma warning restore IDE0051 // Remove unused private members
    {
        tree.ShootLog();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(playerInRangeCheckPos.position, playerInRangeCheckRadius);
    }
}
