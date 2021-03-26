using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls
{
	public partial class ContentPage : IPage
	{
		// TODO ezhart That there's a layout alignment here tells us this hierarchy needs work :) 
		public Primitives.LayoutAlignment HorizontalLayoutAlignment => Primitives.LayoutAlignment.Fill;

		public IView View => Content;

		// TODO ezhart super sus
		public Thickness Margin => Thickness.Zero;

		internal override void InvalidateMeasureInternal(InvalidationTrigger trigger)
		{
			IsArrangeValid = false;
			base.InvalidateMeasureInternal(trigger);
		}

//		protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
//		{
//			System.Diagnostics.Debug.WriteLine($">>>>>> ContentPage.MeasureOverride: widthConstraint = {widthConstraint}, heightConstraint = {heightConstraint}");

//			var width = widthConstraint;
//			var height = heightConstraint;

//#if WINDOWS
//			if (double.IsInfinity(width))
//			{
//				width = 800;
//			}

//			if (double.IsInfinity(height))
//			{
//				height = 800;
//			}
//#endif

//			IsMeasureValid = true;
//			return new Size(width, height);
//		}

		protected override void ArrangeOverride(Rectangle bounds)
		{
			System.Diagnostics.Debug.WriteLine($">>>>>> ContentPage.ArrangeOverride: bounds = {bounds}");

			if (IsArrangeValid)
			{
				return;
			}

			IsArrangeValid = true;
			IsMeasureValid = true;
			Arrange(bounds);
			Handler?.SetFrame(Frame);

			if (Content is IFrameworkElement fe)
			{
				fe.InvalidateArrange();
				fe.Measure(Frame.Width, Frame.Height);
				fe.Arrange(Frame);
			}

			if (Content is Layout layout)
				layout.ResolveLayoutChanges();
		}
	}
}
