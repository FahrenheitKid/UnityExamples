using System.Collections.Generic;
using Timers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI gameOverText;

    [SerializeField]
    private TextMeshProUGUI winText;

    [SerializeField]
    private Player player_Ref;

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

    bool twoPlayers = false;

    bool isMenu = false;

    // Start is called before the first frame update
    private void Start()
    {
        

       
            isMenu = SceneManager.GetActiveScene().name == "menu";
        
       
        

        if(!isMenu)
        {
            timerEnemySpawn = new Timer(1 / enemyPerSecond, Timer.INFINITE, spawnEnemy);
            TimersManager.SetTimer(this, timerEnemySpawn);

            if (!player_Ref || player_Ref == null)
            {
                player_Ref = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            }

        }
        
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
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


    public void teste()
    {
        twoPlayers = true;
    }
}