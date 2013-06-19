//
//  NormLi.c
//  Unity-iPhone
//
//  Created by NormLi 1 on 2013-03-05.
//
//

#include <stdio.h>
#include <GLKit/GLKit.h>
#include <UIKit/UIKit.h>
#include <Foundation/Foundation.h>
// When native code plugin is implemented in .mm / .cpp file, then functions
// should be surrounded with extern "C" block to conform C function naming rules

//Must use for Inheritance
@interface PhotoClass:NSObject
@end

@implementation PhotoClass

//Call Back
- (void)photo:(UIImage*)img didFinishSavingWithError:(NSError*)err contextInfo:(void*)ci {
    NSLog(@"Error: %@", err);
}

//Method
- (void) MovePhoto:(NSString*)_imageName {
    NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
    NSString *documentsDirectory = [paths objectAtIndex:0];
    NSFileManager *fm = [NSFileManager new];
    NSError *err = nil;
    NSArray *ar = [fm contentsOfDirectoryAtPath:documentsDirectory error:&err];
    NSString *fullPath = [documentsDirectory stringByAppendingPathComponent:[NSString stringWithFormat:@"%@", _imageName]];
    UIImage *photoImage = [UIImage imageWithContentsOfFile:fullPath];
    UIImageWriteToSavedPhotosAlbum(photoImage, self, @selector(photo:didFinishSavingWithError:contextInfo:), nil);
}

@end

@interface ConfirmDelegate : NSObject<UIAlertViewDelegate>
@end

@implementation ConfirmDelegate

- (void) alertView:(UIAlertView*)alertView clickedButtonAtIndex:(NSInteger)buttonIndex{
    NSLog(@"Index: %i", buttonIndex);
    if(buttonIndex > 0){
        UnitySendMessage("Main Camera", "ReturnConfirm", "Yes");
    }
    else{
        UnitySendMessage("Main Camera", "ReturnConfirm", "No");
    }
}

@end

// Converts C style string to NSString
NSString* CreateNSString (const char* string)
{
	if (string)
		return [NSString stringWithUTF8String: string];
	else
		return [NSString stringWithUTF8String: ""];
}

// Helper method to create C string copy
char* MakeStringCopy (const char* string)
{
	if (string == NULL)
		return NULL;
	
	char* res = (char*)malloc(strlen(string) + 1);
	strcpy(res, string);
	return res;
}

UIImage* ScreenShot() {
    
    NSInteger myDataLength = 320 * 480 * 4;
    
    // allocate array and read pixels into it.
    GLubyte *buffer = (GLubyte *) malloc(myDataLength);                   //Malloc can't happen per frame
    glReadPixels(0, 0, 320, 480, GL_RGBA, GL_UNSIGNED_BYTE, buffer);
    
    // gl renders "upside down" so swap top to bottom into new array.
    // there's gotta be a better way, but this works.
    GLubyte *buffer2 = (GLubyte *) malloc(myDataLength);
    for(int y = 0; y < 480; y++)
    {
        for(int x = 0; x < 320 * 4; x++)
        {
            buffer2[(479 - y) * 320 * 4 + x] = buffer[y * 4 * 320 + x];
        }
    }
    
    // make data provider with data.
    CGDataProviderRef provider = CGDataProviderCreateWithData(NULL, buffer2, myDataLength, NULL);
    
    // prep the ingredients
    int bitsPerComponent = 8;
    int bitsPerPixel = 32;
    int bytesPerRow = 4 * 320;
    CGColorSpaceRef colorSpaceRef = CGColorSpaceCreateDeviceRGB();
    CGBitmapInfo bitmapInfo = kCGBitmapByteOrderDefault;
    CGColorRenderingIntent renderingIntent = kCGRenderingIntentDefault;
    
    // make the cgimage
    CGImageRef imageRef = CGImageCreate(320, 480, bitsPerComponent, bitsPerPixel, bytesPerRow, colorSpaceRef, bitmapInfo, provider, NULL, NO, renderingIntent);
    
    
    free(buffer2);
    free(buffer);
    
    // then make the uiimage from that
    UIImage *myImage = [UIImage imageWithCGImage:imageRef];
    
    return myImage;
}


extern "C" {
    
    void _FireAlert(const char *title, const char *msg) {
        
        NSString *_title = CreateNSString(title);
        NSString *_msg = CreateNSString(msg);
        
        dispatch_queue_t q = dispatch_get_main_queue();
        
        dispatch_async(q, ^{
            UIApplication *app = [UIApplication sharedApplication];
            UIViewController *mainVC = [[app keyWindow] rootViewController];
            UIAlertView* av = [[UIAlertView alloc] initWithTitle:_title message:_msg delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
            [av show];
            //UIViewController *newVC = [[UIViewController alloc] initWithCoder:nil];
        });
    }
    
    void _ConfirmAlert(const char *socialText){
        
        NSString *_title = CreateNSString(socialText);
        NSString *_msg = @"Would you like to visit \nPeek-AR.com?";
        
        dispatch_queue_t q = dispatch_get_main_queue();
        
        dispatch_async(q, ^{
            UIApplication *app = [UIApplication sharedApplication];
            UIViewController *mainVC = [[app keyWindow] rootViewController];
            id<UIAlertViewDelegate> avd = [[ConfirmDelegate alloc] init];
            UIAlertView* av = [[UIAlertView alloc] initWithTitle:_title message:_msg delegate:avd cancelButtonTitle:@"No" otherButtonTitles:@"Yes", nil];
            [av show];
            //UIViewController *newVC = [[UIViewController alloc] initWithCoder:nil];
        });
    }
    
    void _ImageSaver(const char *imageName) {
        NSString *_imageName = CreateNSString(imageName);
        PhotoClass *pc = [[PhotoClass alloc] init];
        [pc MovePhoto:_imageName];
    }
}
	
