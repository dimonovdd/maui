using System.Linq;

// This is a temporary namespace until we rename everything and move the legacy layouts
namespace Microsoft.Maui.Controls.Layout2
{
	public abstract class StackLayout : Layout, IStackLayout
	{
		public int Spacing { get; set; }

		bool _isMeasureValid;
		bool _isArrangeValid;

		public override bool IsMeasureValid
		{
			get
			{
				// todo ezhart We could use for() over these lists for a tiny performance win
				return _isMeasureValid
					&& Children.All(child => child.IsMeasureValid);
			}

			protected set
			{
				_isMeasureValid = value;
			}
		}

		public override bool IsArrangeValid 
		{
			get
			{
				return _isArrangeValid
					&& Children.All(child => child.IsArrangeValid);
			}

			protected set
			{
				_isArrangeValid = value;
			}
		}
	}
}
