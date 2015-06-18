//
//  MediaPlayerSerializationHelpers.m
//  Unity-iPhone
//
//  Created by Mike Desaro on 1/29/12.
//  Copyright (c) 2012 prine31. All rights reserved.
//

#import "MediaPlayerSerializationHelpers.h"
#import <AVFoundation/AVFoundation.h>
#import "MediaPlayerManager.h"


@implementation MPMediaItem(JSON)

- (NSString*)extractPropertyOrEmptyString:(NSString*)property
{
	NSString *prop = [self valueForKey:property];
	if( !prop )
		prop = @"";
	return prop;
}


- (NSDictionary*)dictionaryRepresentation
{
	AVURLAsset *asset = [AVURLAsset URLAssetWithURL:[self valueForProperty:MPMediaItemPropertyAssetURL] options:nil];
	
	NSDictionary *dict = [NSDictionary dictionaryWithObjectsAndKeys:
						  @(asset.hasProtectedContent), @"hasProtectedContent",
						  @(asset.isExportable), @"isExportable",
						  @(asset.isReadable), @"isReadable",
						
						  [self extractPropertyOrEmptyString:MPMediaItemPropertyArtist], @"artist",
						  [self extractPropertyOrEmptyString:MPMediaItemPropertyTitle], @"title",
						  [self valueForProperty:MPMediaItemPropertyPlaybackDuration], @"duration",
						  [self extractPropertyOrEmptyString:MPMediaItemPropertyGenre], @"genre",
						  [self extractPropertyOrEmptyString:MPMediaItemPropertyAlbumTitle], @"album",
						  [self extractPropertyOrEmptyString:MPMediaItemPropertyComposer], @"composer",
						  [[self valueForProperty:MPMediaItemPropertyAssetURL] absoluteString], @"assetURL", nil];
	
	
	
	return dict;
}

@end



@implementation MPMediaItemCollection(JSON)

- (NSMutableArray*)arrayRepresentation
{
	NSMutableArray *items = [NSMutableArray arrayWithCapacity:self.count];
	
	for( MPMediaItem *item in self.items )
		[items addObject:[item dictionaryRepresentation]];
	
	return items;
}

@end
