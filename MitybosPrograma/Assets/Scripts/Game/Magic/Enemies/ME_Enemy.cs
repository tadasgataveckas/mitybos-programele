using UnityEngine;

public class ME_Enemy : ME_Entity
{
    public float Xp = 10;
    public float Speed = 1f;
    public float Damage = 5f;
    public float DamageCooldown = 0.5f;

    public GameObject Target;
    public ME_Player Player;
    private Rigidbody2D RB;

    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;
        Target = GameObject.FindWithTag("Player");
        Player = Target.GetComponent<ME_Player>();
        RB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
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
        Player.GiveXp(Xp);
        Destroy(gameObject);
    }

    public override void UpdateHealthBar()
    {
        healthbar.UpdateSlider();
    }
}
