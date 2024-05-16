using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ME_Upgrade : MonoBehaviour
{
    public Weapon weapon = Weapon.None;
    public Effect effect = Effect.None;
    public string upgradeName = "";
    public string description = "";
    public bool repeatable = true;
    public bool available = true;

    public TextMeshProUGUI nameField;
    public TextMeshProUGUI descriptionField;

    private ME_Player player;
    private ME_Weapon weaponInstance;

    public enum Effect
    {
        None = 0,
        W_DamageUp = 1,
        W_ProjectileCountUp = 2,
        W_FireRateUp = 3,
        W_HitRateUp = 4,
        W_SpeedUp = 5,
        W_RotationSpeedUp = 6,
        W_LifespanUp = 7,
        W_SizeUp = 8
    }

    public enum Weapon
    {
        None = 0,
        PeaShooter = 1,
        CarrotSaber = 2,
        HotSauceTrailblazer = 3,
        VoltEel = 4
    }

    private void Start()
    {
        nameField.SetText(upgradeName);
        descriptionField.SetText(description);

        player = FindObjectOfType<ME_Player>();

        if (weapon != Weapon.None)
            weaponInstance = player.weapons[(int)weapon - 1];
    }

    public virtual void ApplyEffect()
    {
        switch (effect)
        {
            case Effect.W_DamageUp:
                weaponInstance.damage *= 1.2f;
                break;
            case Effect.W_ProjectileCountUp:
                weaponInstance.projectileCount += 1;
                break;
            case Effect.W_FireRateUp:
                weaponInstance.shotCooldown *= 0.8f;
                break;
            case Effect.W_HitRateUp:
                weaponInstance.damageCooldown *= 0.8f;
                break;
            case Effect.W_SpeedUp:
                weaponInstance.speed *= 1.2f;
                break;
            case Effect.W_RotationSpeedUp:
                weaponInstance.rotationSpeed *= 1.2f;
                break;
            case Effect.W_LifespanUp:
                weaponInstance.lifespan *= 1.2f;
                break;
            case Effect.W_SizeUp:
                weaponInstance.scale *= 1.2f;
                break;
        }

        if (weapon != Weapon.None)
            weaponInstance.ResetWeapon();

        if (!repeatable)
            available = false;
    }
}
