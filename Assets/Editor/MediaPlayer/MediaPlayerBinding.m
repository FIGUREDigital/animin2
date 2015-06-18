//
//  MediaPlayerBinding.m
//  iPodTest
//
//  Created by Mike on 8/22/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import "MediaPlayerManager.h"
#include <MediaPlayer/MediaPlayer.h>


// Converts NSString to C style string by way of copy (Mono will free it)
#define MakeStringCopy( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL

// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]

// Converts C style string to NSString as long as it isnt empty
#define GetStringParamOrNil( _x_ ) ( _x_ != NULL && strlen( _x_ ) ) ? [NSString stringWithUTF8String:_x_] : nil


bool _mediaPlayerIsiPodMusicPlaying()
{
	MPMusicPlayerController *musicPlayer = [MPMusicPlayerController iPodMusicPlayer];
	return ( musicPlayer.playbackState == MPMusicPlaybackStatePlaying );
}


void _mediaPlayerUseApplicationMusicPlayer( bool shouldUseApplicationPlayer )
{
	[[MediaPlayerManager sharedManager] useApplicationMusicPlayer:shouldUseApplicationPlayer];
}


void _mediaPlayerShowMediaPicker()
{
	[[MediaPlayerManager sharedManager] showMediaPicker];
}

	
void _mediaPlayerPlay()
{
	[[MediaPlayerManager sharedManager] play];
}


void _mediaPlayerPause()
{
	[[MediaPlayerManager sharedManager] pause];
}


void _mediaPlayerStop()
{
	[[MediaPlayerManager sharedManager] stop];
}


float _mediaPlayerGetVolume()
{
	return [[MediaPlayerManager sharedManager] getVolume];
}


void _mediaPlayerSetVolume( float volume )
{
	[[MediaPlayerManager sharedManager] setVolume:volume];
}


const char * _mediaPlayerGetCurrentTrack()
{
	NSString *track = [[MediaPlayerManager sharedManager] getCurrentTrack];
	return MakeStringCopy( track );
}


void _mediaPlayerSkipToNextItem()
{
	[[MediaPlayerManager sharedManager] skipToNextItem];
}


void _mediaPlayerSkipToPreviousItem()
{
	[[MediaPlayerManager sharedManager] skipToPreviousItem];
}


void _mediaPlayerSkipToBeginning()
{
	[[MediaPlayerManager sharedManager] skipToBeginning];
}


void _mediaPlayerBeginSeekingForward()
{
	[[MediaPlayerManager sharedManager] beginSeekingForward];
}


void _mediaPlayerBeginSeekingBackward()
{
	[[MediaPlayerManager sharedManager] beginSeekingBackward];
}


void _mediaPlayerEndSeeking()
{
	[[MediaPlayerManager sharedManager] endSeeking];
}


int _mediaPlayerNumberOfItemsInPlaylist()
{
	return [[MediaPlayerManager sharedManager] numberOfItemsInPlaylist];
}


bool _mediaPlayerIsPlaying()
{
	return [[MediaPlayerManager sharedManager] isPlaying];
}


// Manual playback
const char * _mediaPlayerGetPlaylists()
{
	NSString *playlistString = [[MediaPlayerManager sharedManager] getPlaylists];
	
	if( playlistString )
		return MakeStringCopy( playlistString );
	return MakeStringCopy( @"" );
}


void _mediaPlayerPlayPlaylist( const char * playlistId )
{
	unsigned long long rawItemId = strtoull( playlistId, NULL, 0 );
	
	NSNumber *itemId = [NSNumber numberWithUnsignedLongLong:rawItemId];
	[[MediaPlayerManager sharedManager] playPlaylistWithId:itemId];
}


void _mediaPlayerSetRepeatAndShuffle( int repeat, int shuffle )
{
	MPMusicRepeatMode repeatMode = (MPMusicRepeatMode)repeat;
	MPMusicShuffleMode shuffleMode = (MPMusicShuffleMode)shuffle;
	
	[MediaPlayerManager sharedManager].musicPlayer.repeatMode = repeatMode;
	[MediaPlayerManager sharedManager].musicPlayer.shuffleMode = shuffleMode;
}


// querying and advanced
const char * _mediaPlayerQuery( const char * songTitle, const char * artist, const char * album )
{
	NSString *result = [[MediaPlayerManager sharedManager] searchForSongTitle:GetStringParamOrNil( songTitle )
																   albumTitle:GetStringParamOrNil( album )
																	   artist:GetStringParamOrNil( artist )];
	return MakeStringCopy( result );
}


void _mediaPlayerDoNotAutoPlayWhenPickerFinishes( bool doNotUseMediaPlayer )
{
	[MediaPlayerManager sharedManager].doNotUseMediaPlayerToPlayFiles = doNotUseMediaPlayer;
}


void _mediaPlayerExportSongFromLibrary( const char * assetURL, const char * filename )
{
	[[MediaPlayerManager sharedManager] exportAssetAtUrl:GetStringParam( assetURL ) toFile:GetStringParam( filename )];
}


void _mediaPlayerExportSongFromLibraryAsWav( const char * assetURL, const char * filename, BOOL useMono, int bitDepth, float sampleRate )
{
	// working parameters: mono: true, bitDepth: 16, sampleRate: 44100
	NSURL *url = [NSURL URLWithString:GetStringParam( assetURL )];
	[[MediaPlayerManager sharedManager] exportWavAssetAtUrl:url toFile:GetStringParam( filename ) mono:useMono bitDepth:bitDepth sampleRate:sampleRate];
}


void _mediaPlayerExportSongFromLibraryAsAAC( const char * assetURL, const char * filename, BOOL useMono, int bitRate, float sampleRate )
{
	// bitRate of 128000, sampleRage of 44100
	NSURL *url = [NSURL URLWithString:GetStringParam( assetURL )];
	[[MediaPlayerManager sharedManager] exportAACAssetAtUrl:url toFile:GetStringParam( filename ) mono:useMono bitRate:bitRate sampleRate:sampleRate];
}



