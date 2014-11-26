using UnityEngine;
using System.Collections;

public enum UIFunctionalityId
{
	None = 0,
	OpenOrCloseMainMenuPopup,
	SetActiveItemOnBottomBarAndClose,
	OpenMinigamesScreen,
	OpenStatsScreen,

	BackFromMinigames,
	PlayMinigameSpaceship,
	PlayMinigameCubeRunners,
	PlayMinigameGunFighters,
	PlayMinigameUnknown,

	BackToCaringFromStats,
	Settings,

	TakePicture,
	SharePicture,
	ShowAlbum,
	ShowStickers,
	ClosePictureScreen,

	OpenPictureScreen,
	CloseStartMinigameScreen,
	StartSelectedMinigame,
	CloseCurrentMinigame,
	MoveSpaceshipLeft,
	MoveSpaceshipRight,
	OpenSettings,
	CloseSettings,
	CloseGamecard,
	ShowAchievements,
	CloseAchivements,

	OpenCloseFoods,
	OpenCloseMedicine,
	OpenCloseItems,
	ChangeCameraFacingMode,
	ClearAllGroundItems,
	ClearDragedItem,

	ResumeInterruptedMinigame,
	ExitInterruptedMinigame,
	JumbOnCubeRunner,

	PlayPauseSong,
	NextSong,
	PreviousSong,

	ViewTracklist,
	ReturnFromTracklist,
	SelectedTrack,
	RecordVideo,
	StopRecordVideo,
	ShootBullet,

	LighbulbSwitch,
	PauseGame,
	ResumeGame,
	HomeGame,
	RestartGame,
	AudioOnOffGame,
	BackToStatsFromAchievement,
	PlaySinglePlayer,
	PlayMultiplayer,
	ShowLeaderboards,
	StartFriendGame,
	JoinFriendGame,
	JoinRandomGame,
	BackFromStartGameOptions,
	GoToMainMenuFromGame,
	ShowCredits,
	HideCredits,
	ResetAnimin,
}
