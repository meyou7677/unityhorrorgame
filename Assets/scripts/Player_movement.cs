using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player_movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float groundDrag;
    [SerializeField] private float JumpForce;
    [Header("ground detection")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform orientation;
    private float _horizontalMovement;
    private float _verticalMovement;
    private bool _isGrounded;
    private Vector3 _moveDirection;
    private Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * playerHeight * 0.5f);
    }


    // Update is called once per frame


    void Update()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 10f, groundMask);
        Debug.Log(_isGrounded);
        InputManager();
        SpeedControl();
        _rb.drag = _isGrounded ? groundDrag : 0f;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_isGrounded)
            {
                _rb.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
            }
            
        }

        
    }
    private void InputManager()
    {
        _horizontalMovement = Input.GetAxisRaw("Horizontal");
        _verticalMovement = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        _moveDirection = orientation.forward * _verticalMovement + orientation.right * _horizontalMovement;
        _rb.AddForce(_moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void SpeedControl()
    {
        var velocity = _rb.velocity;
        var flatVelocity = new Vector3(velocity.x, 0f, velocity.z);
        if (!(flatVelocity.magnitude > moveSpeed)) return;
        var limitedVelocity = flatVelocity.normalized * moveSpeed;
        _rb.velocity = new Vector3 (limitedVelocity.x, _rb.velocity.y, limitedVelocity.z);
    }


}
