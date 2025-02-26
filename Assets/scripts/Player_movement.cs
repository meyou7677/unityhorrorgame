using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player_movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float groundDrag;
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

    // Update is called once per frame
    void Update()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);
        InputManager();
        SpeedControl();
        _rb.drag = _isGrounded ? groundDrag : 0f;
        
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
