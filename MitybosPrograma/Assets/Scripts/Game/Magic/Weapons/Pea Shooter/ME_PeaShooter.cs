using UnityEngine;

public class ME_PeaShooter : ME_Weapon
{
    public GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {

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
            GameObject projectile = BuildProjectileOnWorld(projectilePrefab);
            projectile.transform.rotation = Quaternion.Euler(0, 0, 360 / projectileCount * i);
        }
    }
}
