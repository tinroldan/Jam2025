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

    public Transform dieZone;

    private bool canFight=false;


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
        canFight = true;
    }

    public void RevivePlayers()
    {
        StartCoroutine(LoopCoroutine());
    }

    IEnumerator LoopCoroutine()
    {
        for (int i = 0; i < playersOnGame.Count; i++) // Cambia 10 por el número de iteraciones deseado
        {
            playersOnGame[i].transform.position = transform.position;
            playersOnGame[i].ResetLive();
            yield return new WaitForSeconds(1f); // Espera 1 segundo antes de continuar
        }

        canFight = true;
    }



    public void CheckPlayers()
    {
        int playersAlive = 0;

        for (int i = 0; i < playersOnGame.Count; i++)
        {
            if (playersOnGame[i].ImDie==false)
            {
                playersAlive++;
            }
        }

        if(playersAlive <= 1&&playersOnGame.Count>1)
        {
            FinishRound();
        }
    }

    public void FinishRound()
    {
        PlayerMovement winPlayer = null;
        for (int i = 0; i < playersOnGame.Count; i++)
        {
            if (playersOnGame[i].ImDie == false)
            {
                winPlayer = playersOnGame[i];
                break;
            }
        }

        if(winPlayer != null)
        {
            winPlayer.SetPoints();
        }

        for (int i = 0; i < playersOnGame.Count; i++)
        {
            playersOnGame[i].UpdatePointsUI();
        }
        canFight = false;
        GameManager.Instance.ShowPoints();
        winPlayer.DieEvent();
    }

    public void ResetPlayers()
    {
        playersOnGame.Clear();
        canFight = true;
    }

    void Update()
    {

        if (canFight)
        {
            CheckPlayers();
        }

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
        //player.SetColor();
    }
}
