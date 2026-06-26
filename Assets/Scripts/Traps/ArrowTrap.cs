using System.Collections;

using UnityEngine;

public class ArrowTrap : Trap
{
    [SerializeField] BoxCollider2D Detector;
    [SerializeField] Arrow arrow;
    [SerializeField] Vector3 insOffset;

    private bool canShoot = true;
    private TrapDetector trapDetector;
    private Coroutine shootCo;

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

    private void ThrowArrow()
    {
        if (isActivated && canShoot && shootCo == null)
            shootCo = StartCoroutine(ThrowArrowCo());
    }
    private IEnumerator ThrowArrowCo()
    {
        Instantiate(arrow,transform.position + insOffset,transform.rotation);
        canShoot = false;
        yield return new WaitForSeconds(3);
        canShoot = true;
        shootCo = null;

       
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    protected override void AnimationController()
    {
        base.AnimationController();
        anim.SetBool("canShoot", canShoot);
    }
    

    public void ResetTrap()
    {
        isActivated = false;
        canActivated = true;
        trapDetector.activateTrap = false;
    }





}
