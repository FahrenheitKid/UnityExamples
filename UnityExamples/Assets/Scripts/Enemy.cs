using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    protected float speed;
    [SerializeField]
    protected Transform target;
    [SerializeField]
    protected Vector3 direction;
    [SerializeField]
    protected float strength;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Move()
    {
        
        Vector3 velocity = speed * direction * Time.deltaTime;
        transform.Translate(velocity);
    }


    public virtual void Init()
    {
        direction = target.position - transform.position;
        direction = direction.normalized;
    }
}
