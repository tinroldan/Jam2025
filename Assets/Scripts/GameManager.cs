using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CloseGame()
    {
        Application.Quit();
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
