using System.Collections.Generic;
using Timers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI gameOverText;

    [SerializeField]
    private TextMeshProUGUI winText;

    [SerializeField]
    private Player[] players_Ref = new Player[2];

    [SerializeField]
    private GameObject square_Prefab;

    [SerializeField]
    private GameObject triangle_Prefab;

    [SerializeField]
    private List<Enemy> enemyList;

    [SerializeField]
    private float enemyPerSecond = 0.5f;

    [SerializeField]
    private float enemySpawnOffset = 20f;

    [SerializeField]
    private Timer timerEnemySpawn;

    [SerializeField]
    private bool twoPlayers = false;

    [SerializeField]
    private bool isMenu = false;

    [SerializeField]
    private Toggle togglePlayers;

    // Start is called before the first frame update
    private void Start()
    {
        isMenu = SceneManager.GetActiveScene().name == "Menu";

        if (!isMenu)
        {
            timerEnemySpawn = new Timer(1 / enemyPerSecond, Timer.INFINITE, spawnEnemy);
            TimersManager.SetTimer(this, timerEnemySpawn);

            GameObject[] aux = GameObject.FindGameObjectsWithTag("Player");
                
                for (int i = 0; i < aux.Length; i++)
                {
                    if (i >= players_Ref.Length) break;

                    players_Ref[i] = aux[i].GetComponent<Player>();
                }
               
                if(!twoPlayers)
                {
                    players_Ref[1].gameObject.SetActive(false);
                }
            
            if (!gameOverText || gameOverText == null)
            {
                gameOverText = GameObject.Find("Game Over Text").GetComponent<TextMeshProUGUI>();
                gameOverText.enabled = false;
            }

        }

        
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += this.OnLoadCallback;
    }

    void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
    {
        Start();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void GameOver()
    {
        gameOverText.enabled = true;
        gameOverText.gameObject.SetActive(true);
    }

    public void spawnEnemy()
    {
        float posX = 0;
        float posY = 0;

        // se o inimigo vai se mover majoritariamente na vertical
        bool isVertical = Random.value > 0.5f;

        if (isVertical)
        {
            if (Random.value > 0.5f) // spawn na parte de cima
            {
                posY = -enemySpawnOffset;
            }
            else // spawn na parte de baixo
            {
                posY = Screen.height + enemySpawnOffset;
            }

            posX = Random.Range(0, Screen.width);
        }
        else
        {
            if (Random.value > 0.5f) // spawn na parte de cima
            {
                posX = -enemySpawnOffset;
            }
            else // spawn na parte de baixo
            {
                posX = Screen.width + enemySpawnOffset;
            }

            posY = Random.Range(0, Screen.height);
        }

        int whichEnemy = Random.Range(0, 2);
        Vector2 spawnPos = new Vector2(posX, posY);
        spawnPos = Camera.main.ScreenToWorldPoint(spawnPos);
        switch (whichEnemy)
        {
            case 0:

                Square sq = Instantiate(square_Prefab, spawnPos, Quaternion.identity).GetComponent<Square>();
                enemyList.Add(sq);

                break;

            case 1:

                Triangle tr = Instantiate(triangle_Prefab, spawnPos, Quaternion.identity).GetComponent<Triangle>();
                enemyList.Add(tr);
                break;

            default:
                goto case 1;
                break;
        }
    }

    public void RemoveEnemyFromList(Enemy e)
    {
        enemyList.Remove(e);
    }

    public void loadGame()
    {
        SceneManager.LoadScene(1);
        
    }

    public void set2Players()
    {
        twoPlayers = togglePlayers.isOn;
        print("2Players = " + twoPlayers);
    }
}