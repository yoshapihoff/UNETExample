using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Transform MyTransform;

    [SerializeField]
    private float Speed;

    void Update()
    {
        Vector3 movementVector = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        movementVector *= Speed * Time.deltaTime;
        MyTransform.Translate(movementVector);
    }
}
