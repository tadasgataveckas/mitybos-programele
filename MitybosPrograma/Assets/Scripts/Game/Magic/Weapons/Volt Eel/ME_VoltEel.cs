using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ME_VoltEel : ME_Weapon
{
    public GameObject projectilePrefab;
    private List<GameObject> bolts = new List<GameObject>();

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
            bolts.Add(projectile);
            projectile.transform.rotation = Quaternion.Euler(0, 0, 360 / projectileCount * i - 90);
            StartCoroutine(StartPeriodicDisable(projectile));
        }
    }

    public void DeleteProjectiles()
    {
        foreach (GameObject projectile in bolts)
            Destroy(projectile);
        bolts.Clear();
    }

    public void ResetWeapon()
    {
        DeleteProjectiles();
        SpawnProjectiles();
    }
}
