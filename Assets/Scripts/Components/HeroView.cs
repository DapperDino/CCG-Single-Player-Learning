using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TheLiquidFire.Notifications;

public class HeroView : MonoBehaviour {
	public Image avatar;
	public Text attack;
	public Text health;
	public Text armor;
	public Sprite active;
	public Sprite inactive;
	public Hero hero { get; private set; }

	public void SetHero (Hero hero) {
		this.hero = hero;
		Refresh ();
	}

	void OnEnable () {
		this.AddObserver (OnPerformDamageAction, Global.PerformNotification<DamageAction> ());
	}

	void OnDisable () {
		this.RemoveObserver (OnPerformDamageAction, Global.PerformNotification<DamageAction> ());
	}

	void Refresh () {
		if (hero == null)
			return;
		avatar.sprite = inactive; // TODO: Add activation logic
		attack.text = hero.attack.ToString ();
		health.text = hero.hitPoints.ToString ();
		armor.text = hero.armor.ToString ();
	}

	void OnPerformDamageAction (object sender, object args) {
		var action = args as DamageAction;
		if (action.targets.Contains (hero)) {
			Refresh ();
		}
	}
}