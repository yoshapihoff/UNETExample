using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerHealthSync : NetworkBehaviour
{
    [SerializeField]
    private float MaxHealth;
    private Text MyHealthText;
    [SerializeField]
    private float Damage = 1f;

    [SyncVar (hook ="OnHealthChanged")]
    private float SyncHealth;

    void Start()
    {
        MyHealthText = GameObject.Find("MyHealth").GetComponent<Text>();
        SyncHealth = MaxHealth;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            CmdProvideDamageToServer();
            other.GetComponent<BulletShoot>().Reset();
        }
    }

    void OnHealthChanged(float newHealth)
    {
        if (isLocalPlayer)
        {
            MyHealthText.text = "HEALTH: " + newHealth;
        }
    }

    [Command]
    void CmdProvideDamageToServer()
    {
        SyncHealth -= Damage;
    }
}
