using System.Windows.Forms;

namespace Flywheel.Generator
{
	public class MessageBox : IMessageBox
	{
		#region IMessageBox Members

		public void ShowError(string message)
		{
			System.Windows.Forms.MessageBox.Show(message, "Error processing templates",
			                                     MessageBoxButtons.OK, MessageBoxIcon.Stop);
		}

		public void ShowSuccess(string message)
		{
			System.Windows.Forms.MessageBox.Show(message, "Successful Generation", MessageBoxButtons.OK,
			                                     MessageBoxIcon.None);
		}

		#endregion
	}
}