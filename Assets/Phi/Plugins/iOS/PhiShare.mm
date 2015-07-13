#include "NSData+Base64.h"
#include "PhiShare.h"
@implementation PhiShare : NSObject

	// Implement creation of singleton
static PhiShare *instance;

+ (id)Instance {
	if (instance == nil)
	{
		instance = [[self alloc] init];
	}
	return instance;
}


-(void) ShareImage: (NSString*)image64 x:(float) x y:(float) y w:(float) w h:(float) h
{
	NSData *imageData = [NSData dataFromBase64String:image64];
	UIImage *image = [[UIImage alloc] initWithData:imageData];
 
	UIActivityViewController *avc = [[UIActivityViewController alloc]initWithActivityItems:@[image] applicationActivities:nil];
	UIViewController *vc =  UnityGetGLViewController();
	
		// Check if iOS 8
	if ( [avc respondsToSelector:@selector(popoverPresentationController)] )
	{
		UIPopoverPresentationController *presentationController = [avc popoverPresentationController];
		presentationController.sourceView = vc.view;
		NSLog(@"bounds = %@", NSStringFromCGRect(vc.view.bounds));
		
		presentationController.sourceRect = CGRectMake(vc.view.bounds.size.width * x, vc.view.bounds.size.height * y,vc.view.bounds.size.width * w, vc.view.bounds.size.height * h);
	}
	[vc presentViewController:avc animated:YES completion:nil];
}


	// Unity interface
extern "C"{
	void ShareImageI(const char * image64, float x, float y, float w, float h){
		[[PhiShare Instance] ShareImage:[NSString stringWithUTF8String: image64] x: x y:y w:w h:h];
	}
}

@end
