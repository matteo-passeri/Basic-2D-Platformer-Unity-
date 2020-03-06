using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public float fireRate = 0;
    public int damage = 10;
    public LayerMask whatToHit;

    public Transform BulletTrailPrefab;
    public Transform MuzzleFlashPrefab;
    public Transform HitPrefab;
    float timeToSpawnEffect = 0;
    public float effectSpawnRate = 10;

    //Handle camera shaking
    public float cameraShakeAmt = 0.05f;
    public float camShakeLength = 0.1f;
    CameraShake camShake;

    public string weaponShootSound = "DefaultShot";

    float timeToFire = 0;
    Transform firePoint;

    // Cashing
    AudioManager audioManager;

    // for shooting
    Vector2 mousePosition;
    Vector2 firePointPosition;

    // Start is called before the first frame update
    void Awake()
    {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("No FirePoint!");
        }
    }

    void Start()
    {
        camShake = GameMaster.gm.GetComponent<CameraShake>();
        if (camShake == null) {
            Debug.LogError("No cameraShake scriupt on gm");
        }

        audioManager = AudioManager.instance;
        if(audioManager==null) {
            Debug.Log("WEAPON: No audioManager in scene");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if single bust
        if (fireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        // if automatic
        else
        {
            // if holding the button
            if (Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);

        // DrawLine is slightly out of grade
        Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 100, Color.cyan);

        if (hit.collider != null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);

            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(damage);
                // Debug.Log("We hit " + hit.collider.name + " and did " + damage + " damage");

            }
        }

        if (Time.time >= timeToSpawnEffect)
        {
            Vector3 hitPos;
            Vector3 hitNormal;

            if (hit.collider == null)
            {
                hitPos = (mousePosition - firePointPosition) * 30;
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else
            {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }

            Effect(hitPos, hitNormal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }

    void Effect(Vector3 hitPos, Vector3 hitNormal)
    {
        // calculate the angle & direction from firePoint to mouse
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //...

        Transform muzzleClone = Instantiate(MuzzleFlashPrefab, firePoint.position, rotation) as Transform;
        muzzleClone.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        muzzleClone.localScale = new Vector3(size, size, size);
        Destroy(muzzleClone.gameObject, 0.2f);

        Transform trailClone = Instantiate(BulletTrailPrefab, firePoint.position, rotation) as Transform;
        // LineRenderer lr = trailClone.GetComponent<LineRenderer>();

        // if (lr != null)
        // {
        // //     lr.SetPosition (0, firePoint.position);
        //     lr.SetPosition (1, hitPos);            

        // }
        Destroy(trailClone.gameObject, 0.2f);

        if (hitNormal != new Vector3(9999, 9999, 9999))
        {
            Transform hitParticle = Instantiate(HitPrefab, hitPos, Quaternion.FromToRotation(Vector3.forward, hitNormal)) as Transform;
            Destroy(hitParticle.gameObject, 1f);
        }

        // shake the camera
        camShake.Shake (cameraShakeAmt, camShakeLength);

        //Play shoot sound
        audioManager.PlaySound(weaponShootSound);
    }
}
