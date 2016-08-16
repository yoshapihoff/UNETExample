using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class PlayerRotationSync : NetworkBehaviour
{
    [SyncVar(hook = "SyncRotationValues")]
    private float SyncRotation;

    [SerializeField]
    private Transform MyTransform;
    [SerializeField]
    private bool UseHistoricalLerping = false;

    [SerializeField]
    private float LerpRate = 15f;
    private float LastRotation;
    [SerializeField]
    private float Threshold = 1f;

    private List<float> SyncRotationList = new List<float>();
    [SerializeField]
    private float CloseEnough = 0.5f;

    void Update()
    {
        LerpRotation();
    }

    void FixedUpdate()
    {
        TransmitRotation();
    }

    void LerpRotation()
    {
        if (!isLocalPlayer)
        {
            if (UseHistoricalLerping)
            {
                HistoricalLerping();
            }
            else
            {
                OrdinaryLerping();
            }
        }
    }

    [Command]
    void CmdProvideRotationToServer(float rotation)
    {
        SyncRotation = rotation;
    }

    [ClientCallback]
    void TransmitRotation()
    {
        if (isLocalPlayer)
        {
            if (CheckIfBeyondThreshold(MyTransform.localEulerAngles.y, LastRotation))
            {
                LastRotation = MyTransform.localEulerAngles.y;
                CmdProvideRotationToServer(LastRotation);
            }
        }
    }

    bool CheckIfBeyondThreshold(float rot1, float rot2)
    {
        return Mathf.Abs(rot1 - rot2) > Threshold;
    }

    void SyncRotationValues(float lastestRotation)
    {
        SyncRotation = lastestRotation;
        SyncRotationList.Add(SyncRotation);
    }

    void LerpRotation(float newRotation)
    {
        MyTransform.rotation = Quaternion.Lerp(MyTransform.rotation, Quaternion.Euler(0f, newRotation, 0f), Time.deltaTime * LerpRate);
    }

    void OrdinaryLerping()
    {
        LerpRotation(SyncRotation);
    }

    void HistoricalLerping()
    {
        if (SyncRotationList.Count > 0)
        {
            LerpRotation(SyncRotationList[0]);
            if (Mathf.Abs(MyTransform.localEulerAngles.y - SyncRotationList[0]) < CloseEnough)
            {
                SyncRotationList.RemoveAt(0);
            }
        }
    }
}
