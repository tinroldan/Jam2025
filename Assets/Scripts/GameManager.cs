using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] GameObject playerPointsContainer;
    private List<PlayerPoints> allPlayers;
    private List<PlayerIndicator> playerIndicators;

    [SerializeField] GameObject playerWin;
    [SerializeField] Image playerWinProfile;

    public static event Action<int> OnPlayerDead;

    [SerializeField]
    private GameObject playerPointscanvas;

    private bool someOneWon=false;
    private Color playerWonColor;

    private PlayerMovement currentWonPlayer = new PlayerMovement();
    public static void TriggerEvent(int value)
    {
        OnPlayerDead?.Invoke(value); 
    }

    public void IWon(PlayerMovement player)
    {
        someOneWon=true;
        playerWonColor = player.playerColor;
        playerWinProfile.color = playerWonColor;
        currentWonPlayer = player;
        player.DieEvent();
    }

    public void SomeOneWin()
    {
        playerWinProfile.color = playerWonColor;
        playerWin.SetActive(true);
    }

    public void ShowPoints()
    {
        playerPointscanvas.SetActive(true);
    }

    public void Continue()
    {
        playerPointscanvas.SetActive(false);
        
        if (!someOneWon)
        {
            //currentWonPlayer.DieEvent();
            PlayerManager.Instance.RevivePlayers();
        }
        else
        {
            
            SomeOneWin();
            someOneWon = false;
            
            
        }
    }

    public void PlayerDead()
    {

    }

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

    public void SetNewPlayer(PlayerPoints player)//, PlayerIndicator playerIndicator)
    {
        allPlayers.Add(player);
        //playerIndicators.Add(playerIndicator);

        player.transform.SetParent(playerPointsContainer.transform);
        player.transform.localScale = Vector3.one;
        //playerIndicator.transform.SetParent(gameObject.transform.GetChild(0).transform);    

    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        PlayerManager.Instance.ResetPlayers();
        playerWin.SetActive(false);
        foreach (var item in allPlayers)
        {
            Destroy(item.gameObject);
        }
        allPlayers.Clear();
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
