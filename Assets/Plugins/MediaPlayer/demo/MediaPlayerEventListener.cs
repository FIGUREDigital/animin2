using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace Prime31
{
	public class MediaPlayerEventListener : MonoBehaviour
	{
#if UNITY_IPHONE
		void OnEnable()
		{
			// Listen to all events for illustration purposes
			MediaPlayerManager.mediaPlayerFinished += mediaPlayerFinished;
			MediaPlayerManager.mediaPlayerCancelled += mediaPlayerCancelled;
			MediaPlayerManager.songChanged += songChanged;
			MediaPlayerManager.mediaPickerChoseSongsEvent += mediaPickerChoseSongsEvent;
			MediaPlayerManager.exportedFileFromLibraryEvent += exportedFileFromLibraryEvent;
			MediaPlayerManager.exportedFileFromLibraryFailedEvent += exportedFileFromLibraryFailedEvent;
		}
			
		
		void OnDisable()
		{
			// Remove all event handlers
			MediaPlayerManager.mediaPlayerFinished -= mediaPlayerFinished;
			MediaPlayerManager.mediaPlayerCancelled -= mediaPlayerCancelled;
			MediaPlayerManager.songChanged -= songChanged;
			MediaPlayerManager.mediaPickerChoseSongsEvent -= mediaPickerChoseSongsEvent;
			MediaPlayerManager.exportedFileFromLibraryEvent -= exportedFileFromLibraryEvent;
			MediaPlayerManager.exportedFileFromLibraryFailedEvent -= exportedFileFromLibraryFailedEvent;
		}
		
		
		void mediaPlayerFinished( int count )
		{
			Debug.Log( "chose " + count.ToString() + " songs" );
		}
		
		
		void mediaPlayerCancelled()
		{
			Debug.Log( "mediaPlayerCancelled" );
		}
		
		
		void songChanged()
		{
			Debug.Log( "song changed" );
		}
		
		
		void mediaPickerChoseSongsEvent( List<MediaPlayerTrack> tracks )
		{
			Debug.Log( "mediaPickerChoseSongsEvent. total tracks: " + tracks.Count );
		}
		
		
		void exportedFileFromLibraryEvent( string file )
		{
			Debug.Log( "exportedFileFromLibraryEvent: " + file );
		}
		
		
		void exportedFileFromLibraryFailedEvent( string error )
		{
			Debug.Log( "exportedFileFromLibraryFailedEvent: " + error );
		}
#endif	
	}

}
