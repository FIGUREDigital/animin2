using UnityEngine;
using System.Collections;

public enum EmotionId
{
	Default = 0,
	Blink,
	Happy1,
	Happy2,
	Happy3,
	Happy4,
	Happy5,
	Happy6,
	Count,

}

public class EmotionPerModelData
{
	public string[] TexturePaths;
}

public class CharacterSwapManagementScript : MonoBehaviour 
{
	//private AnimationClip[,] AnimationsPerModel;
    private string[,] Models;
    private string[] EggModels;
    public GameObject CurrentModel;
	public Vector3 defaultScale;
    public GameObject CurrentEgg;
    public Texture2D CurrentEyesOpenTexture;

	public AnimatorOverrideController TBOAdultAnimations;
    public AnimatorOverrideController[,] AnimationLists;

    public Texture2D[,] Normals;
    public Texture2D[,] Blinks;


	void Awake()
	{
		Debug.Log ("CharacterSwapManagementScript AWAKE");
		//AnimationsPerModel = new AnimationClip[(int)CreatureTypeId.Count, (int)CreatureAnimationId.Count];

		//LoadAnimations(CreatureTypeId.TBOAdult, "Models/TBO/Adult", "/tbo_adult@");
		//LoadAnimations(CreatureTypeId.TBOAdult, "Models/TBO/Kid", "/tbo_kid@");

        EggModels = new string[(int)PersistentData.TypesOfAnimin.Count];
        EggModels[(int)PersistentData.TypesOfAnimin.Tbo] = "Prefabs/Characters/tbo_egg";
		EggModels[(int)PersistentData.TypesOfAnimin.Kelsey] = "Prefabs/Characters/tbo_egg";	// All use tbo's egg and change it's color
		EggModels[(int)PersistentData.TypesOfAnimin.Mandi] = "Prefabs/Characters/tbo_egg"; // All use tbo's egg and change it's color
		EggModels[(int)PersistentData.TypesOfAnimin.Pi] = "Prefabs/Characters/tbo_egg"; // All use tbo's egg and change it's color
	
        Models = new string[(int)PersistentData.TypesOfAnimin.Count, (int)AniminEvolutionStageId.Count];
        Models[(int)PersistentData.TypesOfAnimin.Tbo, (int)AniminEvolutionStageId.Baby] = "Prefabs/Characters/tbo_baby";
        Models[(int)PersistentData.TypesOfAnimin.Tbo, (int)AniminEvolutionStageId.Kid] = "Prefabs/Characters/tbo_kid";
        Models[(int)PersistentData.TypesOfAnimin.Tbo, (int)AniminEvolutionStageId.Adult] = "Prefabs/Characters/tbo_adult";

        Models[(int)PersistentData.TypesOfAnimin.Kelsey, (int)AniminEvolutionStageId.Baby] = "Prefabs/Characters/ke_baby";
        Models[(int)PersistentData.TypesOfAnimin.Kelsey, (int)AniminEvolutionStageId.Kid] = "Prefabs/Characters/ke_kid";
        Models[(int)PersistentData.TypesOfAnimin.Kelsey, (int)AniminEvolutionStageId.Adult] = "Prefabs/Characters/ke_adult";

        Models[(int)PersistentData.TypesOfAnimin.Mandi, (int)AniminEvolutionStageId.Baby] = "Prefabs/Characters/ma_baby";
        Models[(int)PersistentData.TypesOfAnimin.Mandi, (int)AniminEvolutionStageId.Kid] = "Prefabs/Characters/ma_kid";
        Models[(int)PersistentData.TypesOfAnimin.Mandi, (int)AniminEvolutionStageId.Adult] = "Prefabs/Characters/ma_adult";

        Models[(int)PersistentData.TypesOfAnimin.Pi, (int)AniminEvolutionStageId.Baby] = "Prefabs/Characters/pi_baby";
        Models[(int)PersistentData.TypesOfAnimin.Pi, (int)AniminEvolutionStageId.Kid] = "Prefabs/Characters/pi_kid";
        Models[(int)PersistentData.TypesOfAnimin.Pi, (int)AniminEvolutionStageId.Adult] = "Prefabs/Characters/pi_adult";

        AnimationLists = new AnimatorOverrideController[(int)PersistentData.TypesOfAnimin.Count, (int)AniminEvolutionStageId.Count];
        AnimationLists[(int)PersistentData.TypesOfAnimin.Tbo, (int)AniminEvolutionStageId.Baby] = Resources.Load<AnimatorOverrideController>(@"AnimControllers/TBOBabyAnimations");
        AnimationLists[(int)PersistentData.TypesOfAnimin.Tbo, (int)AniminEvolutionStageId.Kid] = Resources.Load<AnimatorOverrideController>(@"AnimControllers/TBOKidAnimations");
        AnimationLists[(int)PersistentData.TypesOfAnimin.Tbo, (int)AniminEvolutionStageId.Adult] = Resources.Load<AnimatorOverrideController>(@"AnimControllers/TBOAdultAnimations");
	
        AnimationLists[(int)PersistentData.TypesOfAnimin.Kelsey, (int)AniminEvolutionStageId.Baby] = Resources.Load<AnimatorOverrideController>(@"AnimControllers/KelsiBabyAnimations");
        AnimationLists[(int)PersistentData.TypesOfAnimin.Kelsey, (int)AniminEvolutionStageId.Kid] = Resources.Load<AnimatorOverrideController>(@"AnimControllers/KelsyKidAnimations");
        AnimationLists[(int)PersistentData.TypesOfAnimin.Kelsey, (int)AniminEvolutionStageId.Adult] = Resources.Load<AnimatorOverrideController>(@"AnimControllers/KelsiAdultAnimations");

        AnimationLists[(int)PersistentData.TypesOfAnimin.Mandi, (int)AniminEvolutionStageId.Baby] = Resources.Load<AnimatorOverrideController>(@"AnimControllers/MandiBabyAnimations");
        AnimationLists[(int)PersistentData.TypesOfAnimin.Mandi, (int)AniminEvolutionStageId.Kid] = Resources.Load<AnimatorOverrideController>(@"AnimControllers/MandiKidAnimations");
        AnimationLists[(int)PersistentData.TypesOfAnimin.Mandi, (int)AniminEvolutionStageId.Adult] = Resources.Load<AnimatorOverrideController>(@"AnimControllers/MandiAdultAnimations");

        AnimationLists[(int)PersistentData.TypesOfAnimin.Pi, (int)AniminEvolutionStageId.Baby] = Resources.Load<AnimatorOverrideController>(@"AnimControllers/PiBabyAnimations");
        AnimationLists[(int)PersistentData.TypesOfAnimin.Pi, (int)AniminEvolutionStageId.Kid] = Resources.Load<AnimatorOverrideController>(@"AnimControllers/PiKidAnimations");
        AnimationLists[(int)PersistentData.TypesOfAnimin.Pi, (int)AniminEvolutionStageId.Adult] = Resources.Load<AnimatorOverrideController>(@"AnimControllers/PiAdultAnimations");
        /*
        AnimationLists[(int)PersistentData.TypesOfAnimin.Mandi, (int)AniminEvolutionStageId.Baby] = Resources.Load<AnimatorOverrideController>(@"AnimControllers/MandiBabyAnimations");
        AnimationLists[(int)PersistentData.TypesOfAnimin.Mandi, (int)AniminEvolutionStageId.Kid] = Resources.Load<AnimatorOverrideController>(@"AnimControllers/MandiKidAnimations");
        AnimationLists[(int)PersistentData.TypesOfAnimin.Mandi, (int)AniminEvolutionStageId.Adult] = Resources.Load<AnimatorOverrideController>(@"AnimControllers/MandiAdultAnimations");

        AnimationLists[(int)PersistentData.TypesOfAnimin.Pi, (int)AniminEvolutionStageId.Baby] = Resources.Load<AnimatorOverrideController>(@"AnimControllers/PiBabyAnimations");
        AnimationLists[(int)PersistentData.TypesOfAnimin.Pi, (int)AniminEvolutionStageId.Kid] = Resources.Load<AnimatorOverrideController>(@"AnimControllers/PiKidAnimations");
        AnimationLists[(int)PersistentData.TypesOfAnimin.Pi, (int)AniminEvolutionStageId.Adult] = Resources.Load<AnimatorOverrideController>(@"AnimControllers/PiAdultAnimations");
        */
	}

	void OnGUI()
	{
//		if(GUI.Button(new Rect(0, 0, 200, 100), "TBOBaby"))
//		{
//			LoadCharacter(CreatureTypeId.TBOBaby);
//		}

//		if(GUI.Button(new Rect(250, 0, 200, 100), "TBOKid"))
//		{
//			LoadCharacter(CreatureTypeId.TBOKid);
//		}

//		if(GUI.Button(new Rect(500, 0, 200, 100), "TBOAdult"))
//		{
//			LoadCharacter(CreatureTypeId.TBOAdult);
//		}
	}

    public string GetModelPath(PersistentData.TypesOfAnimin animinId, AniminEvolutionStageId id)
	{
		return Models[(int)animinId, (int)id];
	}

    public AnimatorOverrideController GetAnimationControlller(PersistentData.TypesOfAnimin animinId, AniminEvolutionStageId id)
	{
		return AnimationLists[(int)animinId, (int)id];
	}

    public void LoadCharacter(PersistentData.TypesOfAnimin animinId, AniminEvolutionStageId id, bool includeEgg)
	{
        if (animinId == PersistentData.TypesOfAnimin.TboAdult)
            animinId = PersistentData.TypesOfAnimin.Tbo;



        if (includeEgg)
        {
            Object resourceEgg = Resources.Load(EggModels[(int)animinId]);
            GameObject instanceEgg = GameObject.Instantiate(resourceEgg) as GameObject;
			MultipleModelMaterials mmm = instanceEgg.GetComponent<MultipleModelMaterials>();
			if(mmm != null)
			{
				mmm.Index = (int)animinId;
			}
            Vector3 eggScale = instanceEgg.transform.localScale;
            Vector3 eggPos = instanceEgg.transform.localPosition;
            instanceEgg.transform.SetParent(UIGlobalVariablesScript.Singleton.MainCharacterRef.transform, true);
            instanceEgg.transform.localScale = eggScale;
            instanceEgg.transform.localPosition = eggPos;
            if (CurrentEgg != null)
            {
                CurrentEgg.transform.parent = null;
                Destroy(CurrentEgg);
            }
            CurrentEgg = instanceEgg;
        }

		Object resource = Resources.Load(Models[(int)animinId, (int)id]);

		GameObject instance = GameObject.Instantiate(resource) as GameObject;

        CurrentEyesOpenTexture = (Texture2D)instance.GetComponentInChildren<Renderer>().material.mainTexture;

		Vector3 scale = instance.transform.localScale;
		defaultScale = scale;
		if(includeEgg)
        {
			scale = new Vector3(4,4,4);
        }
		//RuntimeAnimatorController controller = CurrentModel.GetComponent<Animator>().runtimeAnimatorController;

		instance.transform.parent = UIGlobalVariablesScript.Singleton.MainCharacterRef.transform;

		if(CurrentModel != null)
		{
		
			CurrentModel.transform.parent = null;
			Destroy(CurrentModel);
		}

		instance.transform.localPosition = Vector3.zero;
		instance.transform.localScale = scale;

		instance.transform.localRotation = Quaternion.identity;

		CurrentModel = instance;

		CurrentModel.GetComponent<Animator>().runtimeAnimatorController = AnimationLists[(int)animinId, (int)id];


		UIGlobalVariablesScript.Singleton.SoundEngine.LoadSoundsForAnimin(animinId, id);
		/*for(int i=0;i<TBOAdultAnimations.clips.Length;++i)
		{
			CurrentModel.GetComponent<Animator>().runtimeAnimatorController
			overrideController[clipOverride.clipNamed] = clipOverride.overrideWith;
		}
		
		animator.runtimeAnimatorController = overrideController;
*/

//		Debug.Log ("Getting Controller Script : ["+UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimationControllerScript>()+"];");
		if (UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimationControllerScript>()!=null)
			UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimationControllerScript>().SetCharacter(instance);

	}
}

//public enum AniminId
//{
//	Pi = 0,
//	Tbo = 1,
//	Kelsey = 2,
//	Mandi = 3,
//	Count,
//}

public enum AniminEvolutionStageId
{
	Baby = 0,
	Kid,
	Adult,

	Count,
}

public enum CreatureAnimationId
{
	_celebrate = 0,
	_dance,
	_eat_pill,
	_evolve_react,
	_feed,
	_happy_01,
	_happy_02,
	_happy_03,
	_happy_04,
	_happy_05,
	_hit,
	_hungry_loop,
	_idle_stand,
	_injection_react,
	_jump,
	_jump_in_portal,
	_jump_out_portal,
	_look_01,
	_look_02,
	_look_03,
	_no,
	_pat_react,
	_pick_up_object_idle,
	_run,
	_sad_01,
	_sad_02,
	_sad_03,
	_sad_04,
	_sleep,
	_sleep_to_idle_stand,
	_throw,
	_tickle,
	_unwell,
	_walk,
	_wave,

	Count
}
