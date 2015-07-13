
@interface PhiShare : NSObject

+ (id) Instance;

- (void) ShareImage:(NSString*)image64 x:(float) x y:(float) y w:(float) w h:(float) h;
@end
