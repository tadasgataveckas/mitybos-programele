using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ME_MeateorBall : ME_Weapon
{
    public GameObject projectilePrefab;
    private List<GameObject> swords = new List<GameObject>();
    //private Camera mainCamera = Camera.main;

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
        GameObject projectile = BuildProjectileOnWorld(projectilePrefab);

        Vector2 screenPosition = new Vector2(Random.Range(0, Screen.width), Screen.height);
        Vector2 newPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        newPosition.y += 1f;

        projectile.transform.position = newPosition;
        projectile.transform.rotation = Quaternion.Euler(0, 0, -90);

    }
}
