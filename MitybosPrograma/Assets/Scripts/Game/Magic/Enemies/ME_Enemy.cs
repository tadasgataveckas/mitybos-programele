using System.Collections;
using TMPro;
using UnityEngine;

public class ME_Enemy : ME_Entity
{
    public float Xp = 10;
    public float Speed = 1f;
    public float Damage = 5f;
    public float DamageCooldown = 0.5f;
    public TextMeshProUGUI DamageNumberPrefab;

    // target gets set in game manager
    [HideInInspector] public GameObject Target;
    [HideInInspector] public ME_Player Player;
    private Rigidbody2D RB;

    private void Awake()
    {
        HP = MaxHP;
        RB = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Player = Target.GetComponent<ME_Player>();
    }

    // Update is called once per frame
    void Update()
    {
        // added so collisions won't fuck up
        RB.velocity = Vector2.zero;

        // movement
        if (Target != null)
            MoveToTarget(Target.transform.position - transform.position);
    }

    private void MoveToTarget(Vector2 direction)
    {
        direction.Normalize();
        transform.Translate(direction * Speed * Time.deltaTime);
    }

    public override void Die()
    {
        if (isDead || this == null)
            return;

        isDead = true;
        Player.GiveXp(Xp);
        Destroy(gameObject);
    }

    public override void UpdateHealthBar()
    {
        healthbar.UpdateSlider();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        StartCoroutine(DisplayDamage(damage));
    }

    private IEnumerator DisplayDamage(float damage)
    {
        TextMeshProUGUI damageUI = Instantiate(DamageNumberPrefab, transform.position, transform.rotation);
        damageUI.SetText(damage.ToString());
        damageUI.transform.SetParent(healthbar.transform);
        yield return new WaitForSeconds(0.5f);
        Destroy(damageUI);
    }
}
