using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Timers;

public class Game : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI gameOverText;
    [SerializeField]
    TextMeshProUGUI winText;
    [SerializeField]
    Player player_Ref;

    [SerializeField]
    GameObject square_Prefab;
    [SerializeField]
    GameObject triangle_Prefab;


    [SerializeField]
    List <Enemy> enemyList;

    [SerializeField]
    float enemyPerSecond = 0.5f;
    [SerializeField]
    float enemySpawnOffset = 20f;

    [SerializeField]
    Timer timerEnemySpawn;


    // Start is called before the first frame update
    void Start()
    {

        timerEnemySpawn = new Timer(1 / enemyPerSecond, Timer.INFINITE, spawnEnemy);
        TimersManager.SetTimer(this, timerEnemySpawn);

        if(!player_Ref || player_Ref == null)
        {
            player_Ref = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }


    }

    // Update is called once per frame
    void Update()
    {
        

        if(Input.GetKeyDown(KeyCode.R))
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

        if(isVertical)
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
            default:
                goto case 0;
                break;
        }

      

        
    }
}
