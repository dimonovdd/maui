using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls
{
	public partial class ContentPage : IPage
	{
		// TODO ezhart That there's a layout alignment here tells us this hierarchy needs work :) 
		public Primitives.LayoutAlignment HorizontalLayoutAlignment => Primitives.LayoutAlignment.Fill;

		// TODO ezhart super sus
		public Thickness Margin => Thickness.Zero;

		IView IPage.Content => Content;

		internal override void InvalidateMeasureInternal(InvalidationTrigger trigger)
		{
			IsArrangeValid = false;
			base.InvalidateMeasureInternal(trigger);
		}

		public override bool IsMeasureValid 
		{
			get 
			{ 
				return base.IsMeasureValid && Content.IsMeasureValid; 
			}

			protected set => base.IsMeasureValid = value; 
		}

		public override bool IsArrangeValid 
		{ 
			get 
			{
				return base.IsArrangeValid && Content.IsArrangeValid; 
			} 

			protected set => base.IsArrangeValid = value; 
		}

		protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
		{
			if (Content is IFrameworkElement fe)
			{
				fe.Measure(widthConstraint, heightConstraint);
			}

			IsMeasureValid = true;
			return new Size(widthConstraint, heightConstraint);
		}

		protected override void ArrangeOverride(Rectangle bounds)
		{
			if (IsArrangeValid)
			{
				return;
			}

			IsArrangeValid = true;
			IsMeasureValid = true;

			if (Content is IFrameworkElement element)
			{
				element.Arrange(bounds);
			}

			Arrange(bounds);

			//if (Content is Layout layout)
			//{
			//	// Force layout resolution if this is a Forms layout
			//	// TODO ezhart Not sure this should be happening here; the renderer itself should be doing this. Investigate.
			//	layout.ResolveLayoutChanges();
			//}

			//Handler?.SetFrame(Frame);
		}

		protected override void InvalidateMeasureOverride()
		{
			base.InvalidateMeasureOverride();
			if (Content is IFrameworkElement fe)
			{
				fe.InvalidateMeasure();
			}
		}
	}
}
