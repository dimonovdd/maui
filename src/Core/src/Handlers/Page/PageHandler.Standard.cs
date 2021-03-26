using System;

namespace Microsoft.Maui.Handlers
{
	public partial class PageHandler : AbstractViewHandler<IPage, object>
	{
		protected override object CreateNativeView() => throw new NotImplementedException();
	}
}
