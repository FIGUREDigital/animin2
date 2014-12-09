using UnityEngine;
using System.Collections;

public class GunGameEnemyScript : MonoBehaviour //Photon.MonoBehaviour
{

    public float Speed;
    public GameObject TargetToFollow;
    public int Level;
    public bool HasMerged;
    public GameObject SplatSetByCode;
    public Color SkinColor;
    public string BulletSplat;

	
    // Harry Start
    private bool m_Paused;

    public bool Paused
    {
        get { return m_Paused; }
        set
        {
            m_Paused = value;
            this.GetComponent<Animator>().enabled = !value;
        }
    }

    public bool PreventMerge;


    // SHAUN START
    // ---------------------------------------------------------------------------------------------------------------------------------------------------


    //public bool local { get { return __local;} }

    private Vector3 __networkPosition;
    private Quaternion __networkRotation;


    private void UpdatePositionRemotely(Vector3 position)
    {
        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 5);
    }

    private void UpdateRotationRemotely(Quaternion rotation)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 5);
    }

    // SHAUN END
    // ---------------------------------------------------------------------------------------------------------------------------------------------------



    // Use this for initialization
    void Start()
    {
        //int level = int.Parse(GetComponent<PhotonView>().instantiationData[0].ToString());
        //int textureIndex = int.Parse(GetComponent<PhotonView>().instantiationData[1].ToString());
        //Vector3 positino = (Vector3)GetComponent<PhotonView>().instantiationData[2];
        //		Debug.Log("RECEIVED level: " + level.ToString());
        //UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().SpawnEnemyEnd(this.gameObject, Level, 0, transform.position);

        //PreventMerge = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Paused)
            return;

        Vector3 pos = transform.position;
        pos.y = 0;
        transform.position = pos;



        /////


        //GameObject mainCharacter = UIGlobalVariablesScript.Singleton.MainCharacterRef;
        if (UIGlobalVariablesScript.Singleton.GunGameScene == null)
            return;
        GunsMinigameScript minigame = UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>();

        if (HasMerged)
        {
            // minigame.SpawnedObjects.Remove(this.gameObject);

            Destroy(this.gameObject);

            return;
        }

        Vector3 direction = Vector3.Normalize(TargetToFollow.transform.localPosition - this.transform.localPosition);
        this.transform.localPosition += direction * Speed * Time.deltaTime;

        Debug.Log("Enemy : [" + this.name + "]; Speed : [" + Speed + "];");


        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("EnemyGunGame");
        for (int i = 0; i < allEnemies.Length; ++i)
        {
            if (allEnemies[i] == this.gameObject)
                continue;
            GunGameEnemyScript enemyScript = allEnemies[i].GetComponent<GunGameEnemyScript>();
            if (enemyScript.Level != this.Level)
                continue;
            if (enemyScript.Level != 0)
                continue;
            if (enemyScript.HasMerged)
                continue;

            float radius = Vector3.Distance(allEnemies[i].transform.localPosition, this.gameObject.transform.localPosition);
                
            if (radius <= 0.06f)
            {
                Debug.Log("the enemy has been merged");
                GameObject newObject = minigame.SpawnEnemyStart(Level + 1);
                newObject.transform.localPosition = this.gameObject.transform.localPosition;


                HasMerged = true;
                enemyScript.HasMerged = true;

                //minigame.SpawnedObjects.Remove(this.gameObject);
                //Destroy(this.gameObject);

                //minigame.SpawnedObjects.Remove(allEnemies[i]);
                //Destroy(allEnemies[i]);

                ReceiveEventMerged();

            }

                    //if enemies are close and they are following the player...
                //else if (radius <= 0.6f && minigame.PlayersCharacters.Contains(TargetToFollow))
            else if (radius <= 0.6f)
            {
                Debug.Log("Target : [" + TargetToFollow.name + "];");

                if (PreventMerge)
                    continue;
                if (enemyScript.PreventMerge)
                    continue;

                this.TargetToFollow = allEnemies[i];
                enemyScript.TargetToFollow = this.gameObject;

                float newSpeed = Speed * 2f;
                Speed += newSpeed;
                enemyScript.Speed = newSpeed;

                PreventMerge = true;
                enemyScript.PreventMerge = true;
            }
            else
            {
                //this.TargetToFollow = minigame.PlayersCharacters[Random.Range(0, minigame.PlayersCharacters.Count)]; //Commented out. Should already be following the player...
            }
        }

        RotateToLookAtPoint(TargetToFollow.transform.position);

        UpdateRotationLookAt();


        //Being Foregiveness
        GameObject mainChara = UIGlobalVariablesScript.Singleton.MainCharacterRef;

        Vector3 charaPos = mainChara.transform.position;
        float arb = 10f;
        Vector3 dist = this.transform.position - charaPos;
        float dist_amt = (this.transform.position - charaPos).magnitude;
        if (dist.y > -0.5f)
        {
            Vector3 nearest = charaPos + ((this.collider.bounds.center - charaPos).normalized * Mathf.Min(mainChara.GetComponent<CharacterController>().radius * arb, dist_amt));
            bool contains = this.collider.bounds.Contains(nearest);
            if (contains)
            {
                //UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript> ().OnEvilCharacterHit (this.gameObject);
                UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().OnHitByEnemy(this.gameObject, mainChara);
            }
            Debug.DrawLine(charaPos, nearest, Color.red);
            Debug.DrawLine(charaPos, this.collider.bounds.center, Color.green);
        }
        //End Forgiveness

    }

    private void UpdateRotationLookAt()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, RotateDirectionLookAt, Time.deltaTime * 6);
    }

    public void ResetRotation()
    {
        RotateDirectionLookAt = transform.rotation;
    }

    private Quaternion RotateDirectionLookAt = Quaternion.Euler(0, 180, 0);

    public void RotateToLookAtPoint(Vector3 worldPoint)
    {
        RotateDirectionLookAt = Quaternion.LookRotation(Vector3.Normalize(worldPoint - transform.position));
    }

    void OnCollisionEnter(Collision collision)
    {
        GunsMinigameScript gunGame = UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>();

        // BARREL COLLIDED WITH BULLET
        if (collision.gameObject.tag == "Bullet")// || collision.gameObject.tag == "Player")
        {
            bool isRightColor = false;

            if (collision.gameObject.name.Contains("Green") && this.name.Contains("green"))
                isRightColor = true;

            if (collision.gameObject.name.Contains("Yellow") && this.name.Contains("yellow"))
                isRightColor = true;

            if (collision.gameObject.name.Contains("Blue") && this.name.Contains("blue"))
                isRightColor = true;

            if (collision.gameObject.name.Contains("Red") && this.name.Contains("red"))
                isRightColor = true;

            if (collision.gameObject.name.Contains("ALL"))
                isRightColor = true;


            // IF COLLIDED WITH THE RIGHT COLOR
            if (isRightColor)
            {
                if (TargetToFollow != null)
                {
                    Debug.Log("TargetToFollow != null: " + TargetToFollow.name);
                }

                if (TargetToFollow != null && !gunGame.PlayersCharacters.Contains(TargetToFollow))
                {
                    GunGameEnemyScript script = TargetToFollow.GetComponent<GunGameEnemyScript>();
                    if (script == null)
                        Debug.Log(TargetToFollow.name);

                    if (script.TargetToFollow == this.gameObject)
                    {
                        script.TargetToFollow = gunGame.PlayersCharacters[Random.Range(0, gunGame.PlayersCharacters.Count)];
                    }
                }
                gunGame.Points += 200;

                ReceiveEventMerged();
                Destroy(this.gameObject);

            }
        }
    }



    [RPC]
    protected void ReceiveEventMerged()
    {
        Debug.Log("RPC: ReceiveEventMerged");
        UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.GunGame_monsters_merge);
    }

    [RPC]
    protected void ReceiveEventBulletHitEnemy()
    {
        UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().Points += 200;// * (Level + 1);
        //UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().SpawnedObjects.Remove(this.gameObject);


        GameObject instance = (GameObject)Instantiate(SplatSetByCode);
        instance.transform.parent = UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().SpawnedObjectsAllOthers.transform;
        instance.transform.position = this.transform.position;
        instance.transform.rotation = Quaternion.Euler(instance.transform.rotation.eulerAngles.x, instance.transform.rotation.eulerAngles.y, Random.Range(0, 360));

        //UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().SpawnedObjects.Add(instance);

        for (int i = 0; i < 20; ++i)
        {
            UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().ShootEnemyDestroyedEffects(
                Random.Range(0.30f, 0.50f), this.gameObject.transform.localPosition, SkinColor, BulletSplat);
        }
    }
	
}
