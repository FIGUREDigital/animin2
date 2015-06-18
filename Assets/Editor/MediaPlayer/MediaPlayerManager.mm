//
//  MediaPlayerManager.m
//  iPodTest
//
//  Created by prime[31]
//  Copyright 2015 prime[31]. All rights reserved.
//

#import "MediaPlayerManager.h"
#import "MediaPlayerSerializationHelpers.h"
#include <QuartzCore/QuartzCore.h>
#include <AudioToolbox/AudioToolbox.h>
#import <AVFoundation/AVFoundation.h>


#if UNITY_VERSION < 500
void UnityPause( bool pause );
void UnitySetAudioSessionActive( bool active );
#else
void UnityPause( int pause );
void UnitySetAudioSessionActive( int active );
#endif

UIViewController *UnityGetGLViewController();



@interface MediaPlayerManager()
@property (nonatomic, retain) AVAssetReader *assetReader;
@property (nonatomic, retain) AVAssetReaderOutput *assetReaderOutput;
@property (nonatomic, retain) AVAssetWriterInput *assetWriterInput;
@property (nonatomic, retain) AVAssetWriter *assetWriter;
@property (nonatomic, retain) NSString *exportPath;
@end


@implementation MediaPlayerManager

@synthesize musicPlayer = _musicPlayer, doNotUseMediaPlayerToPlayFiles, delegate, assetReader, assetReaderOutput, assetWriter, assetWriterInput, exportPath;

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSObject

+ (MediaPlayerManager*)sharedManager
{
	static MediaPlayerManager *sharedSingleton;
	
	if( !sharedSingleton )
		sharedSingleton = [[MediaPlayerManager alloc] init];
	
	return sharedSingleton;
}


- (id)init
{
	if( ( self = [super init] ) )
	{
		// default to app player
		_shouldUseApplicationPlayer = YES;
	}
	return self;
}


+ (NSString*)jsonStringFromObject:(NSObject*)object
{
	NSError *error = nil;
	NSData *jsonData = [NSJSONSerialization dataWithJSONObject:object options:0 error:&error];
	if( jsonData && !error )
		return [[[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding] autorelease];
	else
		NSLog( @"jsonData was null, error: %@", [error localizedDescription] );

	return @"{}";
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Private

- (MPMusicPlayerController*)musicPlayer
{
	// Default the first access of the player ot the applicationMusicPlayer if the user hasn't chosen directly
	if( !_musicPlayer )
	{
		if( _shouldUseApplicationPlayer )
		{
			// Turn off shuffle and turn repeat all on
			[_musicPlayer setShuffleMode:MPMusicShuffleModeOff];
			[_musicPlayer setRepeatMode:MPMusicRepeatModeAll];
			self.musicPlayer = [MPMusicPlayerController applicationMusicPlayer];
		}
		else
		{
			self.musicPlayer = [MPMusicPlayerController iPodMusicPlayer];
		}
		
		[[NSNotificationCenter defaultCenter] addObserver:self
												 selector:@selector(onSongChanged:)
													 name:MPMusicPlayerControllerNowPlayingItemDidChangeNotification
												   object:_musicPlayer];
		[_musicPlayer beginGeneratingPlaybackNotifications];
	}
	
	return _musicPlayer;
}


- (void)cleanupWavExport
{
	self.assetReader = nil;
	self.assetReaderOutput = nil;
	self.assetWriter = nil;
	self.assetWriterInput = nil;
	self.exportPath = nil;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSNotification

- (void)onSongChanged:(NSNotification*)note
{
	UnitySendMessage( "MediaPlayerManager", "mediaPlayerSongDidChange", "" );
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Public

- (void)useApplicationMusicPlayer:(BOOL)shouldUseApplicationPlayer
{
	// Kill the music player if we have one
	if( _musicPlayer )
	{
		[_musicPlayer stop];
		[_musicPlayer release];
		_musicPlayer = nil;
	}
	
	_shouldUseApplicationPlayer = shouldUseApplicationPlayer;
}


- (void)showMediaPicker
{
	UnityPause( true );
	
	// Create the picker controller
	_picker = [[MPMediaPickerController alloc] initWithMediaTypes:MPMediaTypeMusic];
	_picker.delegate = self;
	_picker.allowsPickingMultipleItems = YES;
	_picker.prompt = @"Add songs to play";

	// We need to display this in a popover on iPad
	if( NO && UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad )
		_picker.modalPresentationStyle = UIModalPresentationFormSheet;
	
	// show the picker
	[UnityGetGLViewController() presentViewController:_picker animated:YES completion:nil];
	[_picker release];
}


- (BOOL)isPlaying
{
	return ( [self.musicPlayer playbackState] == MPMusicPlaybackStatePlaying );
}


- (void)play
{	
	MPMusicPlaybackState playbackState = [self.musicPlayer playbackState];
	
	// if stopped or paused start playing
	if( playbackState == MPMusicPlaybackStateStopped || playbackState == MPMusicPlaybackStatePaused )
		[self.musicPlayer play];
}


- (void)pause
{
	MPMusicPlaybackState playbackState = [self.musicPlayer playbackState];
	
	// if playing, pause
	if( playbackState == MPMusicPlaybackStatePlaying )
		[self.musicPlayer pause];
}


- (void)stop
{
	MPMusicPlaybackState playbackState = [self.musicPlayer playbackState];
	
	// if playing or paused, stop
	if( playbackState == MPMusicPlaybackStatePlaying || playbackState == MPMusicPlaybackStatePaused )
		[self.musicPlayer stop];
}


- (void)skipToNextItem
{
	[self.musicPlayer skipToNextItem];
}


- (void)skipToPreviousItem
{
	[self.musicPlayer skipToPreviousItem];
}


- (void)skipToBeginning
{
	[self.musicPlayer skipToBeginning];
}


- (void)beginSeekingForward
{
	[self.musicPlayer beginSeekingForward];
}


- (void)beginSeekingBackward
{
	[self.musicPlayer beginSeekingBackward];
}


- (void)endSeeking
{
	[self.musicPlayer endSeeking];
}


- (int)numberOfItemsInPlaylist
{
	return _totalItemsInPlaylist;
}


- (float)getVolume
{
	return self.musicPlayer.volume;
}


- (void)setVolume:(float)volume
{
	self.musicPlayer.volume = volume;
}


- (NSString*)getCurrentTrack
{
	MPMediaItem *currentTrack = self.musicPlayer.nowPlayingItem;
	
	if( !currentTrack )
		return @"|||";
	return [NSString stringWithFormat:@"%@|||%@", [currentTrack valueForProperty:MPMediaItemPropertyArtist], [currentTrack valueForProperty:MPMediaItemPropertyTitle]];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Manual Playback

- (NSString*)getPlaylists
{
	MPMediaQuery *query = [MPMediaQuery playlistsQuery];
	
	// Grab all the playlists and return the id and title
	NSMutableString *resultString = [NSMutableString string];
	NSArray *collections = [query collections];
	
	for( MPMediaPlaylist *playlistItem in collections )
	{
		// uint64_t (unsigned long long)
		NSNumber *itemId = [playlistItem valueForProperty:MPMediaPlaylistPropertyPersistentID];
		NSString *playlistTitle = [playlistItem valueForProperty:MPMediaPlaylistPropertyName];

		[resultString appendFormat:@"%llu|||%@||||", [itemId unsignedLongLongValue], playlistTitle];
	}

	return resultString;
}


- (void)playPlaylistWithId:(NSNumber*)itemId
{
	// query and get the playlist we are looking for
	MPMediaQuery *query = [MPMediaQuery playlistsQuery];
	
	MPMediaPropertyPredicate *predicate = [MPMediaPropertyPredicate predicateWithValue:itemId
																		   forProperty:MPMediaPlaylistPropertyPersistentID];
	
	[query addFilterPredicate:predicate];
	
	NSArray *collections = [query collections];
	if( collections.count == 0 )
		return;
	
	MPMediaItemCollection *mediaItemCollection = [collections objectAtIndex:0];
	
	// save the number of playlist items the user chose
	_totalItemsInPlaylist = mediaItemCollection.count;
	[self.musicPlayer setQueueWithItemCollection:mediaItemCollection];
	[self.musicPlayer play];

	// hack that should no longer be needed. leaving it here in case Unity audio breaks again
	//[self performSelector:@selector(shareAudioWithUnity) withObject:nil afterDelay:4.0];
}


// hack that should no longer be needed. leaving it here in case Unity audio breaks again
//- (void)shareAudioWithUnity
//{
//	// share audio with Unity
//	UnitySetAudioSessionActive( true );
//}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - Querying and file fetching

- (NSString*)searchForSongTitle:(NSString*)title albumTitle:(NSString*)album artist:(NSString*)artist
{
	MPMediaQuery *everything = [[[MPMediaQuery alloc] init] autorelease];
	
	if( title )
	{
		MPMediaPropertyPredicate *predicate = [MPMediaPropertyPredicate predicateWithValue:title forProperty:MPMediaItemPropertyTitle];
		[everything addFilterPredicate:predicate];
	}
	
	if( album )
	{
		MPMediaPropertyPredicate *predicate = [MPMediaPropertyPredicate predicateWithValue:album forProperty:MPMediaItemPropertyAlbumTitle];
		[everything addFilterPredicate:predicate];
	}
	
	if( artist )
	{
		MPMediaPropertyPredicate *predicate = [MPMediaPropertyPredicate predicateWithValue:artist forProperty:MPMediaItemPropertyArtist];
		[everything addFilterPredicate:predicate];
	}
	
	NSArray *queryResult = [everything items];
	
	// serialize the items
	NSMutableArray *items = [NSMutableArray arrayWithCapacity:queryResult.count];
	
	for( MPMediaItem *item in queryResult )
		[items addObject:[item dictionaryRepresentation]];
	
	return [MediaPlayerManager jsonStringFromObject:items];
}


- (BOOL)isAssetClearForExport:(AVURLAsset*)asset
{
	// do some basic checks on the asset
	if( asset.hasProtectedContent )
	{
		UnitySendMessage( "MediaPlayerManager", "exportFailed", "audio file contains protected content" );
		return NO;
	}


	if( !asset.isReadable )
	{
		UnitySendMessage( "MediaPlayerManager", "exportFailed", "audio file contains protected content" );
		return NO;
	}


	if( !asset.isExportable )
	{
		UnitySendMessage( "MediaPlayerManager", "exportFailed", "audio file is not exportable" );
		return NO;
	}

	return YES;
}


- (void)exportAssetAtUrl:(NSString*)sourceUrl toFile:(NSString*)filename
{
    NSURL *url = [NSURL URLWithString:sourceUrl];
	
    AVURLAsset *asset = [AVURLAsset URLAssetWithURL:url options:nil];
	if( ![self isAssetClearForExport:asset] )
		return;
	

	
	// for debugging purposes though Apple doesn't list actual file types here for some reason
	//NSArray *compatibleExportTypes = [AVAssetExportSession exportPresetsCompatibleWithAsset:asset];
	//NSLog( @"compatibleExportTypes: %@", compatibleExportTypes );
	
    AVAssetExportSession *exporter = [[AVAssetExportSession alloc] initWithAsset:asset
																	  presetName:AVAssetExportPresetPassthrough];
	
	if( [exporter respondsToSelector:@selector(determineCompatibleFileTypesWithCompletionHandler:) ] )
		[exporter determineCompatibleFileTypesWithCompletionHandler:^( NSArray *compatibleFileTypes )
		{
			NSLog( @"compatibleFileTypes: %@", compatibleFileTypes );
		}];
	
    //exporter.outputFileType = @"public.mpeg-4"; // this no longer work as of iOS 5.1 and 6
	
	// hack: export as a quicktime movie then rename the file back to mp3 later
	exporter.outputFileType = @"com.apple.quicktime-movie";
	filename = [filename stringByAppendingString:@".mov"];
    NSString *exportFile = [NSHomeDirectory() stringByAppendingPathComponent:[NSString stringWithFormat:@"/Documents/%@", filename]];
	
	if( [[NSFileManager defaultManager] fileExistsAtPath:exportFile] )
		[[NSFileManager defaultManager] removeItemAtPath:exportFile error:nil];
	
    exporter.outputURL = [NSURL fileURLWithPath:exportFile];
	
    // do the export
    [exporter exportAsynchronouslyWithCompletionHandler:
	^{
		switch( exporter.status )
		{
			case AVAssetExportSessionStatusWaiting:
			case AVAssetExportSessionStatusExporting:
			case AVAssetExportSessionStatusCancelled:
			{
				break;
			}
			case AVAssetExportSessionStatusFailed:
			{
				UnitySendMessage( "MediaPlayerManager", "exportFailed", exporter.error.localizedDescription.UTF8String );
				break;
			}
			case AVAssetExportSessionStatusCompleted:
			{
				if( [[NSFileManager defaultManager] fileExistsAtPath:exportFile] )
				{
					// rename the file
					NSString *fixedPath = [exportFile stringByReplacingOccurrencesOfString:@".mov" withString:@""];
					[[NSFileManager defaultManager] moveItemAtPath:exportFile toPath:fixedPath error:nil];
					UnitySendMessage( "MediaPlayerManager", "exportedFileFromLibrary", fixedPath.UTF8String );
				}
				else
				{
					UnitySendMessage( "MediaPlayerManager", "exportFailed", "" );
				}
				break;
			}
			case AVAssetExportSessionStatusUnknown:
			{
				UnitySendMessage( "MediaPlayerManager", "exportFailed", "unknown status" );
				break;
			}
		}
	}];
}


// working parameters: mono: true, bitDepth: 16, sampleRate: 44100
- (void)exportWavAssetAtUrl:(NSURL*)assetURL toFile:(NSString*)filename mono:(BOOL)useMono bitDepth:(int)bitDepth sampleRate:(float)sampleRate
{
	int numberOfChannels;
	AudioChannelLayout channelLayout;
	memset( &channelLayout, 0, sizeof( AudioChannelLayout ) );
	
	if( useMono )
	{
		numberOfChannels = 1;
		channelLayout.mChannelLayoutTag = kAudioChannelLayoutTag_Mono;
	}
	else
	{
		numberOfChannels = 2;
		channelLayout.mChannelLayoutTag = kAudioChannelLayoutTag_Stereo;
	}
	
	
	NSDictionary *outputSettings;
	outputSettings = [NSDictionary dictionaryWithObjectsAndKeys:
					  [NSNumber numberWithInt:kAudioFormatLinearPCM], AVFormatIDKey,
					  [NSNumber numberWithFloat:sampleRate], AVSampleRateKey,
					  [NSNumber numberWithInt:numberOfChannels], AVNumberOfChannelsKey,
					  [NSData dataWithBytes:&channelLayout length:sizeof(AudioChannelLayout)], AVChannelLayoutKey,
					  [NSNumber numberWithInt:bitDepth], AVLinearPCMBitDepthKey,
					  [NSNumber numberWithBool:NO], AVLinearPCMIsNonInterleaved,
					  [NSNumber numberWithBool:NO],AVLinearPCMIsFloatKey,
					  [NSNumber numberWithBool:NO], AVLinearPCMIsBigEndianKey,
					  nil];
	
	[self exportAssetAtUrl:assetURL toFile:filename withSettings:outputSettings outputFileType:AVFileTypeWAVE];
}


// bitRate of 128000, sampleRage of 44100
- (void)exportAACAssetAtUrl:(NSURL*)assetURL toFile:(NSString*)filename mono:(BOOL)useMono bitRate:(int)bitRate sampleRate:(float)sampleRate
{
	int numberOfChannels;
	AudioChannelLayout channelLayout;
	memset( &channelLayout, 0, sizeof( AudioChannelLayout ) );
	
	if( useMono )
	{
		numberOfChannels = 1;
		channelLayout.mChannelLayoutTag = kAudioChannelLayoutTag_Mono;
	}
	else
	{
		numberOfChannels = 2;
		channelLayout.mChannelLayoutTag = kAudioChannelLayoutTag_Stereo;
	}
	
	
	NSDictionary *outputSettings;
	outputSettings = [NSDictionary dictionaryWithObjectsAndKeys:
					  [NSNumber numberWithInt:kAudioFormatMPEG4AAC], AVFormatIDKey,
					  [NSNumber numberWithFloat:sampleRate], AVSampleRateKey,
					  [NSNumber numberWithInt:numberOfChannels], AVNumberOfChannelsKey,
					  [NSData dataWithBytes:&channelLayout length:sizeof(AudioChannelLayout)], AVChannelLayoutKey,
					  [NSNumber numberWithInt:bitRate], AVEncoderBitRateKey,
					  nil];

	[self exportAssetAtUrl:assetURL toFile:filename withSettings:outputSettings outputFileType:AVFileTypeAppleM4A];
}


- (void)exportAssetAtUrl:(NSURL*)assetURL toFile:(NSString*)filename withSettings:(NSDictionary*)settings outputFileType:(NSString*)outputFileType
{
	// set up an AVAssetReader to read from the iPod Library
	AVURLAsset *songAsset = [AVURLAsset URLAssetWithURL:assetURL options:nil];
	
	// do some basic checks on the asset
	if( ![self isAssetClearForExport:songAsset] )
		return;


	NSError *assetError = nil;
	self.assetReader = [AVAssetReader assetReaderWithAsset:songAsset error:&assetError];
	if( assetError )
	{
		UnitySendMessage( "MediaPlayerManager", "exportFailed", assetError.localizedDescription.UTF8String );
		[self cleanupWavExport];
		return;
	}
	
	self.assetReaderOutput = [AVAssetReaderAudioMixOutput  assetReaderAudioMixOutputWithAudioTracks:songAsset.tracks audioSettings:nil];
	if( ![self.assetReader canAddOutput:self.assetReaderOutput] )
	{
		UnitySendMessage( "MediaPlayerManager", "exportFailed", "can't add reader output" );
		[self cleanupWavExport];
		return;
	}
	[self.assetReader addOutput:self.assetReaderOutput];
	
	
	// where are we going to export the file
	self.exportPath = [NSHomeDirectory() stringByAppendingPathComponent:[NSString stringWithFormat:@"/Documents/%@", filename]];
	if( [[NSFileManager defaultManager] fileExistsAtPath:exportPath] )
		[[NSFileManager defaultManager] removeItemAtPath:exportPath error:nil];
	
	NSURL *exportURL = [NSURL fileURLWithPath:exportPath];
	self.assetWriter = [AVAssetWriter assetWriterWithURL:exportURL fileType:outputFileType error:&assetError];
	if( assetError )
	{
		UnitySendMessage( "MediaPlayerManager", "exportFailed", assetError.localizedDescription.UTF8String );
		[self cleanupWavExport];
		return;
	}
	

	self.assetWriterInput = [AVAssetWriterInput assetWriterInputWithMediaType:AVMediaTypeAudio outputSettings:settings];
	if( [self.assetWriter canAddInput:self.assetWriterInput] )
	{
		[self.assetWriter addInput:self.assetWriterInput];
	}
	else
	{
		UnitySendMessage( "MediaPlayerManager", "exportFailed", "can't add writer input" );
		[self cleanupWavExport];
		return;
	}
	
	self.assetWriterInput.expectsMediaDataInRealTime = NO;
	
	[self.assetWriter startWriting];
	[self.assetReader startReading];
	
	AVAssetTrack *soundTrack = [songAsset.tracks objectAtIndex:0];
	CMTime startTime = CMTimeMake( 0, soundTrack.naturalTimeScale );
	[self.assetWriter startSessionAtSourceTime:startTime];
	
	dispatch_queue_t mediaInputQueue = dispatch_queue_create( "conversionQueue", NULL );
	__block MediaPlayerManager *blockSelf = self;
	[self.assetWriterInput requestMediaDataWhenReadyOnQueue:mediaInputQueue usingBlock:^
	 {
		 while( blockSelf.assetWriterInput.readyForMoreMediaData )
		 {
			 if( blockSelf.assetReader.status == AVAssetReaderStatusReading )
			 {
				 CMSampleBufferRef nextBuffer = [blockSelf.assetReaderOutput copyNextSampleBuffer];
				 if( nextBuffer )
				 {
					 // append buffer
					 [blockSelf.assetWriterInput appendSampleBuffer:nextBuffer];
					
					 CFRelease( nextBuffer );
				 }
				 else
				 {
					 [blockSelf.assetWriterInput markAsFinished];
					 [blockSelf.assetWriter finishWritingWithCompletionHandler:
					  ^{
						 dispatch_sync( dispatch_get_main_queue(),
						   ^{
							   // either call the delegate or send the message back to Unity
							   if( delegate && [delegate respondsToSelector:@selector(exportComplete:)] )
								   [delegate performSelector:@selector(exportComplete:) withObject:blockSelf.exportPath];
							   else
								   UnitySendMessage( "MediaPlayerManager", "exportedFileFromLibrary", blockSelf.exportPath.UTF8String );
						   });

						 [blockSelf cleanupWavExport];
					 }];
					
					 break;
				 }
			 }
		 }
	 }];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark MPMediaPickerControllerDelegate

- (void)mediaPicker:(MPMediaPickerController*)mediaPicker didPickMediaItems:(MPMediaItemCollection*)mediaItemCollection
{
    // Add 'collection' to the music player's playback queue, but only if the user chose at least one song to play.
    if( mediaItemCollection )
	{
		// if we are playing all audio through Unity we just return the song list
		if( doNotUseMediaPlayerToPlayFiles )
		{
			UnitySendMessage( "MediaPlayerManager", "mediaPlayerDidPickRawMediaItems", [MediaPlayerManager jsonStringFromObject:mediaItemCollection.arrayRepresentation].UTF8String );
		}
		else
		{
			// save the number of playlist items the user chose
			_totalItemsInPlaylist = mediaItemCollection.count;
			[self.musicPlayer setQueueWithItemCollection:mediaItemCollection];
			[self.musicPlayer play];
			
			NSString *totalItems = [NSString stringWithFormat:@"%i", _totalItemsInPlaylist];
			UnitySendMessage( "MediaPlayerManager", "mediaPlayerDidPickMediaItems", [totalItems UTF8String] );
		}
    }
	else
	{
		// nothing chosen. count this as a cancel
		UnitySendMessage( "MediaPlayerManager", "mediaPlayerDidCancel", "" );
	}
	
	// dismiss the picker and unpause
	[UnityGetGLViewController() dismissViewControllerAnimated:YES completion:nil];
	UnityPause( false );
}


- (void)mediaPickerDidCancel:(MPMediaPickerController*)mediaPicker
{
	_totalItemsInPlaylist = 0;
	
	// dismiss the picker and unpause
	[UnityGetGLViewController() dismissViewControllerAnimated:YES completion:nil];
	UnityPause( false );
	
	UnitySendMessage( "MediaPlayerManager", "mediaPlayerDidCancel", "" );
}


@end
