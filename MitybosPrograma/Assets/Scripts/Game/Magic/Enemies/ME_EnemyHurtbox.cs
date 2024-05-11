using UnityEngine;

public class ME_EnemyHurtbox : MonoBehaviour
{
    public ME_Enemy enemy;
    public void DealDamageToEnemy(float damage)
    {
        enemy.TakeDamage(damage);
    }
}
