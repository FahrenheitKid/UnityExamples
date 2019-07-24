using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Timers;

public class Player : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField]
    private float speed = 1;

    [SerializeField]
    private float runSpeed = 10;

    [SerializeField]
    private float smoothSpeedVelocity;

    [SerializeField]
    private float smoothSpeedTime = 0.2f;

    private Vector2 velocity = Vector2.zero;

    [Header("Score Settings")]
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI multiplierText;

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
    float scoreTimer;
    [SerializeField]
    float scoreTimerMax = 5;
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
    private AudioSource hitSound;

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

        hitSound = GetComponent<AudioSource>();
        scoreTimer = Time.time;
        
        TimersManager.SetLoopableTimer(this, 1f, increaseScore);

    }

    // Update is called once per frame
    private void Update()
    {
        // queremos mover apenas no x, então pegamos o input apenas horizontal
        Vector2 inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        inputDir = inputDir.normalized;

        float targetSpeed = runSpeed * inputDir.magnitude;

        //atualizamos nossa velocidade atual com  a velocidade alvo suavizada
        speed = Mathf.SmoothDamp(speed, targetSpeed, ref smoothSpeedVelocity, smoothSpeedTime);
        velocity = speed * inputDir * Time.deltaTime * inputDir.magnitude;

        transform.Translate(velocity);


        ScoreTimer();
        fixCollision();
    }

    public void ScoreTimer()
    {
        if(Time.time >= scoreTimer+ scoreTimerMax)
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
            }
            else if (other.GetComponent<Triangle>())
            {
            }

            hitSound.Play();
            ScoreMultiplier = 0;
            scoreTimer = Time.time;
            Destroy(other.gameObject);
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