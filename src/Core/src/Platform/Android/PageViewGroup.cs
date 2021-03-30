using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;

namespace Microsoft.Maui.Handlers
{
	public class PageViewGroup : ViewGroup
	{
		public PageViewGroup(Context context) : base(context)
		{
		}

		public PageViewGroup(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		public PageViewGroup(Context context, IAttributeSet attrs) : base(context, attrs)
		{
		}

		public PageViewGroup(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
		}

		public PageViewGroup(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
		}

		public override void RequestLayout()
		{
			System.Diagnostics.Debug.WriteLine($">>>>>> PageViewGroup.RequestLayout");

			base.RequestLayout();
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

			System.Diagnostics.Debug.WriteLine($">>>>>> PageViewGroup OnMeasure {deviceIndependentWidth}, {deviceIndependentHeight}");

			var size = CrossPlatformMeasure(deviceIndependentWidth, deviceIndependentHeight);

			System.Diagnostics.Debug.WriteLine($">>>>>> PageViewGroup CrossPlatformMeasure result was {size}");

			var nativeWidth = Context.ToPixels(size.Width);
			var nativeHeight = Context.ToPixels(size.Height);

			SetMeasuredDimension((int)nativeWidth, (int)nativeHeight);
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

			System.Diagnostics.Debug.WriteLine($">>>>>> PageViewGroup CrossPlatformArrange to {destination}");

			CrossPlatformArrange(destination);
		}

		internal Func<double, double, Size>? CrossPlatformMeasure { get; set; }
		internal Action<Rectangle>? CrossPlatformArrange { get; set; }
	}
}
