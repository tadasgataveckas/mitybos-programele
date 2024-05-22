using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ME_PowerUpHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public ME_Upgrade upgrade;

    private Image panelImage;
    private Color original;

    public Image image;

    public TextMeshProUGUI nameField;
    public TextMeshProUGUI descriptionField;

    private ME_Player player;
    private ME_Game_Manager manager;
    private ME_Weapon weaponInstance;

    void Start()
    {

        // highlight
        panelImage = GetComponent<Image>();
        original = panelImage.color;

        // attribute assignment
        image.sprite = upgrade.sprite;
        image.color = upgrade.color;
        nameField.SetText(upgrade.upgradeName);
        descriptionField.SetText(upgrade.description);

        manager = FindObjectOfType<ME_Game_Manager>();
        player = FindObjectOfType<ME_Player>();

        if (upgrade.weapon != ME_Upgrade.Weapon.None)
            weaponInstance = player.weapons[(int)upgrade.weapon - 1];
    }

    public virtual void ApplyEffect()
    {
        switch (upgrade.effect)
        {
            // damage+
            case ME_Upgrade.Effect.W_DamageUp:
                weaponInstance.damage *= 1.2f;
                break;
            // projectile count+
            case ME_Upgrade.Effect.W_ProjectileCountUp:
                weaponInstance.projectileCount += 1;
                break;
            // fire rate+
            case ME_Upgrade.Effect.W_FireRateUp:
                weaponInstance.shotCooldown *= 0.8f;
                break;
            // hit rate+
            case ME_Upgrade.Effect.W_HitRateUp:
                weaponInstance.damageCooldown *= 0.8f;
                break;
            // projectile speed+
            case ME_Upgrade.Effect.W_SpeedUp:
                weaponInstance.speed *= 1.2f;
                break;
            // rotation speed+
            case ME_Upgrade.Effect.W_RotationSpeedUp:
                weaponInstance.rotationSpeed *= 1.2f;
                break;
            // life span+
            case ME_Upgrade.Effect.W_LifespanUp:
                weaponInstance.lifespan *= 1.2f;
                break;
            // size+
            case ME_Upgrade.Effect.W_SizeUp:
                weaponInstance.scale *= 1.2f;
                break;
            // give weapon
            case ME_Upgrade.Effect.GiveWeapon:
                weaponInstance.gameObject.SetActive(true);

                // add weapon upgrades to pool
                manager.upgradePool.AddRange(weaponInstance.upgrades);
                break;
        }

        if (upgrade.weapon != ME_Upgrade.Weapon.None)
            weaponInstance.ResetWeapon();

        if (!upgrade.repeatable)
            manager.reserveUpgradePool.Remove(upgrade);
    }



    public void SelectPowerUp()
    {
        manager.PickUpgrade(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        panelImage.color = new Color(1, 0.8f, 0);

    }

    public void OnPointerExit(PointerEventData eventData)
    {

        panelImage.color = original;

    }
}
