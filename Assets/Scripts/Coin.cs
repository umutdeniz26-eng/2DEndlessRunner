using UnityEngine;

public class Coin : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
        GameManager.instance.coins++;
        Debug.Log("Parayi aldin");
        Destroy(gameObject);

        }
    }
}
