using UnityEngine;
using static UnityEngine.AudioSettings;
using UnityEngine.UIElements;

public class ME_Player : ME_Entity
{
    //player stats
    public double FireRate = 1;
    public float Range = 1;

    private double HPRegenDelay = 3;
    private double ActiveHPRegenDelay = 1;
    private double HPRegenAmount = 1;

    private int[] weapons = new int[4];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    // Shoots
    public void Shoot()
    {

    }

    // Activates player level up function
    public void LevelUp()
    {

    }

    // Dashes past enemies while making player briefly invincible
    private void Dash()
    {

    }

    // Activates player level up function
    public void RegenerateHP()
    {
        //ActiveHPRegenDelay -= delta;
        //if (ActiveHPRegenDelay <= 0 && HP < MaxHP)
        //    HP += HPRegenAmount;
    }

    public void ResetHPRegenDelay()
    {
        ActiveHPRegenDelay = HPRegenDelay;
    }

    public override void Die()
    {

    }
}
