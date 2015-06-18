using UnityEngine;
using System.Collections.Generic;
using Prime31;



namespace Prime31
{
	public class MediaPlayerGUIManager : MonoBehaviourGUI
	{
		private List<MediaPlayerPlaylist> playlists;
	
	
#if UNITY_IPHONE
		void OnGUI()
		{
			beginColumn();
	
	
			if( GUILayout.Button( "Show Media Picker" ) )
			{
				MediaPlayerBinding.showMediaPicker();
			}
	
	
			if( GUILayout.Button( "Play" ) )
			{
				MediaPlayerBinding.play();
			}
	
	
			if( GUILayout.Button( "Stop" ) )
			{
				MediaPlayerBinding.stop();
			}
	
	
			if( GUILayout.Button( "Pause" ) )
			{
				MediaPlayerBinding.pause();
			}
	
	
			if( GUILayout.Button( "Get Volume" ) )
			{
				float volume = MediaPlayerBinding.getVolume();
				Debug.Log( "MediaPlayer volume: " + volume );
			}
	
	
			if( GUILayout.Button( "Set Volume" ) )
			{
				MediaPlayerBinding.setVolume( 0.1f );
			}
	
	
			if( GUILayout.Button( "Get Playlists" ) )
			{
				this.playlists = MediaPlayerBinding.getPlaylists();
	
				foreach( MediaPlayerPlaylist playlist in playlists )
					Debug.Log( playlist );
			}
	
	
			if( GUILayout.Button( "Play Playlist" ) )
			{
				if( this.playlists == null || this.playlists.Count == 0 )
				{
					Debug.Log( "You have no playlists.  Press 'Get Playlists' and make sure there are playlists on your device" );
					return;
				}
	
				MediaPlayerPlaylist playlist = this.playlists[0];
				MediaPlayerBinding.playPlaylist( playlist.playlistId );
			}
	
	
			if( GUILayout.Button( "Is iPod music playing?" ) )
			{
				Debug.Log( "Is iPod music playing? " + MediaPlayerBinding.isiPodMusicPlaying() );
			}
	
	
			endColumn( true );
	
	
			if( GUILayout.Button( "Get Current Track" ) )
			{
				MediaPlayerTrack track = MediaPlayerBinding.getCurrentTrack();
				Debug.Log( "MediaPlayer track: " + track );
			}
	
	
			if( GUILayout.Button( "Skip to Next Track" ) )
			{
				MediaPlayerBinding.skipToNextItem();
			}
	
	
			if( GUILayout.Button( "Skip to Previous Track" ) )
			{
				MediaPlayerBinding.skipToPreviousItem();
			}
	
	
			if( GUILayout.Button( "Skip to Beginning" ) )
			{
				MediaPlayerBinding.skipToBeginning();
			}
	
	
			if( GUILayout.Button( "Get Total Items in Playlist" ) )
			{
				int itemCount = MediaPlayerBinding.numberOfItemsInPlaylist();
				Debug.Log( "MediaPlayer total items in playlist: " + itemCount );
			}
	
	
			if( GUILayout.Button( "Is Media Player Playing" ) )
			{
				bool isPlaying = MediaPlayerBinding.isPlaying();
				Debug.Log( "MediaPlayer isPlaying " + isPlaying );
			}
	
	
			if( GUILayout.Button( "Play Local Clip" ) )
			{
				AudioSource source = GetComponent<AudioSource>();
				source.Play();
			}
	
	
			if( GUILayout.Button( "Set Repeat/Shuffle" ) )
			{
				MediaPlayerBinding.setRepeatAndShuffle( MusicRepeatMode.All, MusicShuffleMode.Songs );
			}
	
			endColumn();
	
	
			if( bottomRightButton( "Advanced Scene" ) )
			{
				Application.LoadLevel( "MediaPlayerRawScene" );
			}
	
	
		}
	
#endif
	}

}
