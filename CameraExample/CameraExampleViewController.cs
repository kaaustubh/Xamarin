using System;
using System.Drawing;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreAnimation;
using MonoTouch.Foundation;
using MonoTouch.AVFoundation;
using MonoTouch.CoreMedia;
using System.Runtime.InteropServices;


namespace CameraExample
{
	public partial class CameraExampleViewController : UIViewController
	{
		AVCaptureStillImageOutput output;
		AVCaptureConnection captureConnection;
		NSData imageData;
		public CameraExampleViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{

			CALayer _layer = new CALayer ();
			base.ViewDidLoad ();
			var captureSession = new MonoTouch.AVFoundation.AVCaptureSession();
			var previewLayer = new MonoTouch.AVFoundation.AVCaptureVideoPreviewLayer(captureSession)
			{
				LayerVideoGravity = MonoTouch.AVFoundation.AVLayerVideoGravity.ResizeAspectFill,
				Frame = this.View.Bounds
			};
			var device = MonoTouch.AVFoundation.AVCaptureDevice.DefaultDeviceWithMediaType(
				MonoTouch.AVFoundation.AVMediaType.Video);

			NSError error;

			var input = new MonoTouch.AVFoundation.AVCaptureDeviceInput(device, out error);

			captureSession.AddInput(input);

			this.View.Layer.AddSublayer(previewLayer);

			output = new AVCaptureStillImageOutput ();
			output.OutputSettings=NSDictionary.FromObjectAndKey(new NSString("AVVideoCodecKey"), new NSString("AVVideoCodecJPEG"));
			captureSession.AddOutput (output);

			var button = new UIButton(new RectangleF((this.View.Frame.Width-50)/2, this.View.Frame.Height-60, 50, 50));

			button.SetTitle("Click", UIControlState.Normal);



			button.TouchUpInside += HandleButtCaptureImageTouchUpInside;



			this.View.Add(button);

			_layer.Frame = this.View.Bounds;

			_layer.Delegate = new CustomLayerDelegate ();
			_layer.SetNeedsDisplay ();
			this.View.Layer.AddSublayer(_layer);

			captureSession.StartRunning();
			// Perform any additional setup after loading the view, typically from a nib.
		}

		void HandleButtCaptureImageTouchUpInside (object sender, EventArgs e)
		{
			captureConnection = null;

			foreach (AVCaptureConnection capConn in output.Connections)
			{
				foreach (AVCaptureInputPort port in capConn.inputPorts)
				{
					if (port.MediaType == AVMediaType.Video)
					{
						captureConnection = capConn;
						break;
					}
				}
				if (captureConnection != null)
					break;
			}

			output.CaptureStillImageAsynchronously(captureConnection, HandleAVCaptureCompletionHandlercompletionHandler);
//			buttCaptureImage.Enabled = false;
		}

		void HandleAVCaptureCompletionHandlercompletionHandler (CMSampleBuffer imageDataSampleBuffer, NSError error)
		{
			try
			{
				using (var pool = new NSAutoreleasePool ()) {
					imageData = AVCaptureStillImageOutput.JpegStillToNSData(imageDataSampleBuffer);
					//imageDataSampleBuffer.Dispose();
//					parentView.DismissModalViewControllerAnimated(true);
//					parentView.HandlePickedImage(imageData);
//					session.StopRunning();
					var iamge=UIImage.LoadFromData(imageData);
					InvokeOnMainThread(delegate {
						var imageView=new UIImageView(new RectangleF(0,0,this.View.Frame.Size.Width,this.View.Frame.Size.Height));
						imageView.Image=iamge;
						this.View.AddSubview(imageView);
					});

				}
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc);
			}
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}

		#endregion
	}

	public class CustomLayerDelegate :CALayerDelegate
	{
		public override void DrawLayer (CALayer layer, CGContext context)
		{
			//layer.Frame = new CGRect(0,0,ImageView.Image.Size.Width,ImageView.Image.Size.Height);
			//var context=UIGraphics.GetCurrentContext();
			float kEdgeInset = 20;
			float kLineLength = 20;
			float kTopOffset = 20;
			float kBottomOffset = 50;

			RectangleF layerFrame = layer.Frame;
			context.BeginPath ();
			context.SetLineWidth (5);
			context.MoveTo (kEdgeInset+kLineLength, kEdgeInset*2);
			context.AddLineToPoint (kEdgeInset,kEdgeInset*2);
			context.AddLineToPoint (kEdgeInset, kEdgeInset*2+kEdgeInset);
			//context.ClosePath ();

			//UIColor.White.SetStroke ();
			context.SetStrokeColorWithColor (UIColor.White.CGColor);
			context.DrawPath (CGPathDrawingMode.Stroke);

			context.BeginPath ();
			context.SetLineWidth (5);
			context.MoveTo (kEdgeInset, layerFrame.Size.Height-kBottomOffset-kEdgeInset);
			context.AddLineToPoint (kEdgeInset,layerFrame.Size.Height-kBottomOffset);
			context.AddLineToPoint (kLineLength+kEdgeInset, layerFrame.Size.Height-kBottomOffset);
			//context.ClosePath ();

			//UIColor.White.SetStroke ();
			//context.SetStrokeColorWithColor (UIColor.White.CGColor);
			context.DrawPath (CGPathDrawingMode.Stroke);
		
			//context.SetStrokeColorWithColor (UIColor.White.CGColor);
			context.DrawPath (CGPathDrawingMode.Stroke);

			context.BeginPath ();
			context.SetLineWidth (5);
			context.MoveTo (layerFrame.Width-kEdgeInset-kLineLength, kEdgeInset*2);
			context.AddLineToPoint (layerFrame.Width-kEdgeInset,kEdgeInset*2);
			context.AddLineToPoint (layerFrame.Width-kEdgeInset, kEdgeInset*2+kTopOffset);
			context.DrawPath (CGPathDrawingMode.Stroke);

			context.BeginPath ();
			context.SetLineWidth (5);
			context.MoveTo (layerFrame.Width-kEdgeInset,layerFrame.Height-kBottomOffset-kEdgeInset);
			context.AddLineToPoint (layerFrame.Width-kEdgeInset,layerFrame.Height-kBottomOffset);
			context.AddLineToPoint (layerFrame.Width-kEdgeInset*2, layerFrame.Height-kBottomOffset);
			context.DrawPath (CGPathDrawingMode.Stroke);

			//layer.BackgroundColor = UIColor.Green.CGColor;
			layer.BorderWidth = 2.0f;
			//layer.BorderColor = UIColor.White.CGColor;
		}
	}
}

