using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Bird : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;

    public bool IsCollided { get; private set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IsCollided = true;
    }

    public void Push (Vector2 force)
    {
        _rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    public void ActivatePhysics()
    {
        _collider.enabled = true;
        _rigidbody.isKinematic = false;
    }

    public void DeactivatePhysics()
    {
        _collider.enabled = false;

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = 0;
        _rigidbody.isKinematic = true;
    }
}
