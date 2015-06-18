using UnityEngine;
using System;
using System.Collections;



namespace Prime31
{
	public class MediaPlayerPlaylist : MonoBehaviour
	{
		public string playlistId;
		public string title;
		
		
		public static MediaPlayerPlaylist playlistFromString( string playlistString )
		{
			MediaPlayerPlaylist playlist = new MediaPlayerPlaylist();
			
	        string[] playlistParts = playlistString.Split( new string[] { "|||" }, StringSplitOptions.None );
	        if( playlistParts.Length == 2 )
	        {
	            playlist.playlistId = playlistParts[0];
	            playlist.title = playlistParts[1];
	        }
			
			return playlist;
		}
		
		
		public override string ToString()
		{
			 return string.Format( "<MediaPlayerPlaylist> playlistId: {0}, title: {1}", playlistId, title );
		}
	}

}
