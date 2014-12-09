using UnityEngine;
using System.Collections;

public class SoundEngineScript : MonoBehaviour 
{
	//private AudioClip[] MinigameCubeRunners;
	private AudioClip[] GenericSounds;
	private AudioClip[,,] CreatureSounds;
	public AudioSource SoundFxLooper1;
	public AudioClip[] FartSounds;
	public AudioClip[] MusicSounds;

	// Use this for initialization
	void Start () 
	{
		FartSounds = Resources.LoadAll<AudioClip>("Sounds/Main_Stage_Farts");

		MusicSounds = new AudioClip[(int)MusicId.Count];
		MusicSounds[(int)MusicId.CubeGame] = Resources.Load("Sounds/Items/Food_Drop") as AudioClip;
		MusicSounds[(int)MusicId.Gungame] = Resources.Load("Sounds/Items/Food_Drop") as AudioClip;

		GenericSounds = new AudioClip[(int)GenericSoundId.Count];
		GenericSounds[(int)GenericSoundId.DropFood] = Resources.Load("Sounds/Items/Food_Drop") as AudioClip;
		GenericSounds[(int)GenericSoundId.ItemPickup] = Resources.Load("Sounds/Items/Item_Pick_Up") as AudioClip;
		GenericSounds[(int)GenericSoundId.DropMeds] = Resources.Load("Sounds/Items/Meds_Drop") as AudioClip;
		GenericSounds[(int)GenericSoundId.DropItem] = Resources.Load("Sounds/Items/Item_Drop") as AudioClip;
		GenericSounds[(int)GenericSoundId.ItemLand] = Resources.Load("Sounds/Items/Item Land") as AudioClip;


		GenericSounds[(int)GenericSoundId.TakePoo] = Resources.Load("Sounds/Toilet_Functions/Poo") as AudioClip;
		GenericSounds[(int)GenericSoundId.TakePiss] = Resources.Load("Sounds/Toilet_Functions/Wee") as AudioClip;
		GenericSounds[(int)GenericSoundId.CleanPooPiss] = Resources.Load("Sounds/Toilet_Functions/Poo and Wee Cleanup") as AudioClip;

		GenericSounds[(int)GenericSoundId.Bump_Into_Baddy] = Resources.Load("Sounds/Minigame01_Platform/Bump_Into_Baddy") as AudioClip;
		GenericSounds[(int)GenericSoundId.Collect_Box] = Resources.Load("Sounds/Minigame01_Platform/Collect_Box") as AudioClip;
		GenericSounds[(int)GenericSoundId.Fall_Through_Levels] = Resources.Load("Sounds/Minigame01_Platform/Fall_Through_Levels") as AudioClip;
		GenericSounds[(int)GenericSoundId.Grid_Cubes_Fall] = Resources.Load("Sounds/Minigame01_Platform/Grid_Cubes_Fall") as AudioClip;
		GenericSounds[(int)GenericSoundId.Jump] = Resources.Load("Sounds/Minigame01_Platform/Jump") as AudioClip;
		GenericSounds[(int)GenericSoundId.Kill_Baddy] = Resources.Load("Sounds/Minigame01_Platform/Kill_Baddy") as AudioClip;
		GenericSounds[(int)GenericSoundId.Land_After_Falling_Lose_3_Stars] = Resources.Load("Sounds/Minigame01_Platform/Land_After_Falling_Lose_3_Stars") as AudioClip;
		GenericSounds[(int)GenericSoundId.Star_Collect] = Resources.Load("Sounds/Minigame01_Platform/Star_Collect") as AudioClip;
		GenericSounds[(int)GenericSoundId.Star_Complete] = Resources.Load("Sounds/Minigame01_Platform/Star_Complete") as AudioClip;


		GenericSounds[(int)GenericSoundId.GunGame_barrel_destroy] = Resources.Load("Sounds/Minigame02_Gun_Game/barrel_destroy") as AudioClip;
		GenericSounds[(int)GenericSoundId.GunGame_barrel_destroy_multicoloured] = Resources.Load("Sounds/Minigame02_Gun_Game/barrel_destroy_multicoloured") as AudioClip;
		GenericSounds[(int)GenericSoundId.GunGame_bonus_box] = Resources.Load("Sounds/Minigame02_Gun_Game/bonus_box") as AudioClip;
		GenericSounds[(int)GenericSoundId.GunGame_Bump_Into_Baddy] = Resources.Load("Sounds/Minigame02_Gun_Game/Bump_Into_Baddy") as AudioClip;
		GenericSounds[(int)GenericSoundId.GunGame_gun_firing_loop] = Resources.Load("Sounds/Minigame02_Gun_Game/gun_firing_loop") as AudioClip;
		GenericSounds[(int)GenericSoundId.GunGame_monsters_merge] = Resources.Load("Sounds/Minigame02_Gun_Game/monsters_merge") as AudioClip;


		GenericSounds[(int)GenericSoundId.CollectStar1] = Resources.Load("Sounds/Minigame01_Platform/Star_Collect_note1") as AudioClip;
		GenericSounds[(int)GenericSoundId.CollectStar2] = Resources.Load("Sounds/Minigame01_Platform/Star_Collect_note2") as AudioClip;
		GenericSounds[(int)GenericSoundId.CollectStar3] = Resources.Load("Sounds/Minigame01_Platform/Star_Collect_note3") as AudioClip;
		GenericSounds[(int)GenericSoundId.CollectStar4] = Resources.Load("Sounds/Minigame01_Platform/Star_Collect_note4") as AudioClip;
		GenericSounds[(int)GenericSoundId.CollectStar5] = Resources.Load("Sounds/Minigame01_Platform/Star_Collect_note5") as AudioClip;

		GenericSounds[(int)GenericSoundId.GunLoop] = Resources.Load("Sounds/Minigame02_Gun_Game/gun_firing_loop") as AudioClip;

        if (CreatureSounds == null)
        {
            CreatureSounds = new AudioClip[(int)PersistentData.TypesOfAnimin.Count, (int)AniminEvolutionStageId.Count, (int)CreatureSoundId.Count];
        }

	}

	void Update()
	{
		if (SoundFxLooper1 != null) {
			if (!ProfilesManagementScript.Singleton.CurrentProfile.Settings.AudioEnabled) {
				if (SoundFxLooper1.isPlaying)
					SoundFxLooper1.Stop ();
			} else {
				if (LooperPlaying && !SoundFxLooper1.isPlaying)
					SoundFxLooper1.Play ();
				else if (!LooperPlaying && SoundFxLooper1.isPlaying)
					SoundFxLooper1.Stop ();

			}
		}
	}

    public void UnLoadSoundsForAnimin(PersistentData.TypesOfAnimin id, AniminEvolutionStageId evolution)
	{

	}

    public void LoadSoundsForAnimin(PersistentData.TypesOfAnimin id, AniminEvolutionStageId evolution)
	{
        if(id == PersistentData.TypesOfAnimin.Tbo && evolution == AniminEvolutionStageId.Baby) LoadCreatureHelper((int)id, (int)evolution, "tbo_baby");
        else if(id == PersistentData.TypesOfAnimin.Tbo && evolution == AniminEvolutionStageId.Kid) LoadCreatureHelper((int)id, (int)evolution, "tbo_kid");
        else if(id == PersistentData.TypesOfAnimin.Tbo && evolution == AniminEvolutionStageId.Adult) LoadCreatureHelper((int)id, (int)evolution, "tbo_adult");
        else if(id == PersistentData.TypesOfAnimin.Kelsey && evolution == AniminEvolutionStageId.Baby) LoadCreatureHelper((int)id, (int)evolution, "ke_baby");
        else if(id == PersistentData.TypesOfAnimin.Kelsey && evolution == AniminEvolutionStageId.Kid) LoadCreatureHelper((int)id, (int)evolution, "ke_kid");
        else if(id == PersistentData.TypesOfAnimin.Kelsey && evolution == AniminEvolutionStageId.Adult) LoadCreatureHelper((int)id, (int)evolution, "ke_kelsi");

        string sname =
            id == PersistentData.TypesOfAnimin.Tbo ? "Tbo" :
            id == PersistentData.TypesOfAnimin.Pi ? "Pi" :
            id == PersistentData.TypesOfAnimin.Mandi ? "Ma" :
            id == PersistentData.TypesOfAnimin.Kelsey ? "Ke" :
        "";
        string sevo =
            evolution == AniminEvolutionStageId.Baby ? "baby" :
            evolution == AniminEvolutionStageId.Kid ? "kid" :
            evolution == AniminEvolutionStageId.Adult ? "Adult" :
            "";

        string pathname = "Sounds/" + sname + "_" + sevo + "_sfx/" + (sname.ToLower()) + "_" + sevo + "_";

//		Debug.Log ("Filename : [" + pathname + "SFX];");


        if (CreatureSounds == null)
        {
            CreatureSounds = new AudioClip[(int)PersistentData.TypesOfAnimin.Count, (int)AniminEvolutionStageId.Count, (int)CreatureSoundId.Count];
        }

        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.Celebrate] =   Resources.Load(pathname + "celebrate") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.EatPill] =     Resources.Load(pathname + "eat_pill") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.FeedFood] =    Resources.Load(pathname + "feed_food") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.FeedDrink] =   Resources.Load(pathname + "feed_drink") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.Happy1] =      Resources.Load(pathname + "happy_01") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.Happy2] =      Resources.Load(pathname + "happy_02") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.Happy3] =      Resources.Load(pathname + "happy_03") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.Happy4] =      Resources.Load(pathname + "happy_04") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.Happy5] =      Resources.Load(pathname + "happy_05") as AudioClip;

        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.Hungry] =      Resources.Load(pathname + "Hungry") as AudioClip;

        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.JumbInPortal] =Resources.Load(pathname + "jump_in_portal") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.JumbOutPortal]=Resources.Load(pathname + "jump_out_portal") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.No] =          Resources.Load(pathname + "No") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.Sad1] =        Resources.Load(pathname + "sad01") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.Sad2] =        Resources.Load(pathname + "sad02") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.Sad3] =        Resources.Load(pathname + "sad03") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.Sad4] =        Resources.Load(pathname + "sad04") as AudioClip;

        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.SleepToIdle] = Resources.Load(pathname + "sleep_to_idle_stand") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.Throw] =       Resources.Load(pathname + "Throw") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.Tickle] =      Resources.Load(pathname + "Tickle") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.Tickle2] =      Resources.Load(pathname + "tickle_02") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.Tickle3] =      Resources.Load(pathname + "tickle_03") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.SnoringSleeping] =      Resources.Load(pathname + "sleep_snoring_loop") as AudioClip;

        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.Unwell] =      Resources.Load(pathname + "Unwell") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.InjectionReact] = Resources.Load(pathname + "injection_react") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.PatReact] = Resources.Load(pathname + "pat_react") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.IdleWave] = Resources.Load(pathname + "idle_wave") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.RandomTalk1] = Resources.Load(pathname + "talk_01") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.RandomTalk2] = Resources.Load(pathname + "talk_02") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.RandomTalk3] = Resources.Load(pathname + "talk_03") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.RandomTalk4] = Resources.Load(pathname + "talk_04") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.RandomTalk5] = Resources.Load(pathname + "talk_05") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.RandomTalk6] = Resources.Load(pathname + "talk_06") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.RandomTalk7] = Resources.Load(pathname + "talk_07") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.RandomTalk8] = Resources.Load(pathname + "talk_08") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.RandomTalk9] = Resources.Load(pathname + "talk_09") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.RandomTalk10] = Resources.Load(pathname + "talk_10") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.RandomTalk11] = Resources.Load(pathname + "talk_11") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.RandomTalk12] = Resources.Load(pathname + "talk_12") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.RandomTalk13] = Resources.Load(pathname + "talk_13") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.RandomTalk14] = Resources.Load(pathname + "talk_14") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.RandomTalk15] = Resources.Load(pathname + "talk_15") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.RandomTalk16] = Resources.Load(pathname + "talk_16") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.RandomTalk17] = Resources.Load(pathname + "talk_17") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.RandomTalk18] = Resources.Load(pathname + "talk_18") as AudioClip;
        CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.RandomTalk19] = Resources.Load(pathname + "talk_19") as AudioClip;

        #region Old Code specifically for loading Tbo Sounds
        /*
		if(id == AniminId.Tbo && evolution == AniminEvolutionStageId.Baby)
		{
			CreatureSounds[(int)id, (int)evolution, (int)CreatureSoundId.Celebrate] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@celebrate") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.EatPill] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@eat_pill") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.FeedFood] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@feed_food") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.FeedDrink] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@feed_drink") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.Happy1] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@happy_01") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.Happy2] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@happy_02") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.Happy3] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@happy_03") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.Happy4] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@happy_04") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.Happy5] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@happy_05") as AudioClip;
			
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.Hungry] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@Hungry") as AudioClip;
			
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.JumbInPortal] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@jump_in_portal") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.JumbOutPortal] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@jump_out_portal") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.No] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@No") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.Sad1] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@sad01") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.Sad2] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@sad02") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.Sad3] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@sad03") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.Sad4] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@sad04") as AudioClip;
			
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.SleepToIdle] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@sleep_to_idle_stand") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.Throw] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@Throw") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.Tickle] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@Tickle") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.Tickle2] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@tickle_02") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.Tickle3] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@tickle_03") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.SnoringSleeping] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@sleep_snoring_loop") as AudioClip;
			
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.Unwell] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@Unwell") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.InjectionReact] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@injection_react") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.PatReact] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@pat_react") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.IdleWave] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby@idle_wave") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.RandomTalk1] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby_talk_01") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.RandomTalk2] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby_talk_02") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.RandomTalk3] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby_talk_03") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.RandomTalk4] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby_talk_04") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.RandomTalk5] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby_talk_05") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.RandomTalk6] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby_talk_06") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.RandomTalk7] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby_talk_07") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.RandomTalk8] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby_talk_08") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.RandomTalk9] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby_talk_09") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.RandomTalk10] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby_talk_10") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.RandomTalk11] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby_talk_11") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.RandomTalk12] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby_talk_12") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.RandomTalk13] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby_talk_13") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.RandomTalk14] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby_talk_14") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.RandomTalk15] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby_talk_15") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.RandomTalk16] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby_talk_16") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.RandomTalk17] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby_talk_17") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.RandomTalk18] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby_talk_18") as AudioClip;
			CreatureSounds[(int)id,(int)evolution, (int)CreatureSoundId.RandomTalk18] = Resources.Load("Sounds/Tbo_baby_sfx/tbo_baby_talk_19") as AudioClip;

		}
        */
        #endregion
    }

	private void LoadCreatureHelper(int id, int evolution, string name)
	{
		
	}
	
	public void PlayFart()
	{
        if(!ProfilesManagementScript.Singleton.CurrentProfile.Settings.AudioEnabled) return;

		this.audio.PlayOneShot( FartSounds[Random.Range(0, FartSounds.Length)] );
	}


	private bool LooperPlaying;
    public void PlayLoop(PersistentData.TypesOfAnimin animin, AniminEvolutionStageId creatureId, CreatureSoundId soundId)
	{
		//Debug.Log("LOOP: " + soundId.ToString());

        if (SoundFxLooper1!=null) SoundFxLooper1.clip = CreatureSounds[(int)animin, (int)creatureId, (int)soundId];
		LooperPlaying = true;

	}

	public void PlayLoop(GenericSoundId id)
	{
		//Debug.Log("LOOP: " + soundId.ToString());
		
        if(SoundFxLooper1!=null)SoundFxLooper1.clip = GenericSounds[(int)id];
		LooperPlaying = true;
		
	}

	public void StopLoop()
	{
        if (SoundFxLooper1!=null)SoundFxLooper1.Stop();
		LooperPlaying = false;

	}

	public void Play(GenericSoundId id)
	{
        if(!ProfilesManagementScript.Singleton.CurrentProfile.Settings.AudioEnabled) return;
		this.audio.PlayOneShot(GenericSounds[(int)id]);
	}

    public void Play(PersistentData.TypesOfAnimin animin, AniminEvolutionStageId creatureId, CreatureSoundId soundId)
	{
        if (animin == PersistentData.TypesOfAnimin.TboAdult)
            animin = PersistentData.TypesOfAnimin.Tbo;
		if(!ProfilesManagementScript.Singleton.CurrentProfile.Settings.AudioEnabled) return;
		if (CreatureSounds [(int)animin, (int)creatureId, (int)soundId] == null) {
			Debug.Log ("Panic! Sound : [" + animin + "|" + creatureId + "|" + soundId + "] is not valid");
			return;
		}

		//Debug.Log("Play Sound : [" + animin + "|" + creatureId + "|" + soundId + "];");
		//Debug.Log ("CreatureSounds : [" + CreatureSounds [(int)animin, (int)creatureId, (int)soundId] + "];");
		//Debug.Log ("Is Ready to Play? : [" + CreatureSounds [(int)animin, (int)creatureId, (int)soundId].isReadyToPlay + "];");

		this.GetComponent<AudioSource>().PlayOneShot(CreatureSounds[(int)animin, (int)creatureId, (int)soundId]);

	}
}

public enum GenericSoundId
{
	DropItem,
	DropFood,
	DropMeds,
	ItemPickup,
	TakePoo,
	TakePiss,
	CleanPooPiss,
	ItemLand,

	Bump_Into_Baddy,
	Collect_Box,
	Fall_Through_Levels,
	Grid_Cubes_Fall,
	Jump,
	Kill_Baddy,
	Land_After_Falling_Lose_3_Stars,
	Star_Collect,
	Star_Complete,

	CollectStar1,
	CollectStar2,
	CollectStar3,
	CollectStar4,
	CollectStar5,

	ZefCollect,
	ZefSpawn,
	GunLoop,


	GunGame_barrel_destroy,
	GunGame_barrel_destroy_multicoloured,
	GunGame_bonus_box,
	GunGame_Bump_Into_Baddy,
	GunGame_gun_firing_loop,
	GunGame_monsters_merge,

	Count,
}

public enum MusicId
{
	Gungame = 0,
	CubeGame,
	Count,
}

public enum CreatureSoundId
{
	Celebrate,
	EatPill,

	FeedFood,
	FeedDrink,

	Happy1,
	Happy2,
	Happy3,
	Happy4,
	Happy5,
	Hungry,
	JumbInPortal,
	JumbOutPortal,
	No,
	Sad1,
	Sad2,
	Sad3,
	Sad4,
	SleepToIdle,
	Throw,
	Tickle,
	Tickle2,
	Tickle3,
	Unwell,

	PatReact,
	InjectionReact,

	IdleWave,
	SnoringSleeping,


	RandomTalk1,
	RandomTalk2,
	RandomTalk3,
	RandomTalk4,
	RandomTalk5,
	RandomTalk6,
	RandomTalk7,
	RandomTalk8,
	RandomTalk9,
	RandomTalk10,
	RandomTalk11,
	RandomTalk12,
	RandomTalk13,
	RandomTalk14,
	RandomTalk15,
	RandomTalk16,
	RandomTalk17,
	RandomTalk18,
	RandomTalk19,

	Count,
}


