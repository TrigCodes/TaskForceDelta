using UnityEngine;

public class TurretStat : MonoBehaviour
{
    public int Cost { get; set; }
    public int Hitpoint { get; set; }
    public int Shield { get; set; }
    public int Damage { get; set; }
    public float FireRate { get; set; }

    public TurretStat(int cost, int hitpoint, int shield, int damage, float fireRate)
    {
        Cost = cost;
        Hitpoint = hitpoint;
        Shield = shield;
        Damage = damage;
        FireRate = fireRate;
    }
}