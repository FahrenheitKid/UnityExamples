using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Game : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI gameOverText;
    [SerializeField]
    TextMeshProUGUI winText;
    [SerializeField]
    Player player_Ref;

    // Start is called before the first frame update
    void Start()
    {
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
}
