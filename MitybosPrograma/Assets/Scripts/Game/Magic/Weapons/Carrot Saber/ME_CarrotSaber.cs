using System.Collections.Generic;
using UnityEngine;

public class ME_CarrotSaber : ME_Weapon
{
    public GameObject projectilePrefab;
    private List<GameObject> swords = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        SpawnProjectiles();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void SpawnProjectiles()
    {
        for (int i = 0; i < projectileCount; i++)
        {
            GameObject projectile = BuildProjectileOnPlayer(projectilePrefab);
            swords.Add(projectile);

            float angle = i * (360f / projectileCount);
            projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void DeleteProjectiles()
    {
        foreach (GameObject projectile in swords)
            Destroy(projectile);
        swords.Clear();
    }

    public override void ResetWeapon()
    {
        DeleteProjectiles();
        SpawnProjectiles();
    }
}
