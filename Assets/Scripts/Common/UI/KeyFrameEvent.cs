using UnityEngine;
using System.Collections;
using TheLiquidFire.Notifications;

namespace TheLiquidFire.UI
{
	public class KeyFrameEvent : MonoBehaviour 
	{
		public const string EnterFrameValueNotification = "KeyFrameEvent.EnterFrameValue";
		public const string EnterFrameMessageNotification = "KeyFrameEvent.EnterFrameMessage";

		public void EnterFrameValue (int value)
		{
			this.PostNotification(EnterFrameValueNotification, value);
		}

		public void EnterFrameMessage (string message)
		{
			this.PostNotification(EnterFrameMessageNotification, message);
		}
	}
}