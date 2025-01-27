using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    [SerializeField] float waterLevelTimer = 15;
    [SerializeField] Vector2 waterTimerRange = new Vector2(7, 10);

    [SerializeField] private float maxWaterHeight = 1.2f;
    [SerializeField] private float floodingSpeed = 1.5f;

    private float initialWaterHeight;
    private float currentWaterHeight;
    private bool isWaterFull = false;
    private float waterFallTimer = 0;

    void Start()
    {
        //waterLevelTimer = 15;
        initialWaterHeight = transform.localPosition.y;
        currentWaterHeight = initialWaterHeight;
    }

    void Update()
    {
        waterLevelTimer -= Time.deltaTime;
        //Debug.Log(waterLevelTimer);
        if (waterLevelTimer <= 0 && !isWaterFull) //&& isWaterStill)
        {
            //Debug.Log("OMAGAAAAAA");
            RiseWaterLevel();
            //waterLevelTimer = Random.Range(waterTimerRange.x, waterTimerRange.y);
        }
        if (waterLevelTimer <= 0 && isWaterFull)
        {
            waterFallTimer += Time.deltaTime;
            if (waterFallTimer >= 1.5f)
            {
                FallWaterLevel();
                //waterTimer = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("PUATA");
            RiseWaterLevel();
        }
    }

    //void RiseWaterLevel()
    //{
    //    Debug.Log("MONDA");
    //    transform.position = new Vector3(transform.position.x, currentWaterHeight += (floodingSpeed * Time.deltaTime), transform.position.z);
    //    if (currentWaterHeight >= maxWaterHeight)
    //    {
    //        transform.position = new Vector3(transform.position.x, maxWaterHeight, transform.position.z);
    //        //FallWaterLevel();
    //        //waterLevelTimer = Random.Range(waterTimerRange.x, waterTimerRange.y);
    //        isWaterStill = true;
    //    }
    //    //Vector3.MoveTowards(gameObject.transform.position, new Vector3(transform.position.x, maxWaterHeight, transform.position.z), 3*Time.deltaTime);
    //}

    //void FallWaterLevel()
    //{
    //    float fallSpeed = floodingSpeed / 2;
    //    Debug.Log("COCK");
    //    transform.position = new Vector3(transform.position.x, currentWaterHeight -= (floodingSpeed * Time.deltaTime), transform.position.z);
    //    if (currentWaterHeight <= initialWaterHeight)
    //    {
    //        transform.position = new Vector3(transform.position.x, initialWaterHeight, transform.position.z);
    //        isWaterStill = false;
    //        waterLevelTimer = Random.Range(waterTimerRange.x, waterTimerRange.y);
    //    }
    //}
    void RiseWaterLevel()
    {
        //Debug.Log("MONDA");
        transform.localPosition = new Vector3(transform.localPosition.x, currentWaterHeight += (floodingSpeed * Time.deltaTime), transform.localPosition.z);
        if (currentWaterHeight >= maxWaterHeight)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, maxWaterHeight, transform.localPosition.z);
            //FallWaterLevel();
            //waterLevelTimer = Random.Range(waterTimerRange.x, waterTimerRange.y);
            isWaterFull = true;
            waterFallTimer = 0;
        }
        //Vector3.MoveTowards(gameObject.transform.position, new Vector3(transform.position.x, maxWaterHeight, transform.position.z), 3*Time.deltaTime);
    }

    void FallWaterLevel()
    {
        float fallSpeed = floodingSpeed / 2;
        //Debug.Log("COCK");
        transform.localPosition = new Vector3(transform.localPosition.x, currentWaterHeight -= (floodingSpeed * Time.deltaTime), transform.localPosition.z);
        if (currentWaterHeight <= initialWaterHeight)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, initialWaterHeight, transform.localPosition.z);
            isWaterFull = false;
            waterLevelTimer = Random.Range(waterTimerRange.x, waterTimerRange.y);
        }
    }
}
