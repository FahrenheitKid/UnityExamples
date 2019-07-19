using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 1;

    [SerializeField]
    private Slider hpBar;

    [SerializeField]
     Game game_ref;

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
    }

    // Update is called once per frame
    private void Update()
    {
        // queremos mover apenas no x, então pegamos o input apenas horizontal
        Vector2 inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        inputDir = inputDir.normalized;
        Vector3 velocity = speed * inputDir * Time.deltaTime * inputDir.magnitude;
        transform.Translate(velocity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<Square>())
            {
                takeDamage(other.GetComponent<Square>().getStrength());
            }
            else if (other.GetComponent<Triangle>())
            {
            }

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