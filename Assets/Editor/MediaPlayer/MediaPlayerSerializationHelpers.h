//
//  MediaPlayerSerializationHelpers.h
//  Unity-iPhone
//
//  Created by Mike Desaro on 1/29/12.
//  Copyright (c) 2012 prine31. All rights reserved.
//

#import <Foundation/Foundation.h>
#include <MediaPlayer/MediaPlayer.h>


@interface MPMediaItem(JSON)

- (NSDictionary*)dictionaryRepresentation;

@end



@interface MPMediaItemCollection(JSON)

- (NSMutableArray*)arrayRepresentation;

@end