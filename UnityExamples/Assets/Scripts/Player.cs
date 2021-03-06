﻿using Timers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [SerializeField]
    bool isPlayer1;

    [Header("Speed Settings")]
    [SerializeField]
    private float speed = 1;

    [SerializeField]
    private float runSpeed = 10;

    [SerializeField]
    private float smoothSpeedVelocity;

    [SerializeField]
    private float smoothSpeedTime = 0.2f;

    [SerializeField]
    private Vector2 inputDir = Vector2.zero;

    private Vector2 velocity = Vector2.zero;

    [Header("Score Settings")]
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI multiplierText;

    [SerializeField]
    private int _score = 0;  // Backing store

    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            if ((value <= 0))
            {
                _score = 0;
            }
            else _score = value;

            scoreText.text = "Score: " + _score;
        }
    }

    [SerializeField]
    private float scoreTimer;

    [SerializeField]
    private float scoreTimerMax = 5;

    [SerializeField]
    private float _scoreMultiplier = 1;  // Backing store

    public float ScoreMultiplier
    {
        get
        {
            return _scoreMultiplier;
        }
        set
        {
            if ((value <= 1))
            {
                _scoreMultiplier = 1;
                multiplierText.text = "";
            }
            else
            {
                _scoreMultiplier = value;
                multiplierText.text = _scoreMultiplier + "x";
            }
        }
    }

    [SerializeField]
    private Game game_ref;

    [SerializeField]
    private Rigidbody2D rigidbody_ref;

    [Header("Hp Settings")]
    [SerializeField]
    private Slider hpBar;

    [SerializeField]
    private int maxHp = 100;

    [SerializeField]
    private int _hp = 0;  // Backing store

    public int Hp
    {
        get
        {
            return _hp;
        }
        set
        {
            if ((value <= 0))
            {
                _hp = 0;
                Death();
            }
            else if (value > maxHp)
            {
                _hp = maxHp;
            }
            else _hp = value;

            hpBar.value = (float)_hp / (float)maxHp;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        Hp = maxHp;

        if (!game_ref || game_ref == null)
        {
            game_ref = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();
        }

        if (!rigidbody_ref || rigidbody_ref == null)
        {
            rigidbody_ref = GetComponent<Rigidbody2D>();
        }

        scoreTimer = Time.time;

        TimersManager.SetLoopableTimer(this, 1f, increaseScore);
    }

    // Update is called once per frame
    private void Update()
    {
        handleInput();

        float targetSpeed = runSpeed * inputDir.magnitude;

        //atualizamos nossa velocidade atual com  a velocidade alvo suavizada
        speed = Mathf.SmoothDamp(speed, targetSpeed, ref smoothSpeedVelocity, smoothSpeedTime);
        velocity = speed * inputDir * Time.deltaTime * inputDir.magnitude;

        transform.Translate(velocity);

        ScoreTimer();
        fixCollision();
    }

    public void handleInput()
    {

        if(isPlayer1)
        {
            inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        else
        {
            inputDir = new Vector2(Input.GetAxisRaw("Horizontal2"), Input.GetAxisRaw("Vertical2"));
        }
        

        //caso seja na plataforma android
#if UNITY_ANDROID

        if (Input.touches.Length > 0 && Input.touches.Length < 2)
        {
            if(Input.touches[0].phase == TouchPhase.Began || Input.touches[0].phase ==  TouchPhase.Stationary)
            {
                if(Input.touches[0].position.x < Screen.width / 2)
                {
                    inputDir = new Vector2(-1, 0);
                }
                else
                {
                    inputDir = new Vector2(1, 0);
                }
            }
            else if(Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                inputDir = Vector2.zero;
            }
        }

#endif
       



        inputDir = inputDir.normalized;
    }

    public void ScoreTimer()
    {
        if (Time.time >= scoreTimer + scoreTimerMax)
        {
            scoreTimer = Time.time;
            ScoreMultiplier++;
        }
    }

    public void increaseScore()
    {
        Score += (int)(1f * ScoreMultiplier);
    }

    public void fixCollision()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        // Retrieve all colliders we have intersected after velocity has been applied.
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

        foreach (Collider2D hit in hits)
        {
            // Ignore our own collider.
            if (hit == boxCollider)
                continue;

            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

            // Ensure that we are still overlapping this collider.
            // The overlap may no longer exist due to another intersected collider
            // pushing us out of this one.
            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);

                // If we intersect an object beneath us, set grounded to true.
                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0)
                {
                    //  grounded = true;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            print("Colidiu!");
            if (other.GetComponent<Square>())
            {
                takeDamage(other.GetComponent<Square>().getStrength());
                other.GetComponent<Square>().Death(true);
            }
            else if (other.GetComponent<Triangle>())
            {
                other.GetComponent<Triangle>().Death(true);
            }

            ScoreMultiplier = 0;
            scoreTimer = Time.time;
        }
    }

    public void takeDamage(int damage)
    {
        Hp -= damage;
    }

    public void Death()
    {
        game_ref.GameOver();
        print("Morreu");
    }
}