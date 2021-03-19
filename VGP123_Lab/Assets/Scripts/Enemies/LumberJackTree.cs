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


    bool hasLogs = false;
    public bool HasLogs
    {
        get { return hasLogs; }
    }
    float spawnTimer;

    public List<LumberJackLog> LogList
    {
        get { return logList; }
    }
    // Start is called before the first frame update
    void Start()
    {

        if (yShiftFromSpawn < 0) yShiftFromSpawn = 0;
        if (spawnPeriod < 0) spawnPeriod = 0;
        spawnTimer = spawnPeriod;
    }

    // Update is called once per frame
    void Update()
    {

        if (spawnTimer <= spawnPeriod) spawnTimer += Time.deltaTime;
        if (logList.Count < maxLogCount && spawnTimer >= spawnPeriod)
        {
            if (logList.Count == 0)
            {
                var cLog = Instantiate(log.gameObject, transform.position, Quaternion.identity);
                logList.Add(cLog.GetComponent<LumberJackLog>());
                cLog.transform.parent = transform;
            }
            else if (logList[logList.Count - 1].CanBeShot)
            {
                var cLog = Instantiate(log.gameObject, transform.position, Quaternion.identity);
                logList.Add(cLog.GetComponent<LumberJackLog>());
                cLog.transform.parent = transform;
            }

            spawnTimer = 0;
        }
        if (logList.Count > 0)
        {
            for (int i = 0; i < logList.Count; i++)
            {
                if (logList[i].Rb.isKinematic == true)
                {
                    var ogLogPos = logList[i].OgPos;
                    var shootPos = ogLogPos + new Vector3(0f, yShiftFromSpawn, 0f);
                    logList[i].transform.position = Vector3.MoveTowards(logList[i].transform.position, shootPos, spawnMoveSpeed * Time.deltaTime);
                    if (logList[i].transform.position == shootPos)
                    {
                        logList[i].Rb.isKinematic = false;
                        logList[i].CanBeShot = true;
                    }
                }
            }
            if (logList[0].CanBeShot == true)
                hasLogs = true;
        }
        else if (logList.Count <= 0) hasLogs = false;
    }

    public void ShootLog(int index)
    {
        spawnTimer = 0;
        if (logList[index].CanBeShot)
        {
            logList[index].IsShot = true;
            logList.RemoveAt(index);
        }
    }
}
