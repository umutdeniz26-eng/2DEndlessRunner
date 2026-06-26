using UnityEngine;

public class DetectingTrap : Trap
{
    [SerializeField] BoxCollider2D Detector;
    private TrapDetector trapDetector;

    protected override void Awake()
    {
        base.Awake();
        trapDetector = GetComponentInChildren<TrapDetector>();
    }


    protected override void Update()
    {
        base.Update();
        isActivated = trapDetector.activateTrap;
        
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null && canActivated)
        {
            isActivated = true;


            player.TakeDamage();
            canActivated = false;
        }
    }

    public void ResetTrap()
    {
        isActivated = false;
            canActivated = true;
        trapDetector.activateTrap = false;
    }
            

            


}
