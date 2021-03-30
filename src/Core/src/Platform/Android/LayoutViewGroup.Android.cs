using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Rectangle = Microsoft.Maui.Graphics.Rectangle;
using Size = Microsoft.Maui.Graphics.Size;

namespace Microsoft.Maui.Handlers
{
	// TODO ezhart This doesn't need the .Android prefix anymore for the file name
	public class LayoutViewGroup : ViewGroup
	{
		public LayoutViewGroup(Context context) : base(context)
		{
			SetLayerType(LayerType.Software, null);
		}

		public LayoutViewGroup(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
			SetLayerType(LayerType.Software, null);
		}

		public LayoutViewGroup(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			SetLayerType(LayerType.Software, null);
		}

		public LayoutViewGroup(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			SetLayerType(LayerType.Software, null);
		}

		public LayoutViewGroup(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			SetLayerType(LayerType.Software, null);
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			if (Context == null)
			{
				return;
			}
			
			if (CrossPlatformMeasure == null)
			{
				base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
				return;
			}

			var deviceIndependentWidth = widthMeasureSpec.ToDouble(Context);
			var deviceIndependentHeight = heightMeasureSpec.ToDouble(Context);

			System.Diagnostics.Debug.WriteLine($">>>>>> LayoutViewGroup OnMeasure {deviceIndependentWidth}, {deviceIndependentHeight}");

			var size = CrossPlatformMeasure(deviceIndependentWidth, deviceIndependentHeight);

			System.Diagnostics.Debug.WriteLine($">>>>>> LayoutViewGroup CrossPlatformMeasure result was {size}");

			var nativeWidth = Context.ToPixels(size.Width);
			var nativeHeight = Context.ToPixels(size.Height);

			SetMeasuredDimension((int)nativeWidth, (int)nativeHeight);
		}

		public override void RequestLayout()
		{
			System.Diagnostics.Debug.WriteLine($">>>>>> LayoutViewGroup.RequestLayout");

			base.RequestLayout();	
		}

		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			if (CrossPlatformArrange == null || Context == null)
			{
				return;
			}

			var deviceIndependentLeft = Context.FromPixels(l);
			var deviceIndependentTop = Context.FromPixels(t);
			var deviceIndependentRight = Context.FromPixels(r);
			var deviceIndependentBottom = Context.FromPixels(b);

			var destination = Rectangle.FromLTRB(deviceIndependentLeft, deviceIndependentTop,
				deviceIndependentRight, deviceIndependentBottom);

			System.Diagnostics.Debug.WriteLine($">>>>>> LayoutViewGroup CrossPlatformArrange to {destination}");

			CrossPlatformArrange(destination);
		}

		internal Func<double, double, Size>? CrossPlatformMeasure { get; set; }
		internal Action<Rectangle>? CrossPlatformArrange { get; set; }
	}
}
