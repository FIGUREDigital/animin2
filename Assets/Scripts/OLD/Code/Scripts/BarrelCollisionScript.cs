using UnityEngine;
using System.Collections;

public class BarrelCollisionScript : MonoBehaviour
{
    public string[] BulletPrefabs;
    public Sprite BarFrontTexture;
    public Sprite BuletIcon;
    public string BarBackgroundTexture;
    public GameObject DestroyedPrefab;


    void Start()
    {
        UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().SpawnBarrelEnd(this.gameObject, this.transform.localPosition);

    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            ReceiveEventHitBarrel();
        }
    }

    protected void ReceiveEventHitBarrel()
    {
        Debug.Log("RPC: ReceiveEventHitBarrel");
        UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().OnBulletHitBarrel(this);
    }
}