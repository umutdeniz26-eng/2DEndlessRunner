using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Player player;
    Rigidbody2D rb;

    void Start()
    {
        player = GetComponentInParent<Player>();
       
    }

    
    private void EndClimb()
    {
        player.FinishClimb();
    }



    private void EndRoll()
    {
        player.EndRoll();
    }
   

    private void EndKnockback()
    {
        player.EndKnockback();
    }


}
