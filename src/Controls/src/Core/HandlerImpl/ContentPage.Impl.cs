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

		public override bool IsMeasureValid 
		{
			get 
			{ 
				return base.IsMeasureValid && View.IsMeasureValid; 
			}

			protected set => base.IsMeasureValid = value; 
		}

		public override bool IsArrangeValid 
		{ 
			get 
			{
				System.Diagnostics.Debug.WriteLine($">>>>>> ContentPage.IsArrangeValid: base.IsArrangeValid = {base.IsArrangeValid}, View.IsArrangeValid = {View.IsArrangeValid}");
				return base.IsArrangeValid && View.IsArrangeValid; 
			} 

			protected set => base.IsArrangeValid = value; 
		}

		protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
		{
			System.Diagnostics.Debug.WriteLine($">>>>>> ContentPage.MeasureOverride: widthConstraint = {widthConstraint}, heightConstraint = {heightConstraint}");

			var width = widthConstraint;
			var height = heightConstraint;

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

			if (View is IFrameworkElement fe)
			{
				fe.Measure(width, height);
			}

			IsMeasureValid = true;
			return new Size(width, height);
		}

		protected override void ArrangeOverride(Rectangle bounds)
		{
			System.Diagnostics.Debug.WriteLine($">>>>>> ContentPage.ArrangeOverride: bounds = {bounds}");

			if (IsArrangeValid)
			{
				System.Diagnostics.Debug.WriteLine($">>>>>> ContentPage.ArrangeOverride: IsArrangeValid is true, cutting out");
				return;
			}

			IsArrangeValid = true;
			IsMeasureValid = true;
			Arrange(bounds);

			if (View is IFrameworkElement fe)
			{
				System.Diagnostics.Debug.WriteLine($">>>>>> ContentPage.ArrangeOverride: arranging content in frame");
				fe.Arrange(Frame);
			}

			if (View is Layout layout)
				layout.ResolveLayoutChanges();

			Handler?.SetFrame(Frame);
		}
	}
}
