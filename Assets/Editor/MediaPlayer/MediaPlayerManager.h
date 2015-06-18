//
//  MediaPlayerManager.h
//  iPodTest
//
//  Created by Mike on 8/22/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import <Foundation/Foundation.h>
#include <MediaPlayer/MediaPlayer.h>


@interface MediaPlayerManager : NSObject <MPMediaPickerControllerDelegate>
{
	MPMusicPlayerController *_musicPlayer;
	MPMediaPickerController *_picker;
	UIViewController *_viewControllerWrapper;
	int _totalItemsInPlaylist;
	BOOL _shouldUseApplicationPlayer;
}
@property (nonatomic, retain) MPMusicPlayerController *musicPlayer;
@property (nonatomic, assign) BOOL doNotUseMediaPlayerToPlayFiles;
@property (nonatomic, assign) id delegate;


+ (MediaPlayerManager*)sharedManager;

+ (NSString*)jsonStringFromObject:(NSObject*)object;


- (void)useApplicationMusicPlayer:(BOOL)shouldUseApplicationPlayer;

- (void)showMediaPicker;

- (BOOL)isPlaying;

- (void)play;

- (void)pause;

- (void)stop;

- (void)skipToNextItem;

- (void)skipToPreviousItem;

- (void)skipToBeginning;

- (void)beginSeekingForward;

- (void)beginSeekingBackward;

- (void)endSeeking;

- (int)numberOfItemsInPlaylist;

- (float)getVolume;

- (void)setVolume:(float)volume;

- (NSString*)getCurrentTrack;


// Manual playback
- (NSString*)getPlaylists;

- (void)playPlaylistWithId:(NSNumber*)itemId;


// querying and file fetching
- (NSString*)searchForSongTitle:(NSString*)title albumTitle:(NSString*)album artist:(NSString*)artist;

- (void)exportAssetAtUrl:(NSString*)sourceUrl toFile:(NSString*)filename;

// working parameters: mono: true, bitDepth: 16, sampleRate: 44100
- (void)exportWavAssetAtUrl:(NSURL*)assetURL toFile:(NSString*)filename mono:(BOOL)useMono bitDepth:(int)bitDepth sampleRate:(float)sampleRate;

// bitRate of 128000, sampleRage of 44100
- (void)exportAACAssetAtUrl:(NSURL*)assetURL toFile:(NSString*)filename mono:(BOOL)useMono bitRate:(int)bitRate sampleRate:(float)sampleRate;

@end
