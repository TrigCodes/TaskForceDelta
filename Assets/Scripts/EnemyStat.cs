using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    public float Speed { get; set; }
    public int Damage { get; set; }
    public int Hitpoint { get; set; }

    public EnemyStat(float speed, int damage, int hitpoint) {
        Speed = speed;
        Damage = damage;
        Hitpoint = hitpoint;
    }
}
