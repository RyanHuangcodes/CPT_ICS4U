using UnityEngine;
//gpt
public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 5f;

    private Rigidbody2D _rb;
    private Vector2 _moveDirection;

    private Animator _anim;
    private SpriteRenderer _spriteRenderer;
    private bool facingRight = true;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        _moveDirection = new Vector2(moveX, moveY).normalized;

        bool isRunning = _moveDirection.magnitude > 0;
        _anim.SetBool("isRunning", isRunning);
        if (moveX > 0)
        {
            facingRight = true;
            _spriteRenderer.flipX = false;
        }
        else if (moveX < 0)
        {
            facingRight = false;
            _spriteRenderer.flipX = true;
        }

        transform.rotation = Quaternion.identity;
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = _moveDirection * MoveSpeed;
    }
}
