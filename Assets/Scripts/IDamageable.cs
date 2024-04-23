using UnityEngine;

public interface IDamageable {
    public float Health { get; set; }
    public void OnHit(int damage);
    public void OnObjectDestroy();
}