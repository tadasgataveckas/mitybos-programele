using System.Collections;
using TMPro;
using UnityEngine;

public class ME_Enemy : ME_Entity
{
    public float Xp = 10;
    public float Speed = 1f;
    public float Damage = 5f;
    public float DamageCooldown = 0.5f;

    public GameObject DamageNumberPrefab;
    public ParticleSystem DeathEffectPrefab;

    // player gets set in game manager
    [HideInInspector] public ME_Player Player;
    private Rigidbody2D RB;
    private SpriteRenderer SR;

    private void Awake()
    {
        HP = MaxHP;
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // added so collisions won't fuck up
        RB.velocity = Vector2.zero;

        // movement
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        if (Player != null)
        {
            Vector2 direction = Player.transform.position - transform.position;
            SR.flipX = direction.x > 0;
            direction.Normalize();
            transform.Translate(direction * Speed * Time.deltaTime);
        }
    }

    public override void Die()
    {
        if (isDead || this == null)
            return;

        isDead = true;
        Player.GiveXp(Xp);


        ParticleSystem fx = Instantiate(DeathEffectPrefab, transform.position, transform.rotation);
        fx.transform.localScale = new Vector2(transform.localScale.x * fx.transform.localScale.x,
           transform.localScale.y * fx.transform.localScale.y);
        Destroy(gameObject);
    }

    public override void UpdateHealthBar()
    {
        healthbar.UpdateSlider();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        DisplayDamage(damage);
    }

    private void DisplayDamage(float damage)
    {
        GameObject damageUI = Instantiate(DamageNumberPrefab, transform.position, transform.rotation);
        damageUI.GetComponent<ME_DamageNumber>().DisplayDamage(damage);
    }
}
