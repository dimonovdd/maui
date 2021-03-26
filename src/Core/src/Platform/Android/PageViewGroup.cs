﻿using System;
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

			CrossPlatformArrange(destination);
		}

		internal Action<Rectangle>? CrossPlatformArrange { get; set; }
	}
}
