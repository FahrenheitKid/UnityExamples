using UnityEngine;

public class Square : Enemy
{
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Wall") && passWall)
        {
            passWall = false;
            
        }
        else if(collision.CompareTag("Wall") && !passWall)
        {
            Death(false);
        }
    }
}