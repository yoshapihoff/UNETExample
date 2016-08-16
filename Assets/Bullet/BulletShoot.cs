using UnityEngine;
using System.Collections;

public class BulletShoot : MonoBehaviour
{
    [HideInInspector]
    public Transform ShootingPoint;
    [SerializeField]
    private Vector3 ScaleOnDisable;
    [SerializeField]
    private float Speed = 5f;
    [SerializeField]
    private float FinishTime = 3f;
    [SerializeField]
    private Transform MyTransform;
    private float StartTime;

    void OnEnable()
    {
        StartTime = Time.time;
    }

    void Update()
    {
        MyTransform.Translate(Vector3.forward * Speed * Time.deltaTime);
        if (Time.time > StartTime + FinishTime)
        {
            Reset();
        }
        else
        {
            MyTransform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }
    }

    public void Reset()
    {
        GetComponentInChildren<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        MyTransform.parent = ShootingPoint;
        MyTransform.localPosition = Vector3.zero;
        MyTransform.localRotation = Quaternion.identity;
        MyTransform.localScale = ScaleOnDisable;
        enabled = false;
    }
}
