namespace Flywheel.Generator
{
	public interface IMessageBox
	{
		void ShowError(string message);
		void ShowSuccess(string message);
	}
}