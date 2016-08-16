using UnityEngine;
using System.Collections;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField]
    private Transform MyTransform;
    [SerializeField]
    private GameObject MouseBox;

    void FixedUpdate()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseWorldPos, Camera.main.transform.forward, out hit))
        {
            Vector3 viewPos = new Vector3(hit.point.x, 0f, hit.point.z);
            Vector3 myPos = new Vector3(MyTransform.position.x, 0f, MyTransform.position.z);
            Quaternion lookRotation = Quaternion.LookRotation(viewPos - myPos);
            MyTransform.localRotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f);

            MouseBox.transform.position = viewPos;
        }
    }
}
