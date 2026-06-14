using UnityEngine;

public class PunchTrapDetector : MonoBehaviour
{
    private PunchTrap punchTrap;

    void Awake()
    {
        punchTrap = GetComponentInParent<PunchTrap>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>()!=null)
           punchTrap.StartExpansion();
       
    }
}
