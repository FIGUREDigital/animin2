using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;


#if UNITY_IPHONE
namespace Prime31
{
	public class MediaPlayerManager : AbstractManager
	{
		// Fired when user picks songs.  Returns the number of songs they chose
		public static event Action<int> mediaPlayerFinished;

		// Fired when user cancles the media picker
		public static event Action mediaPlayerCancelled;

		// Fired when the current song changes
		public static event Action songChanged;

		// Fired when the picker finishes and doNotUseMediaPlayerToPlayFiles is true
		public static event Action<List<MediaPlayerTrack>> mediaPickerChoseSongsEvent;

		// Fired when a file is successfully exported from the library. The path to the file is returned
		public static event Action<string> exportedFileFromLibraryEvent;

		// Fired when export fails for any reason
		public static event Action<string> exportedFileFromLibraryFailedEvent;



		static MediaPlayerManager()
		{
			AbstractManager.initialize( typeof( MediaPlayerManager ) );
		}


		// The completion handlers paramters are: didSucceed, errorMessage, audioClip
		public static IEnumerator loadAudioFileAtPath( string finalFilePath, Action<bool, string, AudioClip> onComplete )
		{
			if( !finalFilePath.StartsWith( "file:" ) )
				finalFilePath = "file://" + finalFilePath;

			var www = new WWW( finalFilePath );

			yield return www;

			if( www.error != null )
				onComplete( false, www.error, null );

			Debug.Log( "loaded audio file from disk" );
			if( www.audioClip != null )
				onComplete( true, null, www.audioClip );

			www.Dispose();
		}


		public void mediaPlayerDidPickMediaItems( string count )
		{
			if( mediaPlayerFinished != null )
				mediaPlayerFinished( int.Parse( count ) );
		}


		public void mediaPlayerDidCancel( string empty )
		{
			if( mediaPlayerCancelled != null )
				mediaPlayerCancelled();
		}


		public void mediaPlayerSongDidChange( string empty )
		{
			if( songChanged != null )
				songChanged();
		}


		public void mediaPlayerDidPickRawMediaItems( string json )
		{
			if( mediaPickerChoseSongsEvent != null )
			{
				var tracks = MediaPlayerTrack.fromJSON( json );
				foreach( var t in tracks )
					Debug.Log( "t: " + t );
				mediaPickerChoseSongsEvent( tracks );
			}
		}


		public void exportedFileFromLibrary( string filePath )
		{
			if( exportedFileFromLibraryEvent != null )
				exportedFileFromLibraryEvent( filePath );
		}


		public void exportFailed( string error )
		{
			if( exportedFileFromLibraryFailedEvent != null )
				exportedFileFromLibraryFailedEvent( error );
		}

	}

}
#endif
