using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Player player;
    Rigidbody2D rb;

    void Start()
    {
        player = GetComponentInParent<Player>();
       
    }

    private void AfterClimb()
    {

        player.FinishClimb();
    }

    void Update()
    {
        
    }



}
