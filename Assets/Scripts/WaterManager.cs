using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    [SerializeField] float waterLevelTimer;
    [SerializeField] Vector2 waterTimerRange;

    [SerializeField] float maxWaterHeight;
    [SerializeField] float floodingSpeed;

    private float initialWaterHeight;

    // Start is called before the first frame update
    void Start()
    {
        //waterLevelTimer = Random.Range(waterTimerRange.x, waterTimerRange.y);
        initialWaterHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        waterLevelTimer -= Time.deltaTime;
        //Debug.Log(waterLevelTimer);
        if (waterLevelTimer <= 0)
        {
            Debug.Log("OMAGAAAAAA");
            RiseWaterLevel();
            //waterLevelTimer = Random.Range(waterTimerRange.x, waterTimerRange.y);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("PUATA");
            RiseWaterLevel();
        }
    }

    void RiseWaterLevel()
    {
        Debug.Log("MONDA");
        transform.position = new Vector3(transform.position.x, initialWaterHeight + (floodingSpeed *= Time.deltaTime), transform.position.z);
        if (transform.position.y == maxWaterHeight)
        {
            transform.position = transform.position;
            FallWaterLevel();
            waterLevelTimer = Random.Range(waterTimerRange.x, waterTimerRange.y);
        }
        //Vector3.MoveTowards(gameObject.transform.position, new Vector3(transform.position.x, maxWaterHeight, transform.position.z), 3*Time.deltaTime);
    }

    void FallWaterLevel()
    {
        float fallSpeed = floodingSpeed / 2;
        Debug.Log("COCK");
        transform.position = new Vector3(transform.position.x, fallSpeed -= Time.deltaTime, transform.position.z);
        if (transform.position.y == initialWaterHeight)
        {
            transform.position = transform.position;
        }
    }
   // if (timer <= seconds) {
			//// basic timer
			//timer += Time.deltaTime;
   //         // percent is a 0-1 float showing the percentage of time that has passed on our timer!
			// percent = timer / seconds;
   //         // multiply the percentage to the difference of our two positions
			//// and add to the start
			//transform.position = start + Difference* percent;
}
