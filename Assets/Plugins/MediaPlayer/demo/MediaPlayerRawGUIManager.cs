using UnityEngine;
using System.Collections;
using Prime31;


namespace Prime31
{
	[RequireComponent( typeof( AudioSource ) )]
	public class MediaPlayerRawGUIManager : MonoBehaviourGUI
	{
#if UNITY_IPHONE
		// example of listening to the exported event. we do this so that we can play the exported audio file
		void OnEnable()
		{
			MediaPlayerManager.exportedFileFromLibraryEvent += fileWasExportedFromLibrary;
		}


		// be a good citizen and remove event listener
		void OnDisable()
		{
			MediaPlayerManager.exportedFileFromLibraryEvent -= fileWasExportedFromLibrary;
		}


		// Example event handler that just starts playing the audio file immediately if it loaded
		void fileWasExportedFromLibrary( string path )
		{
			if( path.Length == 0 )
			{
				Debug.LogError( "Could not export audio file. Bailing on playback" );
				return;
			}

			// only wav and mp3 files are playable in Unity so dont try to play any others
			if( path.EndsWith( "mp3" ) || path.EndsWith( "wav" ) )
			{
				Debug.Log( "file was exported. going to load and play it now" );
				StartCoroutine
				(
					MediaPlayerManager.loadAudioFileAtPath( path, ( didSucceed, error, clip ) =>
					{
						if( !didSucceed )
						{
							Debug.Log( "loading audio file failed with error: " + error );
							return;
						}

						GetComponent<AudioSource>().clip = clip;
						GetComponent<AudioSource>().Play();
					})
				);
			}
		}


		void OnGUI()
		{
			beginColumn();


			if( GUILayout.Button( "Dont Use iPod Player" ) )
			{
				MediaPlayerBinding.doNotAutoPlayWhenPickerFinishes( true );
			}


			if( GUILayout.Button( "Query Artist" ) )
			{
				var results = MediaPlayerBinding.queryLibrary( null, "Shai Hulud", null );
				Debug.Log( "total results: " + results.Count );

				foreach( var item in results )
					Debug.Log( item );
			}


			if( GUILayout.Button( "Query Album" ) )
			{
				var results = MediaPlayerBinding.queryLibrary( null, null, "red" );
				Debug.Log( "total results: " + results.Count );

				foreach( var item in results )
					Debug.Log( item );
			}


			if( GUILayout.Button( "Query All and Export as MP3" ) )
			{
				var results = MediaPlayerBinding.queryLibrary( null, null, null );
				Debug.Log( "total results: " + results.Count );

				// if we have a result (we should) lets export the song from the library
				if( results.Count > 0 )
				{
					var randomTrackIndex = Random.Range( 0, results.Count - 1 );
					var track = results[randomTrackIndex];

					// always ensure the file is playable before exporting!
					if( track.isPlayable )
					{
						Debug.Log( "exporting track: " + track );
						MediaPlayerBinding.exportSongFromLibrary( track.assetURL, "someExportedSong.mp3" );
					}
					else
					{
						Debug.Log( "track is not playable. not exporting as mp3: " + track );
					}
				}
			}


			if( GUILayout.Button( "Query All and Export as WAV" ) )
			{
				var results = MediaPlayerBinding.queryLibrary( null, null, null );
				Debug.Log( "total results: " + results.Count );

				// if we have a result (we should) lets export the song from the library
				if( results.Count > 0 )
				{
					var randomTrackIndex = Random.Range( 0, results.Count - 1 );
					var track = results[randomTrackIndex];

					// always ensure the file is exportable before exporting!
					if( track.isExportable )
					{
						Debug.Log( "exporting track: " + track );
						MediaPlayerBinding.exportSongFromLibraryAsWav( track.assetURL, "someExportedSong.wav" );
					}
					else
					{
						Debug.Log( "track is not exportable. not exporting as WAV: " + track );
					}
				}
			}


			if( GUILayout.Button( "Query All and Export as AAC" ) )
			{
				var results = MediaPlayerBinding.queryLibrary( null, null, null );
				Debug.Log( "total results: " + results.Count );

				// if we have a result (we should) lets export the song from the library
				if( results.Count > 0 )
				{
					var randomTrackIndex = Random.Range( 0, results.Count - 1 );
					var track = results[randomTrackIndex];

					// always ensure the file is exportable before exporting!
					if( track.isExportable )
					{
						Debug.Log( "exporting track: " + track );
						MediaPlayerBinding.exportSongFromLibraryAsAAC( track.assetURL, "someExportedSong.m4a" );
					}
					else
					{
						Debug.Log( "track is not exportable. not exporting as AAC: " + track );
					}
				}
			}


			endColumn();

			if( bottomRightButton( "Back to 1st Scene" ) )
			{
				Application.LoadLevel( "MediaPlayertestScene" );
			}
		}
#endif
	}

}
