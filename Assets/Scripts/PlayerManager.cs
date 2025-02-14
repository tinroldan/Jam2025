using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [SerializeField]
    private List<PlayerMovement> playersOnGame;

    [SerializeField]
    private Color[] playerColors; //= new Color[4] { Color.blue, Color.yellow, Color.green, Color.red };

    [SerializeField]
    private Transform upperMapLimit;
    [SerializeField]
    private Transform lowerMapLimit;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        playersOnGame = new List<PlayerMovement>();
        playerColors = new Color[] { Color.blue, Color.yellow, Color.green, Color.red };

        //cam = GetComponent<Camera>();   
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (PlayerMovement player in playersOnGame)
        {
            if (player.transform.position.x < upperMapLimit.transform.position.x)
            {
                player.playerIndicator.transform.position = new Vector3(upperMapLimit.transform.position.x, upperMapLimit.transform.position.y, player.transform.position.z);
                player.playerIndicator.objectSprite.enabled = true;
            }
            else if (player.transform.position.x > lowerMapLimit.transform.position.x)
            {
                player.playerIndicator.transform.position = new Vector3(lowerMapLimit.transform.position.x - 3, lowerMapLimit.transform.position.y, player.transform.position.z);
                player.playerIndicator.objectSprite.enabled = true;
            }        
            else if (player.transform.position.z > upperMapLimit.transform.position.z)
            {
                player.playerIndicator.transform.position = new Vector3(player.transform.position.x, upperMapLimit.transform.position.y, upperMapLimit.transform.position.z);
                player.playerIndicator.objectSprite.enabled = true;
            }
            else if (player.transform.position.z < lowerMapLimit.transform.position.z)
            {
                player.playerIndicator.transform.position = new Vector3(player.transform.position.x, lowerMapLimit.transform.position.y, lowerMapLimit.transform.position.z);
                player.playerIndicator.objectSprite.enabled = true;
            }
            else
            {
                player.playerIndicator.objectSprite.enabled = false;
            }
        }
    }

    public void OnPlayerInitialized(PlayerMovement player)
    {
        playersOnGame.Add(player);
        player.playerId = playersOnGame.IndexOf(player);
        Debug.Log(player.playerId);
        player.playerColor = playerColors[player.playerId]; 
    }
}
