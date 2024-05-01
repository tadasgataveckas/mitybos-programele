using System.Collections.Generic;
using UnityEngine;

public class ME_CarrotSaber : ME_Weapon
{
    public GameObject projectilePrefab;
    private List<GameObject> swords = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        weaponName = "Carrot Saber";

        //fireRate = 0.2f;
        //damage = 10f;
        //speed = 180f;
        //projectileCount = 1;

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
            GameObject projectile = BuildProjectileStatic(projectilePrefab);
            swords.Add(projectile);
            projectile.transform.rotation = Quaternion.Euler(0, 0, 360 / projectileCount * i);
        }
    }

    public void DeleteProjectiles()
    {
        foreach (GameObject projectile in swords)
            Destroy(projectile);
        swords.Clear();
    }
}
