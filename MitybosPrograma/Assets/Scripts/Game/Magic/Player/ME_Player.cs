using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class ME_Player : ME_Entity
{
    //player stats
    public double FireRate = 1;
    public float Range = 1;

    public int WeaponCapacity = 3;      // how many weapons can player carry
    public float HPRegenAmount = 0.1f;  // how much HP per frame
    public float HPRegenDelay = 3f;     // how long until HP begins to regen
    private float HPRegenDelayCounter = 0;
    private bool canRegen = true;

    [HideInInspector] public float xp = 0;
    [HideInInspector] public float xpMax = 100;
    [HideInInspector] public int Level = 1;

    public GameObject Caster;
    public ME_Game_Manager Manager;

    public List<ME_Weapon> weapons = new List<ME_Weapon>();

    private void Awake()
    {
        HP = MaxHP;
    }

    private void Start()
    {
        // initial slider update
        Manager.XpSlider.maxValue = xpMax;
        Manager.XpSlider.value = xp;
    }

    // Update is called once per frame
    void Update()
    {
        // regeneration logic
        HPRegenDelayCounter += Time.deltaTime;
        if (HPRegenDelayCounter > HPRegenDelay)
            canRegen = true;

        if (canRegen && HP < MaxHP)
            HP += HPRegenAmount;

        if (HP > MaxHP)
            HP = MaxHP;

        healthbar.UpdateSlider();
    }

    // Activates player level up function
    public void LevelUp()
    {
        Level += 1;
        Manager.xpLabel.SetText("Level: " + Level);
        Manager.TriggerLevelUp();
    }

    public void GiveXp(float xpAmount)
    {
        Manager.Score += xp;
        xp += xpAmount;

        if (xp >= xpMax)
        {
            // increase xp cap
            xpMax *= 1.2f;
            Manager.XpSlider.maxValue = xpMax;

            xp = 0;
            LevelUp();
        }
        Manager.XpSlider.value = xp;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        ResetHPRegenDelay();
    }

    public void ResetHPRegenDelay()
    {
        canRegen = false;
        HPRegenDelayCounter = 0;
    }

    public override void UpdateHealthBar()
    {
        healthbar.UpdateSlider();
    }

    public override void Die()
    {
        Manager.TriggerGameEnd();
    }

    public bool HasReachedWeaponCapacity()
    {
        int count = 0;
        foreach (ME_Weapon weapon in weapons)
            if (weapon.gameObject.activeSelf)
                count++;

        return count >= WeaponCapacity;
    }
}
