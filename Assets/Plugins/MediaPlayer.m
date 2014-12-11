#import <UIKit/UIKit.h>
#import <MediaPlayer/MediaPlayer.h>
#import "MediaPlayer.h"

extern void UnitySendMessage(const char *, const char *, const char *);

void _setVideo(const char * filename)
{
    NSLog(@"Native plugin: %@", [NSString stringWithUTF8String:filename]);
}

void _playSongAtIndex(int index)
{
    [[AniminMediaPlayer sharedInstance] playSongAtIndex:index];
}

void _play()
{
    [[AniminMediaPlayer sharedInstance] playPause];
}

void _moveToNextSong()
{
    [[AniminMediaPlayer sharedInstance] nextSong];
}

void _moveToPreviousSong()
{
    [[AniminMediaPlayer sharedInstance] previousSong];
}

void _pause()
{
}

float _getProgress()
{
    return  [[AniminMediaPlayer sharedInstance] getProgress];
}

int _initMediaPlayer()
{
    return [[AniminMediaPlayer sharedInstance] initializeAll];
}

char* MakeStringCopy (const char* string)
{
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

const char* _getNextSongFromList()
{
    return MakeStringCopy([[AniminMediaPlayer sharedInstance] getNextSongFromList]);
}

void _saveScreenshotToCameraRoll()
{
    return [[AniminMediaPlayer sharedInstance] saveScreenshotToCameraRoll];

}


@implementation AniminMediaPlayer

static AniminMediaPlayer *sharedInstance = nil;
+(AniminMediaPlayer*)sharedInstance {
    if( !sharedInstance )
        sharedInstance = [[AniminMediaPlayer alloc] init];
    
    return sharedInstance;
}


- (int) initializeAll
{
    nextSongIndex = 0;
    musicPlayer = [MPMusicPlayerController iPodMusicPlayer];
    /*
     
     [volumeSlider setValue:[musicPlayer volume]];
     
     if ([musicPlayer playbackState] == MPMusicPlaybackStatePlaying) {
     
     [playPauseButton setImage:[UIImage imageNamed:@"pauseButton.png"] forState:UIControlStateNormal];
     
     } else {
     
     [playPauseButton setImage:[UIImage imageNamed:@"playButton.png"] forState:UIControlStateNormal];
     }
     */
    
    NSNotificationCenter *notificationCenter = [NSNotificationCenter defaultCenter];
    
    [notificationCenter addObserver: self
                           selector: @selector (handle_NowPlayingItemChanged:)
                               name: MPMusicPlayerControllerNowPlayingItemDidChangeNotification
                             object: musicPlayer];
    
    [notificationCenter addObserver: self
                           selector: @selector (handle_PlaybackStateChanged:)
                               name: MPMusicPlayerControllerPlaybackStateDidChangeNotification
                             object: musicPlayer];
    
    [notificationCenter addObserver: self
                           selector: @selector (handle_VolumeChanged:)
                               name: MPMusicPlayerControllerVolumeDidChangeNotification
                             object: musicPlayer];
    
    [musicPlayer beginGeneratingPlaybackNotifications];
    
    
    //Create a query that will return all songs by The Beatles grouped by album
    //    MPMediaQuery* query = [MPMediaQuery songsQuery];
    //    [query addFilterPredicate:[MPMediaPropertyPredicate predicateWithValue:@"The Beatles" forProperty:MPMediaItemPropertyArtist comparisonType:MPMediaPredicateComparisonEqualTo]];
    //    [query setGroupingType:MPMediaGroupingAlbum];
    
    
    MPMediaQuery *everything = [[MPMediaQuery alloc] init];
    itemsFromGenericQuery = [[everything items] retain];
    
    //Pass the query to the player
    [musicPlayer setQueueWithQuery:everything];
    [musicPlayer prepareToPlay];
    //[musicPlayer play];
    
    // [self updateTrackText];
    
    return itemsFromGenericQuery.count;
}

-(const char*)getNextSongFromList
{
    MPMediaItem* singleSongIndex = [itemsFromGenericQuery objectAtIndex:nextSongIndex];
    NSString *songTitle = [singleSongIndex valueForProperty: MPMediaItemPropertyTitle];
    nextSongIndex++;
    return [songTitle UTF8String];
}

-(void)playSongAtIndex:(int)index
{
    MPMediaItem* singleSongIndex = [itemsFromGenericQuery objectAtIndex:index];
    
    [musicPlayer setNowPlayingItem:singleSongIndex];
    [musicPlayer play];
}

-(float)getProgress
{
    long currentPlaybackTime = musicPlayer.currentPlaybackTime;
    long totalPlaybackTime = [[[musicPlayer nowPlayingItem] valueForProperty: @"playbackDuration"] longValue];
    return ((float)currentPlaybackTime / (float)totalPlaybackTime);
}


- (void) updateTrackText
{
    MPMediaItem *currentItem = [musicPlayer nowPlayingItem];
    NSString *titleString = [currentItem valueForProperty:MPMediaItemPropertyTitle];
    NSString *artistString = [currentItem valueForProperty:MPMediaItemPropertyArtist];
    
    if(artistString == nil) artistString = @"";
    if(titleString == nil) titleString = @"";
    
    UnitySendMessage("UI Root", "UpdateTrackInfo", [artistString UTF8String]);
    UnitySendMessage("UI Root", "UpdateArtistInfo", [titleString UTF8String]);
}

- (void) handle_NowPlayingItemChanged: (id) notification
{
    
    
    [self updateTrackText];
}

- (void) handle_PlaybackStateChanged: (id) notification
{
    MPMusicPlaybackState playbackState = [musicPlayer playbackState];
    
    if (playbackState == MPMusicPlaybackStatePaused)
    {
        // [playPauseButton setImage:[UIImage imageNamed:@"playButton.png"] forState:UIControlStateNormal];
        UnitySendMessage("UI Root", "UpdatePlayingStatus",  "false");
        
    }
    else if (playbackState == MPMusicPlaybackStatePlaying)
    {
        //[playPauseButton setImage:[UIImage imageNamed:@"pauseButton.png"] forState:UIControlStateNormal];
        UnitySendMessage("UI Root", "UpdatePlayingStatus",  "true");
        
    } else if (playbackState == MPMusicPlaybackStateStopped)
    {
        UnitySendMessage("UI Root", "UpdatePlayingStatus",  "false");
        //[playPauseButton setImage:[UIImage imageNamed:@"playButton.png"] forState:UIControlStateNormal];
        [musicPlayer stop];
    }
    
}

- (void) handle_VolumeChanged: (id) notification
{
    //[volumeSlider setValue:[musicPlayer volume]];
}



- (void)showMediaPicker
{
    MPMediaPickerController *mediaPicker = [[MPMediaPickerController alloc] initWithMediaTypes: MPMediaTypeAny];
    
    mediaPicker.delegate = self;
    mediaPicker.allowsPickingMultipleItems = YES;
    mediaPicker.prompt = @"Select songs to play";
    
    UIViewController *rootContoller = [[[[UIApplication sharedApplication] delegate] window] rootViewController];
    
    [rootContoller presentViewController:mediaPicker animated:true completion:nil];
    //[self presentViewController:mediaPicker animated:true completion:nil];
    
    
    
    
    // [self presentViewController:mediaPicker animated:true completion:nil];
    // [self.navigationController presentViewController:mediaPicker animated:YES completion:nil];
    
    
    // [self presentModalViewController:mediaPicker animated:YES];
    // [mediaPicker release];
    
}

- (void) mediaPicker: (MPMediaPickerController *) mediaPicker didPickMediaItems: (MPMediaItemCollection *) mediaItemCollection
{
    
    if (mediaItemCollection) {
        
        [musicPlayer setQueueWithItemCollection: mediaItemCollection];
        [musicPlayer play];
    }
    
    UIViewController *rootContoller = [[[[UIApplication sharedApplication] delegate] window] rootViewController];
    
    [rootContoller dismissViewControllerAnimated:YES completion:^ {
        //
    }];
    
}

- (void) mediaPickerDidCancel: (MPMediaPickerController *) mediaPicker
{
    //[self dismissViewControllerAnimated: YES completion:nil];
    UIViewController *rootContoller = [[[[UIApplication sharedApplication] delegate] window] rootViewController];
    
    [rootContoller dismissViewControllerAnimated:YES completion:^ {
        //
    }];
}




- (void)previousSong
{
    [musicPlayer skipToPreviousItem];
}

- (void)playPause
{
    
    if ([musicPlayer playbackState] == MPMusicPlaybackStatePlaying)
    {
        [musicPlayer pause];
        
    }
    else
    {
        [musicPlayer play];
    }
    
}

- (void)nextSong
{
    [musicPlayer skipToNextItem];
}

- (void) DestroyAll
{
    [[NSNotificationCenter defaultCenter] removeObserver: self
                                                    name: MPMusicPlayerControllerNowPlayingItemDidChangeNotification
                                                  object: musicPlayer];
    
    [[NSNotificationCenter defaultCenter] removeObserver: self
                                                    name: MPMusicPlayerControllerPlaybackStateDidChangeNotification
                                                  object: musicPlayer];
    
    [[NSNotificationCenter defaultCenter] removeObserver: self
                                                    name: MPMusicPlayerControllerVolumeDidChangeNotification
                                                  object: musicPlayer];
    
    [musicPlayer endGeneratingPlaybackNotifications];
}


-(void)saveScreenshotToCameraRoll
{
    UIWindow *keyWindow = [[UIApplication sharedApplication] keyWindow];
    CGRect rect = [keyWindow bounds];
    UIGraphicsBeginImageContext(rect.size);
    CGContextRef context = UIGraphicsGetCurrentContext();
    [keyWindow.layer renderInContext:context];
    UIImage *img = UIGraphicsGetImageFromCurrentImageContext();
    
    // Image to save
    // UIImage *img = [UIImage imageNamed:@"ImageName.png"];
    
    // Request to save the image to camera roll
    UIImageWriteToSavedPhotosAlbum(img, self,
                                   @selector(image:didFinishSavingWithError:contextInfo:), nil);
    
    UIGraphicsEndImageContext();
    
}

- (void)image:(UIImage *)image didFinishSavingWithError:(NSError *)error
  contextInfo:(void *)contextInfo
{
    // Was there an error?
    if (error != NULL)
    {
        // Show error message...
        
    }
    else  // No errors
    {
        // Show message image successfully saved
    }
}

@end