namespace Microsoft.Maui
{
	/// <summary>
	/// A View that occupies the entire screen.
	/// </summary>
	public interface IPage : IFrameworkElement
	{
		/// <summary>
		/// Gets the view that contains the content of the Page.
		/// </summary>
		public IView View { get; }
	}
}