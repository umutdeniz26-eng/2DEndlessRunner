using UnityEngine;

public class TrapDetector : MonoBehaviour
{

    [SerializeField] public bool activateTrap;



    

    private void OnTriggerEnter2D(Collider2D collision)
    {

        
        if (collision.GetComponent<Player>() != null)
            activateTrap = true;


    }

    
}
