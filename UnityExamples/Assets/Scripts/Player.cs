using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float speed = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // queremos mover apenas no x, então pegamos o input apenas horizontal
        Vector2 inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        inputDir = inputDir.normalized;
        Vector3 velocity = speed * inputDir * Time.deltaTime * inputDir.magnitude;
        transform.Translate(velocity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("trigger collision");
        if(other.CompareTag("Enemy"))
        {

            Destroy(other.gameObject);
        }
    }

    
    
}
