using UnityEngine;
using UnityEngine.Networking;

public class PlayerShootSync : NetworkBehaviour
{
    private const int BULLETS_COUNT = 40;
    [SerializeField]
    private Transform ShootingPoint;
    [SerializeField]
    private GameObject BulletPrefab;
    [SerializeField]
    private float Interval = 0.5f;
    private float LastShootTime;
    private GameObject[] Bullets = new GameObject[BULLETS_COUNT];
    private Renderer[] Renderers = new Renderer[BULLETS_COUNT];
    private Collider[] Colliders = new Collider[BULLETS_COUNT];
    private BulletShoot[] BulletShoots = new BulletShoot[BULLETS_COUNT];
    private int BulletIndex = 0;

    void Start()
    {
        CreateBulles();
    }

    void Update()
    {
        if (isLocalPlayer && Input.GetAxis("Fire1") > 0f)
        {
            CmdFire();
        }
    }

    private void CreateBulles()
    {
        for (int i = 0; i < Bullets.Length; ++i)
        {
            Bullets[i] = Instantiate<GameObject>(BulletPrefab);

            var bullet = Bullets[i];

            bullet.transform.parent = ShootingPoint;
            bullet.transform.localPosition = Vector3.zero;

            Renderers[i] = bullet.GetComponentInChildren<Renderer>();
            BulletShoots[i] = bullet.GetComponent<BulletShoot>();
            Colliders[i] = bullet.GetComponent<Collider>();

            Renderers[i].enabled = false;
            BulletShoots[i].enabled = false;
            Colliders[i].enabled = false;
        }

    }

    [Command]
    private void CmdFire()
    {
        if (Time.time > LastShootTime + Interval)
        {
            RpcClientFire();
        }
    }

    [ClientRpc]
    private void RpcClientFire()
    {

        if (BulletIndex == Bullets.Length)
        {
            BulletIndex = 0;
        }
        Bullets[BulletIndex].transform.parent = null;

        Renderers[BulletIndex].enabled = true;
        BulletShoots[BulletIndex].enabled = true;
        Colliders[BulletIndex].enabled = true;

        BulletShoots[BulletIndex].ShootingPoint = ShootingPoint;
        ++BulletIndex;
        LastShootTime = Time.time;
    }
}