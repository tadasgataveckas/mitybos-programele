using UnityEngine;
using static UnityEngine.AudioSettings;
using UnityEngine.UIElements;

public class ME_Player : ME_Entity
{
    //player stats
    public double FireRate = 1;
    public float Range = 1;

    public double HPRegenDelay = 3;
    private double ActiveHPRegenDelay = 1;
    private double HPRegenAmount = 1;

    public GameObject Caster;
    public ME_Game_Manager Manager;

    private ME_Weapon[] weapons = new ME_Weapon[4];

    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;

    }

    // Update is called once per frame
    void Update()
    {
        healthbar.UpdateSlider();
    }

    // Activates player level up function
    public void LevelUp()
    {

    }

    // Dashes past enemies while making player briefly invincible
    private void Dash()
    {

    }

    public void GiveXp(float xp)
    {
        Manager.Score += xp;
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

    public override void UpdateHealthBar()
    {
        healthbar.UpdateSlider();
    }

    public override void Die()
    {
        Manager.TriggerGameEnd();
    }
}
