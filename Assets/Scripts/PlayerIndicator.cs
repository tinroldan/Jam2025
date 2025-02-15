using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicator : MonoBehaviour
{
    [SerializeField]
    public Color playerColor = new Color(0, 0, 0);

    //public Image obJectSprite;
    public Image objectSprite;

    private Camera mainCam; //= CameraManager.Instance.cam;
    private Vector3 screenPos = new Vector3(0, 0, 0);

    private void Awake()
    {
        objectSprite = GetComponent<Image>();
        
    }
    void Start()
    {
        //mainCam = CameraManager.Instance.cam;
        mainCam = Camera.main;
        //obJectSprite.tintColor = playerColor;
        //objectSprite.color = playerColor;   
    }

    void Update()
    {
        //screenPos = mainCam.WorldToScreenPoint(transform.position);
        screenPos = mainCam.ScreenToWorldPoint(transform.position);
        //transform.localPosition = screenPos;
        if (screenPos.x < -1 || screenPos.y < -1 || screenPos.x > 0 || screenPos.x > 0) {
         //Figure out if this is the best approach and test
        }
        //Debug.Log(screenPos);
    }
}
