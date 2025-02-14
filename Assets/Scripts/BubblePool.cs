using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePool : MonoBehaviour
{
    public static object instance;

    private List<BoostBubble> pooledBubbles = new List<BoostBubble>();
    [SerializeField] private int poolSize = 20;

    [SerializeField] private BoostBubble boostBubble;
    [SerializeField] private Transform spawnRangeUp;
    [SerializeField] private Transform spawnRangeDown;

    [SerializeField] private float cooldownRate;
    [SerializeField] private float cooldownValue;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            BoostBubble bubb = Instantiate(boostBubble);
            //obj.SetActive(false);
            bubb.gameObject.SetActive(false);
            pooledBubbles.Add(bubb);
        }
        cooldownValue = cooldownRate;
    }
    // Update is called once per frame
    void Update()
    {
        cooldownValue -= Time.deltaTime;
        //Debug.Log(cooldownValue);
        if (cooldownValue <= 0)
        {
            SpawnBubble();
            cooldownValue = cooldownRate;
            //Debug.LogWarning("CUMMMMMM");
        }
    }

    //public BoostBubble GetPooledBubble()
    void SpawnBubble()
    {
        foreach (BoostBubble bubb in pooledBubbles)
        {
            if (bubb.gameObject.activeSelf == false && bubb.GetLastPlayer() == null)
            {
                float spawnX = Random.Range(spawnRangeUp.position.x, spawnRangeDown.position.x);
                float spawnZ = Random.Range(spawnRangeUp.position.z, spawnRangeDown.position.z);
                bubb.transform.position = new Vector3(spawnX, 0.8f, spawnZ);
                bubb.gameObject.SetActive(true);
                bubb.initialHeight = bubb.transform.position.y;
                //pooledBubbles[i].gameObject.transform.position.x = Random.Range(spawnRangeUp.position.x, spawnRangeDown.position.x);
                //return pooledBubbles[i];
                break;
            }
        }
        //for (int i = 0; i < pooledBubbles.Count; i++)
        //{
        //    if (pooledBubbles[i].gameObject.activeSelf == false && pooledBubbles[i].GetLastPlayer() == null)
        //    {
        //        float spawnX = Random.Range(spawnRangeUp.position.x, spawnRangeDown.position.x);
        //        float spawnZ = Random.Range(spawnRangeUp.position.z, spawnRangeDown.position.z);
        //        pooledBubbles[i].transform.position = new Vector3 (spawnX, 0.8f, spawnZ);
        //        pooledBubbles[i].gameObject.SetActive(true);
        //        //pooledBubbles[i].gameObject.transform.position.x = Random.Range(spawnRangeUp.position.x, spawnRangeDown.position.x);
        //        //return pooledBubbles[i];
        //    }
        //}
        //return null;
    }
}
