using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TheLiquidFire.Notifications;

public class Clickable : MonoBehaviour, IPointerClickHandler {
	public const string ClickedNotification = "Clickable.ClickedNotification";

	public void OnPointerClick(PointerEventData eventData) {
		this.PostNotification (ClickedNotification, eventData);
	}
}