using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;

namespace Microsoft.Maui.Controls
{
	public partial class VisualElement : IFrameworkElement
	{
		private IViewHandler _handler;

		public Rectangle Frame => Bounds;

		public IViewHandler Handler
		{
			get => _handler;
			set
			{
				_handler = value;
				IsPlatformEnabled = _handler != null;
			}
		}

		protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			(Handler)?.UpdateValue(propertyName);
		}

		IFrameworkElement IFrameworkElement.Parent => Parent as IView;

		public Size DesiredSize { get; protected set; }

		public virtual bool IsMeasureValid { get; protected set; }

		public virtual bool IsArrangeValid { get; protected set; }

		public void Arrange(Rectangle bounds)
		{
			Layout(bounds);
		}

		void IFrameworkElement.Arrange(Rectangle bounds)
		{
			ArrangeOverride(bounds);
		}

		// ArrangeOverride provides a way to allow subclasses (e.g., Layout) to override Arrange even though
		// the interface has to be explicitly implemented to avoid conflict with the old Arrange method
		protected virtual void ArrangeOverride(Rectangle bounds)
		{
			System.Diagnostics.Debug.WriteLine($">>>>>> {this} ArrangeOverride, bounds = {bounds}");

			if (IsArrangeValid)
			{
				System.Diagnostics.Debug.WriteLine($">>>>>> Arrange valid, skipping");
				return;
			}

			System.Diagnostics.Debug.WriteLine($">>>>>> Arranging ...");

			IsArrangeValid = true;

			var newRect = this.ComputeFrame(bounds);

			Bounds = newRect;
			Handler?.SetFrame(Bounds);
		}

		// TODO: MAUI
		// This is here to support layout calls from legacy code
		// This should go away once we get everything piping through
		// Maui based layout code
		public void Layout(Rectangle bounds)
		{
			if (Bounds != bounds)
			{
				Bounds = bounds;
				Handler?.SetFrame(Bounds);
			}
		}

		// TODO MAUI. Current MAUI layouts don't
		// invalidate if the children change
		//void InvalidateParentHack()
		//{
		//	if (!(this is Page))
		//		this.FindParentOfType<Page>()?.InvalidateMeasure();
		//}

		void IFrameworkElement.InvalidateMeasure()
		{
			InvalidateMeasureOverride();
		}

		// TODO ezhart if you keep this, fix the comments
		// ArrangeOverride provides a way to allow subclasses (e.g., Layout) to override InvalidateMeasure even though
		// the interface has to be explicitly implemented to avoid conflict with the VisualElement.InvalidateMeasure method
		protected virtual void InvalidateMeasureOverride()
		{
			if (!IsMeasureValid && !IsArrangeValid)
				return;

			IsMeasureValid = false;
			IsArrangeValid = false;
			InvalidateMeasure();
			//InvalidateParentHack();
		}

		void IFrameworkElement.InvalidateArrange()
		{
			IsArrangeValid = false;
		}

		Size IFrameworkElement.Measure(double widthConstraint, double heightConstraint)
		{
			return MeasureOverride(widthConstraint, heightConstraint);
		}

		// TODO ezhart if you keep this, fix the comments
		// ArrangeOverride provides a way to allow subclasses (e.g., Layout) to override Measure even though
		// the interface has to be explicitly implemented to avoid conflict with the old Measure method
		protected virtual Size MeasureOverride(double widthConstraint, double heightConstraint)
		{
			System.Diagnostics.Debug.WriteLine($">>>>>> {this} MeasureOverride, (widthConstraint, heighConstraint) = ({widthConstraint}, {heightConstraint})");

			if (!IsMeasureValid)
			{
				System.Diagnostics.Debug.WriteLine($">>>>>> Measure not valid, re-measuring");

				DesiredSize = this.ComputeDesiredSize(widthConstraint, heightConstraint);
			}
			else
			{
				System.Diagnostics.Debug.WriteLine($">>>>>> Measure was valid, skipping measure");
			}

			IsMeasureValid = true;
			return DesiredSize;
		}

		Maui.FlowDirection IFrameworkElement.FlowDirection => FlowDirection.ToPlatformFlowDirection();
		Primitives.LayoutAlignment IFrameworkElement.HorizontalLayoutAlignment => default;
		Primitives.LayoutAlignment IFrameworkElement.VerticalLayoutAlignment => default;

		Maui.Semantics _semantics;
		Maui.Semantics IFrameworkElement.Semantics
		{
			get => _semantics;
		}

		// We don't want to initialize Semantics until someone explicitly 
		// wants to modify some aspect of the semantics class
		internal Semantics SetupSemantics() =>
			_semantics ??= new Maui.Semantics();

		double IFrameworkElement.Width { get => WidthRequest; }
		double IFrameworkElement.Height { get => HeightRequest; }
	}
}
