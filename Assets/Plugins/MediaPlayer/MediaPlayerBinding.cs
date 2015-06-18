using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;



#if UNITY_IPHONE
public enum MusicRepeatMode
{
	Default,
	None,
	One,
	All
}


public enum MusicShuffleMode
{
	Default,
	Off,
	Songs,
	Albums
}



// All Objective-C exposed methods should be bound here

namespace Prime31
{
	public class MediaPlayerBinding
	{
	    [DllImport("__Internal")]
	    private static extern void _mediaPlayerUseApplicationMusicPlayer( bool shouldUseApplicationPlayer );
	
		// If shouldUseApplicationPlayer is true, an app specific player will be used for playback.  Otherwise, the
		// iPod player will be used and any settings or changes will persist outside of your app.
	    public static void useApplicationMusicPlayer( bool shouldUseApplicationPlayer )
	    {
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				_mediaPlayerUseApplicationMusicPlayer( shouldUseApplicationPlayer );
	    }
		
		
	    [DllImport("__Internal")]
	    private static extern void _mediaPlayerShowMediaPicker();
	
	    public static void showMediaPicker()
	    {
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				_mediaPlayerShowMediaPicker();
	    }
		
		
		[DllImport("__Internal")]
	    private static extern void _mediaPlayerPlay();
	
	    public static void play()
	    {
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				_mediaPlayerPlay();
	    }
		
		
	    [DllImport("__Internal")]
	    private static extern void _mediaPlayerPause();
	
		public static void pause()
	    {
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				_mediaPlayerPause();
	    }
		
		
	    [DllImport("__Internal")]
	    private static extern void _mediaPlayerStop();
	
	    public static void stop()
	    {
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				_mediaPlayerStop();
	    }
		
		
	    [DllImport("__Internal")]
	    private static extern float _mediaPlayerGetVolume();
	
	    public static float getVolume()
	    {
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				return _mediaPlayerGetVolume();
			return 0.0f;
	    }
		
		
	    [DllImport("__Internal")]
	    private static extern void _mediaPlayerSetVolume( float volume );
	
	    public static void setVolume( float volume )
	    {
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				_mediaPlayerSetVolume( volume );
	    }
		
		
	    [DllImport("__Internal")]
	    private static extern string _mediaPlayerGetCurrentTrack();
	
	    public static MediaPlayerTrack getCurrentTrack()
	    {
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
			{
				string trackString = _mediaPlayerGetCurrentTrack();
				return MediaPlayerTrack.trackFromString( trackString );
			}
			
			return new MediaPlayerTrack();
	    }
	
	
	    [DllImport("__Internal")]
	    private static extern void _mediaPlayerSkipToNextItem();
	
	    public static void skipToNextItem()
	    {
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				_mediaPlayerSkipToNextItem();
	    }
	
	
	    [DllImport("__Internal")]
	    private static extern void _mediaPlayerSkipToPreviousItem();
	
	    public static void skipToPreviousItem()
	    {
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				_mediaPlayerSkipToPreviousItem();
	    }
	
	
	    [DllImport("__Internal")]
	    private static extern void _mediaPlayerSkipToBeginning();
	
	    public static void skipToBeginning()
	    {
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				_mediaPlayerSkipToBeginning();
	    }
	
	
	    [DllImport("__Internal")]
	    private static extern void _mediaPlayerBeginSeekingForward();
	
		// Moves the playback point forward in the media item faster than the normal playback rate
	    public static void beginSeekingForward()
	    {
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				_mediaPlayerBeginSeekingForward();
	    }
	
	
	    [DllImport("__Internal")]
	    private static extern void _mediaPlayerBeginSeekingBackward();
	
		// Moves the playback point backward in the media item faster than the normal playback rate
	    public static void beginSeekingBackward()
	    {
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				_mediaPlayerBeginSeekingBackward();
	    }
	
	
	    [DllImport("__Internal")]
	    private static extern void _mediaPlayerEndSeeking();
	
		// Stops additional movement of the playback point, returning the playback state to what it was prior to seeking
	    public static void endSeeking()
	    {
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				_mediaPlayerEndSeeking();
	    }
	
	
	    [DllImport("__Internal")]
	    private static extern int _mediaPlayerNumberOfItemsInPlaylist();
	
	    public static int numberOfItemsInPlaylist()
	    {
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				return _mediaPlayerNumberOfItemsInPlaylist();
			return 0;
	    }
	
	
	    [DllImport("__Internal")]
	    private static extern bool _mediaPlayerIsPlaying();
	
	    public static bool isPlaying()
	    {
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				return _mediaPlayerIsPlaying();
			return false;
	    }
		
		
	    [DllImport("__Internal")]
	    private static extern string _mediaPlayerGetPlaylists();
	
		// Gets an array of all the playlists on the iDevice
	    public static List<MediaPlayerPlaylist> getPlaylists()
	    {
			List<MediaPlayerPlaylist> items = new List<MediaPlayerPlaylist>();
			
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
			{
				string playlistString = _mediaPlayerGetPlaylists();
				
				// parse out the products
		        string[] playlistParts = playlistString.Split( new string[] { "||||" }, StringSplitOptions.RemoveEmptyEntries );
		        for( int i = 0; i < playlistParts.Length; i++ )
		            items.Add( MediaPlayerPlaylist.playlistFromString( playlistParts[i] ) );
			}
			
			return items;
	    }
		
		
	    [DllImport("__Internal")]
	    private static extern void _mediaPlayerPlayPlaylist( string playlistId );
	
		// Plays the playlist with the given playlistId
	    public static void playPlaylist( string playlistId )
	    {
	        // Call plugin only when running on real device
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				_mediaPlayerPlayPlaylist( playlistId );
	    }
		
	
	    [DllImport("__Internal")]
	    private static extern void _mediaPlayerSetRepeatAndShuffle( int repeat, int shuffle );
	
		// Sets the repeat and shuffle mode for the media player
	    public static void setRepeatAndShuffle( MusicRepeatMode repeat, MusicShuffleMode shuffle )
	    {
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				_mediaPlayerSetRepeatAndShuffle( (int)repeat, (int)shuffle );
	    }
	
	
	    [DllImport("__Internal")]
	    private static extern bool _mediaPlayerIsiPodMusicPlaying();
	
		// Checks to see if the device is playing iPod music
	    public static bool isiPodMusicPlaying()
	    {
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				return _mediaPlayerIsiPodMusicPlaying();
			return false;
	    }
		
		
		#region Advanced querying and raw audio file methods
		
	    [DllImport("__Internal")]
	    private static extern void _mediaPlayerDoNotAutoPlayWhenPickerFinishes( bool doNotAutoPlay );
	
		// Setting this will make the media picker no longer play when closed. instead it will fire the mediaPickerChoseSongsEvent
		// giving you the ability to load the audio files in Unity
	    public static void doNotAutoPlayWhenPickerFinishes( bool doNotAutoPlay )
	    {
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				_mediaPlayerDoNotAutoPlayWhenPickerFinishes( doNotAutoPlay );
	    }
	
		
	    [DllImport("__Internal")]
	    private static extern string _mediaPlayerQuery( string songTitle, string artist, string album );
	
		// Queries the user's iPod library by songTitle, artist or album. Pass null for anything you dont want to filter by
	    public static List<MediaPlayerTrack> queryLibrary( string songTitle, string artist, string album )
	    {
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
			{
				var result = _mediaPlayerQuery( songTitle, artist, album );
				return MediaPlayerTrack.fromJSON( result );
			}
			
			return new List<MediaPlayerTrack>();
	    }
		
		
	    [DllImport("__Internal")]
	    private static extern void _mediaPlayerExportSongFromLibrary( string assetURL, string filename );
	
		// Exports the song to a file in the Documents directory with the given filename
	    public static void exportSongFromLibrary( string assetURL, string filename )
	    {
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				_mediaPlayerExportSongFromLibrary( assetURL, filename );
	    }
		
		
		[DllImport("__Internal")]
		private static extern void _mediaPlayerExportSongFromLibraryAsWav( string assetURL, string filename, bool useMono, int bitDepth, float sampleRate );
		
		// Exports the song to a file in the Documents directory with the given filename. Unity can only play very specific wav times. Sample rates of 22050 and
		// 44100 are known to work
	    public static void exportSongFromLibraryAsWav( string assetURL, string filename, bool useMono = true, float sampleRate = 22050 )
	    {
	        // Call plugin only when running on real device
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				_mediaPlayerExportSongFromLibraryAsWav( assetURL, filename, true, 16, sampleRate );
	    }
		
		
		[DllImport("__Internal")]
		private static extern void _mediaPlayerExportSongFromLibraryAsAAC( string assetURL, string filename, bool useMono, int bitRate, float sampleRate );
		
		// Exports the song to a file in the Documents directory with the given filename with AAC encoding
	    public static void exportSongFromLibraryAsAAC( string assetURL, string filename, bool useMono = true, int bitRate = 128000, float sampleRate = 44100 )
	    {
	        // Call plugin only when running on real device
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				_mediaPlayerExportSongFromLibraryAsAAC( assetURL, filename, true, bitRate, sampleRate );
	    }
		
		#endregion
	
	}

}
#endif
