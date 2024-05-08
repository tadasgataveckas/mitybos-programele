using UnityEngine;

public class ME_HotSauceTrailblazer : ME_Weapon
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
        GameObject projectile = BuildProjectileFragile(projectilePrefab);
    }
}
