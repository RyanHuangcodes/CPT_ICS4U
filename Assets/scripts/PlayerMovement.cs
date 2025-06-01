using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float _moveSpeed = 10f;
    private Rigidbody2D myBody; 
    private float _movementX;
    private float _movementY;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
