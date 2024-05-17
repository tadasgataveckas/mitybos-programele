using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "ME_Upgrade")]
public class ME_Upgrade : ScriptableObject
{
    public Sprite sprite;
    public Color color = Color.white;
    public Weapon weapon = Weapon.None;
    public Effect effect = Effect.None;
    public string upgradeName;
    public string description;
    public bool repeatable = true;

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
        W_SizeUp = 8,
        GiveWeapon = 9
    }

    public enum Weapon
    {
        None = 0,
        PeaShooter = 1,
        CarrotSaber = 2,
        HotSauceTrailblazer = 3,
        VoltEel = 4,
        MeateorBall = 5
    }
}
