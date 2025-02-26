using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float sensX = 100f;
    [SerializeField] private float sensY = 100f;

    [SerializeField] private Transform orientation;

    private float _xRotation;
    private float _yRotation;
    
    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        var mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensX;
        var mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensY;

        _yRotation += mouseX;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, min: -90f, max: 90f);

        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
        orientation.rotation = Quaternion.Euler(0f, _yRotation, 0f);
    }
}
