using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    protected float speed;

    [SerializeField]
    protected Transform target;

    [SerializeField]
    protected Vector3 direction;
    [SerializeField]
    protected float rotationOffset;

    [SerializeField]
    protected int strength;

    [SerializeField]
    protected AudioSource hitsound;

    [SerializeField]
    protected Game game_ref;

    [SerializeField]
    protected bool passWall = true;

    // Start is called before the first frame update
    private void Start()
    {
        Init();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public virtual void Move()
    {
        Vector3 velocity = speed * direction * Time.deltaTime;
        transform.Translate(velocity);
    }

    public virtual void Init()
    {
        if(!target || target == null)
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (!game_ref || game_ref == null)
            game_ref = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();

        direction = target.position - transform.position;
        direction = direction.normalized;

        if (!hitsound || hitsound == null)
            hitsound = GetComponent<AudioSource>();
    }

    public int getStrength()
    {
        return strength;
    }

    public virtual void Death(bool playHitSound)
    {
        print("executou");

        if (!hitsound.isPlaying && playHitSound)
            hitsound.Play();
        if(GetComponent<BoxCollider2D>())
        GetComponent<BoxCollider2D>().enabled = false;
        else if (GetComponent<PolygonCollider2D>())
        GetComponent<PolygonCollider2D>().enabled = false;

        if(GetComponent<SpriteRenderer>())
        GetComponent<SpriteRenderer>().enabled = false;
        

        game_ref.RemoveEnemyFromList(this);
        if (playHitSound)
            Destroy(gameObject, hitsound.clip.length + 0.1f);
        else Destroy(gameObject);
    }
}