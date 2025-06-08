using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 5f;

    private Rigidbody2D _rb;
    private Vector2 _moveDirection;

    void Start() {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        _moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate() {
        _rb.linearVelocity = _moveDirection * MoveSpeed;
    }
}
