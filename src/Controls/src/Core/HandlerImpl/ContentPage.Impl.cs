using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls
{
	public partial class ContentPage : Microsoft.Maui.IPage
	{
		// TODO ezhart That there's a layout alignment here tells us this hierarchy needs work :) 
		public Primitives.LayoutAlignment HorizontalLayoutAlignment => Primitives.LayoutAlignment.Fill;

		public IView View => Content;

		

		internal override void InvalidateMeasureInternal(InvalidationTrigger trigger)
		{
			IsArrangeValid = false;
			base.InvalidateMeasureInternal(trigger);
		}

		protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
		{
			var width = widthConstraint;
			var height = heightConstraint;

#if WINDOWS
			if (double.IsInfinity(width))
			{
				width = 800;
			}

			if (double.IsInfinity(height))
			{
				height = 800;
			}
#endif

			IsMeasureValid = true;
			return new Size(width, height);
		}

		protected override void ArrangeOverride(Rectangle bounds)
		{
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
