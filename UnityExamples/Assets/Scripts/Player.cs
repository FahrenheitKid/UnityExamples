using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private float runSpeed = 10;
    [SerializeField]
    private float smoothSpeedVelocity;
    [SerializeField]
    private float smoothSpeedTime = 0.2f;

    [SerializeField]
    private Slider hpBar;

    [SerializeField]
     Game game_ref;

    [SerializeField]
    CharacterController charController_ref;

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

        if(!charController_ref || charController_ref == null)
        {
            charController_ref = GetComponent<CharacterController>();
        }
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
        Vector3 velocity = speed * inputDir * Time.deltaTime * inputDir.magnitude;

        //print(velocity);
        //aqui efetuamos a movimentação chamando o método Move do Character controller
        // Na função passamos o vetor de movimentação (multiplicamos por Time.deltaTime pois como estamos na função Update, queremos mover o personagem só a quantidade necessário baseado no último frame)
        charController_ref.Move(velocity);
        //transform.Translate(velocity);
        //aqui atualizamos a velociade inicial com a velocidade interna do character controller que é mais precisa
        speed = new Vector2(charController_ref.velocity.x, charController_ref.velocity.z).magnitude;


        
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