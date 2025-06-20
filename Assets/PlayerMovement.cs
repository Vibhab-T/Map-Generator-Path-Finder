using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody rb;
    public float moveSpeed;
    private Vector3 _moveDirection;

    public InputActionReference driving;

    private void OnEnable()
    {
        driving.action.Enable();
    }

    private void OnDisable()
    {
        driving.action.Disable();
    }


    private void Update()
    {
        Vector2 input = driving.action.ReadValue<Vector2>();
        _moveDirection = new Vector3(input.x, 0, input.y);
        Debug.Log($"Input: {_moveDirection}");

    }
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed, _moveDirection.z * moveSpeed);

    }
}
