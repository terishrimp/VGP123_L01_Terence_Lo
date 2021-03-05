using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberJackTree : MonoBehaviour
{
    [SerializeField] LumberJackLog log = null;
    [SerializeField] int maxLogCount = 2;
    [SerializeField] float yShiftFromSpawn = 1f;
    [SerializeField] float spawnMoveSpeed = 50f;
    [SerializeField] float spawnPeriod = 1f;
    List<LumberJackLog> logList = new List<LumberJackLog>();

    float spawnTimer = 0;

    public List<LumberJackLog> LogList
    {
        get { return logList; }
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
        transform.parent = enemyListGo.transform;

        if (yShiftFromSpawn < 0)
        {
            yShiftFromSpawn = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && logList.Count > 0)
        {
            DebugShootLog();
        }
        if (spawnTimer <= spawnPeriod) spawnTimer += Time.deltaTime;
        if (logList.Count < maxLogCount && spawnTimer >= spawnPeriod)
        {
            if (logList.Count == 0)
            {
                var cLog = Instantiate(log.gameObject, transform.position, Quaternion.identity);
                logList.Add(cLog.GetComponent<LumberJackLog>());
                cLog.transform.parent = transform;
                
            }
            else
            {
                if (logList[logList.Count - 1].CanBeShot)
                {
                    var cLog = Instantiate(log.gameObject, logList[logList.Count - 1].transform.position, Quaternion.identity);
                    logList.Add(cLog.GetComponent<LumberJackLog>());
                    cLog.transform.parent = transform;
                    
                }

            }
        }
        if (logList.Count != 0)
        {
            for (int i = 0; i < logList.Count; i++)
            {
                if (logList[i].Rb.isKinematic == true)
                {
                    var ogLogPos = logList[i].OgPos;
                    var shootPos = ogLogPos + new Vector3(0f, yShiftFromSpawn, 0f);
                    Debug.Log(ogLogPos);
                    Debug.Log(shootPos);
                    logList[i].transform.position = Vector3.MoveTowards(logList[i].transform.position, shootPos, spawnMoveSpeed * Time.deltaTime);
                    if (logList[i].transform.position == shootPos)
                    {
                        logList[i].Rb.simulated = true;
                        logList[i].Rb.isKinematic = false;
                        logList[i].CanBeShot = true;
                    }
                }
            }
        }
    }

    public void DebugShootLog()
    {
        spawnTimer = 0;
        if (logList[0].CanBeShot) { 
        logList[0].IsShot = true;
        logList.RemoveAt(0);
        }

        //var culledList = logList.FindAll(s => s.gameObject != null);
        //for (int i = 0; i < culledList.Count; i++)
        //{
        //    culledList[i].Rb.isKinematic = false;
        //}
    }
}
