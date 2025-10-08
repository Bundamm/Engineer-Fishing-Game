using UnityEngine;

public class Floater : MonoBehaviour
{
    private Water _water;

    [SerializeField]
    private float splashVelocity = 10;

    [SerializeField] 
    private float splashForce = 2;

    private Coroutine _waterCoroutine;
    private Collider2D _collider2D;
    private Rigidbody2D _rigidbody2D;
    private EdgeCollider2D _waterCollider2D;
    
    
    private bool _isInWater;
    
    private void Awake()
    {
        _water = FindAnyObjectByType<Water>();
        _waterCollider2D = _water.GetComponent<EdgeCollider2D>();
        _collider2D = GetComponent<Collider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_isInWater)
        {
            StickToSurface();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            if (other is EdgeCollider2D)
            {
                int multiplier = 1;
                if (_rigidbody2D.linearVelocity.y < 0)
                {
                    multiplier = -1;
                }
                else
                {
                    multiplier = 1;
                }

                float vel = _rigidbody2D.linearVelocity.y * _water.forceMultiplier;
                vel = Mathf.Clamp(Mathf.Abs(vel), 0f, _water.maxForce);
                vel *= multiplier;
                _water.Splash(_collider2D, vel);
                _isInWater = true;
            }

        }
    }

    public void SetIsInWater(bool isInWater)
    {
        _isInWater = isInWater;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            Physics2D.gravity = new Vector2(0, -9.81f);
            _rigidbody2D.gravityScale = 0.3f;
        }
    }

    private void StickToSurface()
    {
        Vector2 closestPoint = _waterCollider2D.ClosestPoint(_rigidbody2D.position);
        _rigidbody2D.gravityScale = 0;
        _rigidbody2D.MovePosition(Vector2.Lerp(_rigidbody2D.position, closestPoint, Time.fixedDeltaTime));
    }

}
