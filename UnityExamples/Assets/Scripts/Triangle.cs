using UnityEngine;

public class Triangle : Enemy
{
    [SerializeField]
    private bool noMoreRetarget = false;
    [SerializeField]
    private float minDistanceToFollow = 2f;

    // Start is called before the first frame update
    private void Start()
    {
        Init();
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
    }

    private void LateUpdate()
    {
        //Tools.Utility.lookAt2D(transform, target, rotationOffset);
    }
    public override void Move()
    {

        Vector3 pivot = Camera.main.ScreenToWorldPoint(GetComponent<SpriteRenderer>().sprite.pivot);
        if (Vector2.Distance(transform.position, target.position) >
            minDistanceToFollow && !noMoreRetarget)
        {

            
            direction = target.position - transform.position;
            direction = direction.normalized;

            
        }
        else
        {
            //print("Distancia menor que" + GetComponent<BoxCollider2D>().size.magnitude);

            noMoreRetarget = true;
        }
        base.Move();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") && passWall)
        {
            passWall = false;

        }
        else if (collision.CompareTag("Wall") && !passWall)
        {
            Death(false);
        }
    }
}