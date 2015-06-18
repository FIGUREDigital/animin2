using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;



namespace Prime31
{
	public class MediaPlayerTrack
	{
		public string artist;
		public string title;
		public string assetURL;
		public float duration;
		public string album;
		public string genre;
		public string composer;
		public bool hasProtectedContent;
		public bool isExportable;
		public bool isReadable;
	
	
		public bool isPlayable
		{
			get
			{
				// protected content is not exportable
				if( hasProtectedContent || assetURL == null )
					return false;
	
				// only mp3s are exportable
				var assetIsMp3 = assetURL.Contains( ".mp3" );
	
				// if isExportable is true then all we need is an mp3
				if( isExportable && assetIsMp3 )
					return true;
	
				// resort to just checking if we have an mp3
				return assetIsMp3;
			}
		}
	
	
		public static List<MediaPlayerTrack> fromJSON( string json )
		{
			var trackList = new List<MediaPlayerTrack>();
	
			// decode the json
			var list = json.listFromJson();
	
			// create DTO's from the Dictionaries
			foreach( Dictionary<string,object> dict in list )
				trackList.Add( new MediaPlayerTrack( dict ) );
	
			return trackList;
		}
	
	
		public static MediaPlayerTrack trackFromString( string trackString )
		{
			MediaPlayerTrack track = new MediaPlayerTrack();
	
	        string[] trackParts = trackString.Split( new string[] { "|||" }, StringSplitOptions.None );
	        if( trackParts.Length == 2 )
	        {
	            track.artist = trackParts[0];
	            track.title = trackParts[1];
	        }
	
			return track;
		}
	
	
		public MediaPlayerTrack(){}
	
	
		public MediaPlayerTrack( Dictionary<string,object> dict )
		{
			if( dict.ContainsKey( "artist" ) )
				artist = dict["artist"].ToString();
	
			if( dict.ContainsKey( "title" ) )
				title = dict["title"].ToString();
	
			if( dict.ContainsKey( "assetURL" ) )
				assetURL = dict["assetURL"].ToString();
	
			if( dict.ContainsKey( "duration" ) )
				duration = float.Parse( dict["duration"].ToString() );
	
			if( dict.ContainsKey( "album" ) )
				album = dict["album"].ToString();
	
			if( dict.ContainsKey( "genre" ) )
				genre = dict["genre"].ToString();
	
			if( dict.ContainsKey( "composer" ) )
				composer = dict["composer"].ToString();
	
			if( dict.ContainsKey( "hasProtectedContent" ) )
				hasProtectedContent = bool.Parse( dict["hasProtectedContent"].ToString() );
	
			if( dict.ContainsKey( "isReadable" ) )
				isReadable = bool.Parse( dict["isReadable"].ToString() );
	
			if( dict.ContainsKey( "isExportable" ) )
				isExportable = bool.Parse( dict["isExportable"].ToString() );
		}
	
	
		public override string ToString()
		{
			 return string.Format( "<MediaPlayerTrack> artist: {0}, title: {1}, duration: {2}, composer: {3}, genre: {4}, album: {5}, hasProtectedContent: {6}, isExportable: {7}, isPlayable: {8}",
				artist, title, duration, composer, genre, album, hasProtectedContent, isExportable, isPlayable );
		}
	
	}

}
