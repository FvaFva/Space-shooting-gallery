using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;

    public void Shot(Vector3 startPosition, Vector3 forward)
    {
        _rigidbody.position = startPosition;
        _rigidbody.velocity = forward * _speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider != null)
        {
            BaseHealth health = collision.collider.GetComponentInParent<BaseHealth>();

            if (health != null)
                health.TakeDamage(_damage);

            Destroy(gameObject);
        }
    }
}
