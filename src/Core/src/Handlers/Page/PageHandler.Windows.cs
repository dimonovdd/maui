using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Maui.Handlers
{
	public partial class PageHandler : ViewHandler<IPage, LayoutView>
	{
		public override void SetVirtualView(IView view)
		{
			base.SetVirtualView(view);

			_ = NativeView ?? throw new InvalidOperationException($"{nameof(NativeView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			NativeView.CrossPlatformMeasure = VirtualView.Measure;
			NativeView.CrossPlatformArrange = VirtualView.Arrange;

			foreach (var child in VirtualView.Children)
			{
				Add(child);
			}
		}

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
				CrossPlatformArrangeChildren = () =>
				{
					foreach (var element in VirtualView.Children)
					{
						element.Handler?.SetFrame(element.Frame);
					}
				}
			};

			return view;
		}
	}
}
