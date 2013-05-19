using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.ComponentModel;

namespace DevelopmentWithADot.AspNetCallbackControl
{	
	[PersistChildren(false)]
	public class CallbackControl : Control, ICallbackEventHandler
	{
		#region Public constructor
		public CallbackControl()
		{
			this.OnClientCallbackSuccess = "onCallbackSuccess";
		}
		#endregion

		#region Public properties and events
		public event EventHandler<CallbackEventArgs> Callback;

		[DefaultValue(false)]
		public Boolean SendAllData
		{
			get;
			set;
		}

		[DefaultValue("onCallbackSuccess")]
		public String OnClientCallbackSuccess
		{
			get;
			set;
		}
		#endregion

		#region Protected override methods
		protected override void Render(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);

			base.Render(writer);
		}

		protected override void OnInit(EventArgs e)
		{
			String reference = this.Page.ClientScript.GetCallbackEventReference(this, "arg", this.OnClientCallbackSuccess, "context");
			String script = String.Concat("\ndocument.getElementById('", this.ClientID, "').callback = function(arg, context){", ((this.SendAllData == true) ? "__theFormPostCollection.length = 0; __theFormPostData = '';  WebForm_InitCallback(); " : String.Empty), reference, ";};\n");

			this.Page.ClientScript.RegisterStartupScript(this.GetType(), String.Concat("callback", this.ClientID), script, true);
			this.Page.RegisterRequiresPostBack(this);

			base.OnInit(e);
		}
		#endregion

		#region Protected virtual methods
		protected virtual void OnCallback(CallbackEventArgs args)
		{
			EventHandler<CallbackEventArgs> handler = this.Callback;

			if (handler != null)
			{
				handler(this, args);
			}
		}
		#endregion

		#region ICallbackEventHandler Members

		String ICallbackEventHandler.GetCallbackResult()
		{
			CallbackEventArgs args = new CallbackEventArgs(this.Context.Items["Data"].ToString());

			this.OnCallback(args);
	
			return (args.Result);
		}

		void ICallbackEventHandler.RaiseCallbackEvent(String eventArgument)
		{
			this.Context.Items["Data"] = eventArgument;
		}

		#endregion
	}
}
