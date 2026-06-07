using UnityEngine;

public class LedgeCheck : MonoBehaviour
{
    [SerializeField] Player player;
    
    [SerializeField] public bool canClimb;
    void Start()
    {
        
    }

    
    void Update()
    {
        CanClimb();
    }

    public bool CanClimb()
    {
        if (player.isWall && !player.isLedge)
        {
            canClimb = true;
            return canClimb;

        }
        else
        {
            canClimb = false;
            return canClimb;

        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer==LayerMask.NameToLayer("Ground")) 
            player.isLedge = true;
    }
}
