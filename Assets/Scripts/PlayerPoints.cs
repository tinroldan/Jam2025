using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPoints : MonoBehaviour
{
    [SerializeField] List<GameObject> pointsArray = new List<GameObject>();
    [SerializeField] Image playerProfile;
    public void UpdatePoints(int points)
    {

        for (int i = 0; i < pointsArray.Count; i++)
        {
            pointsArray[i].SetActive(false);
        }

        for (int i = 0; i < pointsArray.Count; i++)
        {
            if(i+1<= points)
            {
                pointsArray[i].SetActive(true);
            }
        }
    }

    public void SetProfile(Color color)
    {
        playerProfile.color = color;

    }
}
