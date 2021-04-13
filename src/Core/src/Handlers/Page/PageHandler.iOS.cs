﻿using System;

#if __IOS__ || IOS || MACCATALYST
using NativeView = UIKit.UIView;
#else
using NativeView = AppKit.NSView;
#endif

namespace Microsoft.Maui.Handlers
{
	public partial class PageHandler : AbstractViewHandler<IPage, LayoutView>
	{
		protected override LayoutView CreateNativeView()
		{
			if (VirtualView == null)
			{
				throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a LayoutView");
			}

			var view = new LayoutView
			{
				CrossPlatformMeasure = VirtualView.Measure,
				CrossPlatformArrange = VirtualView.Arrange,
			};

			return view;
		}

		public override void SetVirtualView(IView view)
		{
			base.SetVirtualView(view);

			_ = TypedNativeView ?? throw new InvalidOperationException($"{nameof(TypedNativeView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			TypedNativeView.CrossPlatformMeasure = VirtualView.Measure;
			TypedNativeView.CrossPlatformArrange = VirtualView.Arrange;

			//foreach (var child in VirtualView.Children)
			//{
			//	TypedNativeView.AddSubview(child.ToNative(MauiContext));
			//}
		}
	}
}
