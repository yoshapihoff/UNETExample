using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class MovementSync : NetworkBehaviour
{
    [SyncVar (hook = "SyncPositionValues")]
    private Vector3 SyncPos;

    [SerializeField]
    private Transform MyTransform;
    [SerializeField]
    private bool UseHistoricalLerping = false;

    private float LerpRate;
    [SerializeField]
    private float NormalLerpRate = 16f;
    [SerializeField]
    private float FasterLerpRate = 27f;

    private Vector3 LastPos;
    [SerializeField]
    private float Threshold = 5f;

    private List<Vector3> SyncPosList = new List<Vector3>();
    [SerializeField]
    private float CloseEnough = 0.11f;

    void Start()
    {
        LerpRate = NormalLerpRate;
    }

    void Update()
    {
        LerpPosition();
    }

    void FixedUpdate()
    {
        TransmitPosition();
    }

    void LerpPosition()
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
    void CmdProvidePositionToServer(Vector3 pos)
    {
        SyncPos = pos;
    }

    [ClientCallback]
    void TransmitPosition()
    {
        if (isLocalPlayer && Vector3.Distance(MyTransform.position, LastPos) > Threshold)
        {
            CmdProvidePositionToServer(MyTransform.position);
            LastPos = MyTransform.position;
        }
    }

    void SyncPositionValues(Vector3 lastestPos)
    {
        SyncPos = lastestPos;
        SyncPosList.Add(SyncPos);
    }

    void OrdinaryLerping()
    {
        MyTransform.position = Vector3.Lerp(MyTransform.position, SyncPos, Time.deltaTime * LerpRate);
    }

    void HistoricalLerping()
    {
        if (SyncPosList.Count > 0)
        {
            MyTransform.position = Vector3.Lerp(MyTransform.position, SyncPosList[0], Time.deltaTime * LerpRate);
            if (Vector3.Distance(MyTransform.position, SyncPosList[0]) < CloseEnough)
            {
                SyncPosList.RemoveAt(0);
            }

            if (SyncPosList.Count > 10)
            {
                LerpRate = FasterLerpRate;
            }
            else
            {
                LerpRate = NormalLerpRate;
            }
        }
    }
 }       