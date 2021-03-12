using System;

namespace Microsoft.Maui.DeviceTests.Stubs
{
	class AppStub : App, IDisposable
	{
		public override IWindow CreateWindow(IActivationState state)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			Current = null;
		}
	}
}