using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using UnityEngine.UI;


public class UIClickButtonMasterScript : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void _saveScreenshotToCameraRoll();

    public UIFunctionalityId FunctionalityId;
    private static float SavedRadius;
    //private Vector3 SavedScale;

    public static void PopulateInterfaceItems(PopupItemType typeToLoad, List<GameObject> allSprites)
    {
        List<GameObject> subItems = new List<GameObject>();
        GameObject itemsPanel = UIGlobalVariablesScript.Singleton.PanelItems;
        for (int i = 0; i < itemsPanel.transform.childCount; ++i)
        {
            GameObject lister = itemsPanel.transform.GetChild(i).gameObject;
            lister.SetActive(false);
            subItems.Add(lister);
        }
		
        List<InventoryItemData> inventoryItems = new List<InventoryItemData>();
        for (int i = 0; i < ProfilesManagementScript.Singleton.CurrentAnimin.Inventory.Count; ++i)
        {
//			Debug.Log(ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id.ToString());
            if (InventoryItemData.Items[(int)ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id].ItemType != typeToLoad)
                continue;
            inventoryItems.Add(ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i]);
        }
		
		
		
        int panelCount = 0;
        for (int i = 0; i < inventoryItems.Count; i += 2)
        {
            subItems[panelCount].SetActive(true);
            subItems[panelCount].transform.GetChild(0).gameObject.SetActive(false);
            subItems[panelCount].transform.GetChild(1).gameObject.SetActive(false);

            GameObject sprite0 = subItems[panelCount].transform.GetChild(0).gameObject;
            Debug.Log(sprite0.name);

           // subItems[panelCount].transform.GetChild(0).gameObject.GetComponent<Button>().normalSprite = InventoryItemData.Items[(int)inventoryItems[i + 0].Id].SpriteName;
            subItems[panelCount].transform.GetChild(0).gameObject.GetComponent<InterfaceItemLinkToModelScript>().Item3DPrefab = InventoryItemData.Items[(int)inventoryItems[i + 0].Id].PrefabId;
            subItems[panelCount].transform.GetChild(0).gameObject.GetComponent<InterfaceItemLinkToModelScript>().ItemID = InventoryItemData.Items[(int)inventoryItems[i + 0].Id].Id;
            subItems[panelCount].transform.GetChild(0).gameObject.SetActive(true);
            sprite0.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = inventoryItems[i + 0].Count.ToString();

            if (inventoryItems[i + 0].Count == 1)
                sprite0.transform.GetChild(0).gameObject.SetActive(false);
            else
                sprite0.transform.GetChild(0).gameObject.SetActive(true);


            if (allSprites != null)
            {
                allSprites.Add(subItems[panelCount].transform.GetChild(0).gameObject);
            }

            if (inventoryItems.Count > i + 1)
            {
                GameObject sprite1 = subItems[panelCount].transform.GetChild(1).gameObject;

               // subItems[panelCount].transform.GetChild(1).gameObject.GetComponent<Button>().normalSprite = InventoryItemData.Items[(int)inventoryItems[i + 1].Id].SpriteName;
                subItems[panelCount].transform.GetChild(1).gameObject.GetComponent<InterfaceItemLinkToModelScript>().Item3DPrefab = InventoryItemData.Items[(int)inventoryItems[i + 1].Id].PrefabId;
                subItems[panelCount].transform.GetChild(1).gameObject.GetComponent<InterfaceItemLinkToModelScript>().ItemID = InventoryItemData.Items[(int)inventoryItems[i + 1].Id].Id;
                subItems[panelCount].transform.GetChild(1).gameObject.SetActive(true);
                sprite1.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = inventoryItems[i + 1].Count.ToString();

				
                if (inventoryItems[i + 1].Count == 1)
                    sprite1.transform.GetChild(0).gameObject.SetActive(false);
                else
                    sprite1.transform.GetChild(0).gameObject.SetActive(true);

                if (allSprites != null)
                {
                    allSprites.Add(subItems[panelCount].transform.GetChild(1).gameObject);
                }
            }
			
            panelCount++;
        }
    }


    void OnDoubleClick()
    {
        HandleDoubleClick(FunctionalityId);
    }

    void OnClick()
    {
        HandleClick(FunctionalityId, this.gameObject);
    }

    void OnPress(bool isPressed)
    {

        if (isPressed)
        {
            //Debug.Log("PRESS DETECTED");
            switch (FunctionalityId)
            {
                case UIFunctionalityId.JumbOnCubeRunner:
                    {
                        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterControllerScript>().PressedJumb = true;
					
                        if (UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>().TutorialId == MinigameCollectorScript.TutorialStateId.ShowJumb)
                            UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>().AdvanceTutorial();


                        break;
                    }
                case UIFunctionalityId.ShootBullet:
                    {
                        UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.Jump);
				
                        //UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().ShootBulletForward();
                        break;
                    }
            }
        }
    }

    void HandleDoubleClick(UIFunctionalityId id)
    {
        //GameObject mainMenuPopupRef = UIGlobalVariablesScript.Singleton.MainMenuPopupObjectRef;
        //Debug.Log("BUTTON: " + id.ToString());
        switch (id)
        {
            case UIFunctionalityId.ClearAllGroundItems:
                {
                    CharacterProgressScript script = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();
			
                    for (int i = 0; i < script.GroundItems.Count; ++i)
                    {
                        if (script.GroundItems[i].GetComponent<UIPopupItemScript>() != null)
                        {
                            if (script.GroundItems[i].GetComponent<UIPopupItemScript>().Type == PopupItemType.Token)
                            {
                                continue;
                            }
                            else
                            {
                                ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(script.GroundItems[i].GetComponent<UIPopupItemScript>().Id, 1);
                            }
                        }
                        Destroy(script.GroundItems[i]);
                    }
			
                    script.GroundItems.Clear();
                    UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.CleanPooPiss);
                    UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().HidePopupMenus();

                    for (int i = 0; i < EDMMixerScript.Singleton.KeysOn.Length; ++i)
                    {
                        EDMMixerScript.Singleton.KeysOn[i] = false;
                    }

                    break;
                }
        }
    }

    public static void SetSoundSprite()
    {
//        if (ProfilesManagementScript.Singleton.CurrentProfile.Settings.AudioEnabled)
//            UIGlobalVariablesScript.Singleton.SoundSprite.GetComponent<Image>().spriteName = "pauseScreenSound";
//        else
//            UIGlobalVariablesScript.Singleton.SoundSprite.GetComponent<Image>().spriteName = "soundOff";
    }

    public static void HandleClick(UIFunctionalityId id, GameObject sender)
    {

        Debug.Log("BUTTON: " + id.ToString());
        switch (id)
        {
            case UIFunctionalityId.None:
                {
                    Debug.Log("You clicked on a button that does nothing. ");
                    break;
                }

            case UIFunctionalityId.GoToMainMenuFromGame:
                {
                    SaveAndLoad.Instance.SaveAllData();

                    Application.LoadLevel("Menu");

                    break;
                }

            case UIFunctionalityId.AudioOnOffGame:
                {
                    ProfilesManagementScript.Singleton.CurrentProfile.Settings.AudioEnabled = !ProfilesManagementScript.Singleton.CurrentProfile.Settings.AudioEnabled;

                    SetSoundSprite();

                    SaveAndLoad.Instance.SaveAllData();
                    Debug.Log("just saved...ui cllick");

                    break;
                }
            case UIFunctionalityId.ResetAnimin:
                {
                    ProfilesManagementScript.Singleton.CurrentAnimin.SetDefault(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId);
                    Application.LoadLevel("VuforiaTest");

                    break;
                }
			
            case UIFunctionalityId.OpenCloseFoods:
            case UIFunctionalityId.OpenCloseItems:
            case UIFunctionalityId.OpenCloseMedicine:
                {
                    GameObject mainMenuPopupRef = UIGlobalVariablesScript.Singleton.MainMenuPopupObjectRef;

                    if (mainMenuPopupRef.activeInHierarchy && UIGlobalVariablesScript.ButtonTriggeredMainMenuPopupRef != sender)
                    {

                    }
                    else
                    {
                        mainMenuPopupRef.SetActive(!mainMenuPopupRef.activeInHierarchy);
                        UIGlobalVariablesScript.Singleton.StatsButton.SetActive(!mainMenuPopupRef.activeInHierarchy);
                        UIGlobalVariablesScript.Singleton.MinigamesButton.SetActive(!mainMenuPopupRef.activeInHierarchy);
						
                    }
                    UIGlobalVariablesScript.ButtonTriggeredMainMenuPopupRef = sender;




                    //UIGlobalVariablesScript.Singleton.PanelFoods.SetActive(false);
                    //UIGlobalVariablesScript.Singleton.PanelMedicine.SetActive(false);
                    UIGlobalVariablesScript.Singleton.PanelItems.SetActive(true);
                    PopupItemType typeToLoad = PopupItemType.Food;

                    if (id == UIFunctionalityId.OpenCloseFoods)
                    {
                        typeToLoad = PopupItemType.Food;
                        UIGlobalVariablesScript.Singleton.ItemsFoodMedicineLabel.text = "Foods";
                        //UIGlobalVariablesScript.Singleton.PanelFoods.SetActive(true);
                        UIGlobalVariablesScript.Singleton.PopupIndicator.transform.localPosition = new Vector3(-312, UIGlobalVariablesScript.Singleton.PopupIndicator.transform.localPosition.y, 0);
                    }
                    else if (id == UIFunctionalityId.OpenCloseMedicine)
                    {
                        typeToLoad = PopupItemType.Medicine;
                        UIGlobalVariablesScript.Singleton.ItemsFoodMedicineLabel.text = "Medicine";
                        //UIGlobalVariablesScript.Singleton.PanelMedicine.SetActive(true);
                        UIGlobalVariablesScript.Singleton.PopupIndicator.transform.localPosition = new Vector3(312, UIGlobalVariablesScript.Singleton.PopupIndicator.transform.localPosition.y, 0);
                    }
                    else if (id == UIFunctionalityId.OpenCloseItems)
                    {
                        typeToLoad = PopupItemType.Item;
                        UIGlobalVariablesScript.Singleton.ItemsFoodMedicineLabel.text = "Items";
                        //UIGlobalVariablesScript.Singleton.PanelItems.SetActive(true);
                        UIGlobalVariablesScript.Singleton.PopupIndicator.transform.localPosition = new Vector3(0, UIGlobalVariablesScript.Singleton.PopupIndicator.transform.localPosition.y, 0);
                    }

                    //UIGlobalVariablesScript.Singleton.ItemScrollView.GetComponent<ScrollRect>().ResetPosition();


                    PopulateInterfaceItems(typeToLoad, null);

                    break;
                }
			
            case UIFunctionalityId.SetActiveItemOnBottomBarAndClose:
                {
                    GameObject mainMenuPopupRef = UIGlobalVariablesScript.Singleton.MainMenuPopupObjectRef;
                    mainMenuPopupRef.SetActive(false);

                    //	Debug.Log("BUTTON CLICL!!!");

                    //InventoryItemId itemId = UIGlobalVariablesScript.ButtonTriggeredMainMenuPopupRef.GetComponent<InterfaceItemLinkToModelScript>().ItemID;

                    UIGlobalVariablesScript.Singleton.StatsButton.SetActive(!mainMenuPopupRef.activeInHierarchy);
                    UIGlobalVariablesScript.Singleton.MinigamesButton.SetActive(!mainMenuPopupRef.activeInHierarchy);

                    UIGlobalVariablesScript.ButtonTriggeredMainMenuPopupRef.GetComponent<InterfaceItemLinkToModelScript>().ItemID = 
				sender.GetComponent<InterfaceItemLinkToModelScript>().ItemID;
			
                    //string spriteName = UIGlobalVariablesScript.ButtonTriggeredMainMenuPopupRef.GetComponent<Image>().spriteName;
//                    UIGlobalVariablesScript.ButtonTriggeredMainMenuPopupRef.GetComponent<Image>().atlas = sender.GetComponent<Image>().atlas;
//                    UIGlobalVariablesScript.ButtonTriggeredMainMenuPopupRef.GetComponent<Image>().spriteName = sender.GetComponent<Image>().spriteName;
//                    UIGlobalVariablesScript.ButtonTriggeredMainMenuPopupRef.GetComponent<Button>().normalSprite = sender.GetComponent<Image>().spriteName;
                    //UIGlobalVariablesScript.ButtonTriggeredMainMenuPopupRef.GetComponent<Image>().MakePixelPerfect();
                    //UIGlobalVariablesScript.ButtonTriggeredMainMenuPopupRef.GetComponent<Image>().MarkAsChanged();
                    //this.GetComponent<Image>().spriteName = spriteName;
                    //this.GetComponent<Image>().MarkAsChanged();

                    //NGUITools.MarkParentAsChanged(UIGlobalVariablesScript.ButtonTriggeredMainMenuPopupRef);
			
                    break;
                }
			
            case UIFunctionalityId.OpenMinigamesScreen:
                {
                    if (UIGlobalVariablesScript.Singleton.MainMenuPopupObjectRef.activeInHierarchy)
                        UIGlobalVariablesScript.Singleton.MainMenuPopupObjectRef.SetActive(false);

                    UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().Stop(true);

                    UIGlobalVariablesScript.Singleton.CaringScreenRef.SetActive(false);
                    UIGlobalVariablesScript.Singleton.StartMinigameScreenRef.SetActive(false);

                    UIGlobalVariablesScript.Singleton.MinigamesMenuMasterScreenRef.SetActive(true);
                    UIGlobalVariablesScript.Singleton.MinigameMenuScreeRef.SetActive(true);


                    //TrackVuforiaScript.EnableDisableMinigamesBasedOnARStatus();

			
                    break;
                }
			
            case UIFunctionalityId.BackFromMinigames:
                {
                    UIGlobalVariablesScript.Singleton.MinigamesMenuMasterScreenRef.SetActive(false);
                    UIGlobalVariablesScript.Singleton.CaringScreenRef.SetActive(true);


                    break;
                }

            case UIFunctionalityId.CloseCurrentMinigame:
                {
                    if (UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef != null)
                        BetweenSceneData.Instance.minigame = BetweenSceneData.Minigame.Collector;
                    if (UIGlobalVariablesScript.Singleton.GunGameScene != null)
                        BetweenSceneData.Instance.minigame = BetweenSceneData.Minigame.Guns;
                    BetweenSceneData.Instance.ReturnFromMiniGame = true;
                    if (UIGlobalVariablesScript.Singleton.LoadingScreenRef != null)
                    {
                        Debug.Log("LoadingScreenActive");
                        UIGlobalVariablesScript.Singleton.LoadingScreenRef.SetActive(true);
                        UIGlobalVariablesScript.Singleton.MinigameInterruptedMenu.SetActive(false);
                        UIGlobalVariablesScript.Singleton.JoystickArt.SetActive(false);
                        UIGlobalVariablesScript.Singleton.InsideMinigamesMasterScreenRef.SetActive(false);

                        if (UIGlobalVariablesScript.Singleton.GunGameInterface != null)
                            UIGlobalVariablesScript.Singleton.GunGameInterface.SetActive(false);
                    }
                    Application.LoadLevel("VuforiaTest");
                    break;

                }
			
            case UIFunctionalityId.PlayMinigameSpaceship:
            case UIFunctionalityId.PlayMinigameCubeRunners:
            case UIFunctionalityId.PlayMinigameGunFighters:
            case UIFunctionalityId.PlayMinigameUnknown:
                {

                    AchievementManager.Instance.AddToAchievment(AchievementManager.Achievements.PlayMinigames);
                    UIGlobalVariablesScript.SelectedMinigameToPlay = sender.GetComponent<UIClickButtonMasterScript>().FunctionalityId;

                    GameObject.Find("MultiplayerObject").GetComponent<GameController>().SetSinglePlayer();
                    HandleClick(UIFunctionalityId.StartSelectedMinigame, sender);

                    //UIGlobalVariablesScript.Singleton.StartMinigameScreenRef.SetActive(true);
                    //UIGlobalVariablesScript.Singleton.MinigameMenuScreeRef.SetActive(false);


                    break;
                }

            case UIFunctionalityId.PlaySinglePlayer:
                {
                    GameObject.Find("MultiplayerObject").GetComponent<GameController>().SetSinglePlayer();
                    HandleClick(UIFunctionalityId.StartSelectedMinigame, sender);


                    break;
                }
            case UIFunctionalityId.PlayMultiplayer:
                {
                    UIGlobalVariablesScript.Singleton.MultiplayerOptionsScreen.SetActive(true);
                    UIGlobalVariablesScript.Singleton.StartMinigameScreenRef.SetActive(false);

                    //HandleClick(UIFunctionalityId.StartSelectedMinigame, sender);
                    break;
                }
            case UIFunctionalityId.BackFromStartGameOptions:
                {
                    UIGlobalVariablesScript.Singleton.MultiplayerOptionsScreen.SetActive(false);
                    UIGlobalVariablesScript.Singleton.StartMinigameScreenRef.SetActive(true);
			
                    //HandleClick(UIFunctionalityId.StartSelectedMinigame, sender);
                    break;
                }
            case UIFunctionalityId.JoinRandomGame:
                {
                    GameObject.Find("MultiplayerObject").GetComponent<GameController>().SetMultiplayerJoinRandom();
                    break;
                }
            case UIFunctionalityId.StartFriendGame:
                {
                    GameObject.Find("MultiplayerObject").GetComponent<GameController>().SetMultiplayerStartFriendGame();
                    break;
                }
            case UIFunctionalityId.JoinFriendGame:
                {
                    UIGlobalVariablesScript.Singleton.EnterFriendsNameChat.SetActive(true);
                    break;
                }

            case UIFunctionalityId.ShowLeaderboards:
                {
                    break;
                }
			
            case UIFunctionalityId.OpenStatsScreen:
                {
                    UIGlobalVariablesScript.Singleton.CaringScreenRef.SetActive(false);
                    UIGlobalVariablesScript.Singleton.StatsScreenRef.SetActive(true);
                    UIGlobalVariablesScript.Singleton.MainMenuPopupObjectRef.SetActive(false);
			
                    break;
                }

            case UIFunctionalityId.BackToStatsFromAchievement:
                {
                    UIGlobalVariablesScript.Singleton.AchievementsScreenRef.SetActive(false);
                    UIGlobalVariablesScript.Singleton.StatsScreenRef.SetActive(true);
                    break;
                }
			
            case UIFunctionalityId.BackToCaringFromStats:
                {
                    UIGlobalVariablesScript.Singleton.StatsScreenRef.SetActive(false);
                    UIGlobalVariablesScript.Singleton.CaringScreenRef.SetActive(true);
			
                    break;
                }
			
            case UIFunctionalityId.OpenPictureScreen:
                {
                    UIGlobalVariablesScript.Singleton.PicturesScreenRef.SetActive(true);
                    UIGlobalVariablesScript.Singleton.CaringScreenRef.SetActive(false);

                    UIGlobalVariablesScript.Singleton.PicturesScreenRef.GetComponent<DeviceCameraScript>().ResetCamera();

                    break;
                }
			
            case UIFunctionalityId.ClosePictureScreen:
                {
                    UIGlobalVariablesScript.Singleton.PicturesScreenRef.SetActive(false);
                    UIGlobalVariablesScript.Singleton.CaringScreenRef.SetActive(true);
                    UIGlobalVariablesScript.Singleton.PicturesScreenRef.GetComponent<DeviceCameraScript>().Stop();
                    break;
                }
			
            case UIFunctionalityId.CloseStartMinigameScreen:
                {
                    UIGlobalVariablesScript.Singleton.StartMinigameScreenRef.SetActive(false);
                    UIGlobalVariablesScript.Singleton.MinigameMenuScreeRef.SetActive(true);
			
                    break;
                }
			
            case UIFunctionalityId.StartSelectedMinigame:
                {
                    //SavedScale = UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.localScale;
                    //Debug.Log("SavedScale 1:" + SavedScale.ToString());

                    UIGlobalVariablesScript.Singleton.CaringScreenRef.SetActive(false);
                    UIGlobalVariablesScript.Singleton.MinigameMenuScreeRef.SetActive(false);
                    UIGlobalVariablesScript.Singleton.LoadingScreenRef.SetActive(true);

			
                    switch (UIGlobalVariablesScript.SelectedMinigameToPlay)
                    {
                        case UIFunctionalityId.PlayMinigameCubeRunners:
                            Application.LoadLevel("VuforiaTestMinigame1");
                            break;
                        case UIFunctionalityId.PlayMinigameGunFighters:
                            Application.LoadLevel("VuforiaTestMinigame2");
                            break;
                            break;
                    }
                    break;
                }


            case UIFunctionalityId.PauseGame:
                {
                    UIGlobalVariablesScript.Singleton.PauseGameButton.SetActive(false);
                    UIGlobalVariablesScript.Singleton.PausedScreen.SetActive(true);

                    if (UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef != null)
                    {
                        UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>().Paused = true;
                    }
                    else if (UIGlobalVariablesScript.Singleton.GunGameScene != null)
                    {
                        UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().Paused = true;
                    }

                    break;
                }

            case UIFunctionalityId.ResumeGame:
                {
                    UIGlobalVariablesScript.Singleton.PauseGameButton.SetActive(true);
                    UIGlobalVariablesScript.Singleton.PausedScreen.SetActive(false);

                    if (UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef != null)
                    {
                        UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>().Paused = false;
                    }
                    else if (UIGlobalVariablesScript.Singleton.GunGameScene != null)
                    {
                        UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().Paused = false;
                    }

                    break;
                }

            case UIFunctionalityId.CloseSettings:
                {
                    UIGlobalVariablesScript.Singleton.SettingsScreenRef.SetActive(false);
                    UIGlobalVariablesScript.Singleton.StatsScreenRef.SetActive(true);

                    break;
                }
            case UIFunctionalityId.OpenSettings:
                {
                    UIGlobalVariablesScript.Singleton.AchievementsScreenRef.SetActive(false);
                    UIGlobalVariablesScript.Singleton.SettingsScreenRef.SetActive(true);
                    UIGlobalVariablesScript.Singleton.StatsScreenRef.SetActive(false);
                    break;
                }

            case UIFunctionalityId.CloseGamecard:
                {
                    UIGlobalVariablesScript.Singleton.RequiresGamecardScreenRef.SetActive(false);
                    UIGlobalVariablesScript.Singleton.CaringScreenRef.SetActive(true);
                    break;
                }

            case UIFunctionalityId.ShowAchievements:
                {
                    UIGlobalVariablesScript.Singleton.StatsScreenRef.SetActive(false);
                    UIGlobalVariablesScript.Singleton.AchievementsScreenRef.SetActive(true);

                    break;
                }

            case UIFunctionalityId.CloseAchivements:
                {
                    UIGlobalVariablesScript.Singleton.CaringScreenRef.SetActive(true);
                    UIGlobalVariablesScript.Singleton.AchievementsScreenRef.SetActive(false);
                    break;
                }

            case UIFunctionalityId.ChangeCameraFacingMode:
                {
                    UIGlobalVariablesScript.Singleton.ImageTarget.GetComponent<TrackVuforiaScript>().FlipFrontBackCamera();

                    break;
                }

            case UIFunctionalityId.TakePicture:
                {
//			if (Application.platform == RuntimePlatform.IPhonePlayer) 
//			{
//				GameObject.Find("Aperture").GetComponent<Aperture>().Photo();
//			}

                    break;
                }

            case UIFunctionalityId.LighbulbSwitch:
                {
                    //UIWidget widget = UIGlobalVariablesScript.Singleton.LightbulbUI.GetComponent<UIWidget>();

                   // Debug.Log(widget.leftAnchor.target.gameObject.name);
                   // LighbulbSwitchOnOffScript script = widget.leftAnchor.target.gameObject.GetComponent<LighbulbSwitchOnOffScript>();


                    if (UIGlobalVariablesScript.Singleton.LightbulbGreenButton.activeInHierarchy)
                    {
                        UIGlobalVariablesScript.Singleton.LightbulbGreenButton.SetActive(false);
                        UIGlobalVariablesScript.Singleton.LightbulbRedButton.SetActive(true);

                        UIGlobalVariablesScript.Singleton.LightbulbSwitchButton.transform.localPosition = new Vector3(-42, 198, 0);
                        //script.SetOff();
                    }
                    else
                    {
                        UIGlobalVariablesScript.Singleton.LightbulbGreenButton.SetActive(true);
                        UIGlobalVariablesScript.Singleton.LightbulbRedButton.SetActive(false);

                        UIGlobalVariablesScript.Singleton.LightbulbSwitchButton.transform.localPosition = new Vector3(27, 198, 0);
                       // script.SetOn();
                    }



                    break;
                }

            case UIFunctionalityId.ShowAlbum:
                {
                    //GameObject.Find("KamcordPrefab").GetComponent<RecordingGUI>().ShowVideos();
                    break;
                }

            case UIFunctionalityId.RecordVideo:
                {
//			GameObject.Find("KamcordPrefab").GetComponent<RecordingGUI>().StartRecording();
                    sender.GetComponent<UIClickButtonMasterScript>().FunctionalityId = UIFunctionalityId.StopRecordVideo;
                   // sender.GetComponent<Image>().spriteName = "stopvideo";
                    //sender.GetComponent<Button>().normalSprite = "stopvideo";
                    break;
                }

            case UIFunctionalityId.StopRecordVideo:
                {
                    //GameObject.Find("KamcordPrefab").GetComponent<RecordingGUI>().StopRecording();
                    sender.GetComponent<UIClickButtonMasterScript>().FunctionalityId = UIFunctionalityId.RecordVideo;
                   // sender.GetComponent<Image>().spriteName = "video 1";
                    //sender.GetComponent<Button>().normalSprite = "video 1";
                    break;
                }


            case UIFunctionalityId.ClearDragedItem:
                {

			
                    break;
                }

            case UIFunctionalityId.ResumeInterruptedMinigame:
                {
                    UIGlobalVariablesScript.Singleton.MinigameInterruptedMenu.SetActive(false);

                    break;
                }

            case UIFunctionalityId.ExitInterruptedMinigame:
                {
                    UIGlobalVariablesScript.Singleton.MinigameInterruptedMenu.SetActive(false);
                    HandleClick(UIFunctionalityId.CloseCurrentMinigame, sender);
                    UIGlobalVariablesScript.Singleton.Vuforia.OnTrackingLost();
                    break;
                }


            case UIFunctionalityId.PlayPauseSong:
                {
                    UIGlobalVariablesScript.Singleton.UIRoot.GetComponent<MediaPlayerPluginScript>().PlayPause();
                    UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimationControllerScript>().IsDance = true;

                    break;
                }

            case UIFunctionalityId.NextSong:
                {
                    UIGlobalVariablesScript.Singleton.UIRoot.GetComponent<MediaPlayerPluginScript>().NextSong();
                    break;
                }

            case UIFunctionalityId.PreviousSong:
                {
                    UIGlobalVariablesScript.Singleton.UIRoot.GetComponent<MediaPlayerPluginScript>().PreviousSong();
                    break;
                }

            case UIFunctionalityId.ViewTracklist:
                {

                    UIGlobalVariablesScript.Singleton.TracklistPanel.SetActive(true);
                    UIGlobalVariablesScript.Singleton.PlaySongPanel.SetActive(false);

                    break;
                }

            case UIFunctionalityId.ReturnFromTracklist:
                {
                    UIGlobalVariablesScript.Singleton.TracklistPanel.SetActive(false);
                    UIGlobalVariablesScript.Singleton.PlaySongPanel.SetActive(true);
			
                    break;
                }

            case UIFunctionalityId.SelectedTrack:
                {
                    UIGlobalVariablesScript.Singleton.TracklistPanel.SetActive(false);
                    UIGlobalVariablesScript.Singleton.PlaySongPanel.SetActive(true);

                    UIGlobalVariablesScript.Singleton.UIRoot.GetComponent<MediaPlayerPluginScript>().PlaySongAtIndex(
                        sender.GetComponent<TrackSongIndexScript>().TrackIndex);


                    break;
                }

            case UIFunctionalityId.ShowCredits:
                {
                    UIGlobalVariablesScript.Singleton.SettingsScreenRef.SetActive(false);
                    UIGlobalVariablesScript.Singleton.CreditsScreenRef.SetActive(true);
                    break;
                }
            case UIFunctionalityId.HideCredits:
                {
                    UIGlobalVariablesScript.Singleton.CreditsScreenRef.SetActive(false);
                    UIGlobalVariablesScript.Singleton.SettingsScreenRef.SetActive(true);
                    break;
                }

        }
    }

    private void PopulatePopupItems()
    {

    }
}
