using UnityEngine;

public class ME_PeaShooter : ME_Weapon
{
    public GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        //weaponName = "Pea Shooter";

        //fireRate = 0.5f;
        //damage = 10f;
        //speed = 7f;
        //projectileCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    public override void SpawnProjectiles()
    {
        for (int i = 0; i < projectileCount; i++)
        {
            GameObject projectile = BuildProjectile(projectilePrefab);
            projectile.transform.rotation = Quaternion.Euler(0, 0, 360 / projectileCount * i);
        }
    }
}
