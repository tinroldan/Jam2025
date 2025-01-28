using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] GameObject playerPointsContainer;
    private List<PlayerPoints> allPlayers;

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

        allPlayers = new List<PlayerPoints>();
    }

    public void SetNewPlayer(PlayerPoints player)
    {
        allPlayers.Add(player);

        player.transform.SetParent(playerPointsContainer.transform);

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
