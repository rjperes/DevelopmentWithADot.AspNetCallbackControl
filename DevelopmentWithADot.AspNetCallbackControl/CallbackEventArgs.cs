using System;

namespace DevelopmentWithADot.AspNetCallbackControl
{
	[Serializable]
	public class CallbackEventArgs : EventArgs
	{
		public CallbackEventArgs(String data)
		{
			this.Data = data;
			this.Result = String.Empty;
		}

		public String Data
		{
			get;
			private set;
		}

		public String Result
		{
			get;
			set;
		}
	}
}