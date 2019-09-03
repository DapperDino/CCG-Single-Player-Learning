using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class GlobalTests {

	private class Foo
	{

	}

	private class Bar
	{

	}

	[Test]
	public void testGenericAndTypeIDsAreEqual() {
		var id1 = Global.GenerateID<Foo> ();
		var type = new Foo ().GetType ();
		var id2 = Global.GenerateID (type);
		Assert.AreEqual (id1, id2);
	}

	[Test]
	public void testUniqueTypeIDs() {
		var id1 = Global.GenerateID<Foo> ();
		var id2 = Global.GenerateID<Bar> ();
		Debug.Log (id1.ToString());
		Debug.Log (id2.ToString());
		Assert.AreNotEqual (id1, id2);
	}

	[Test]
	public void TestGenericAndTypePrepareNotificationsAreEqual() {
		var prepare1 = Global.PrepareNotification<Foo> ();
		var foo = new Foo ();
		var prepare2 = Global.PrepareNotification (foo.GetType ());
		Assert.AreEqual (prepare1, prepare2);
	}

	[Test]
	public void TestGenericAndTypePerformNotificationsAreEqual() {
		var perform1 = Global.PerformNotification<Foo> ();
		var foo = new Foo ();
		var perform2 = Global.PerformNotification (foo.GetType ());
		Assert.AreEqual (perform1, perform2);
	}

	[Test]
	public void TestPrepareAndPerformNotificationsAreNotEqual() {
		var prepare = Global.PrepareNotification<Foo> ();
		var perform = Global.PerformNotification<Foo> ();
		Assert.AreNotEqual (prepare, perform);
	}

	[Test]
	public void TestPrepareNotificationsAreUniqueForDifferentTypes() {
		var foo = Global.PrepareNotification<Foo> ();
		var bar = Global.PrepareNotification<Bar> ();
		Assert.AreNotEqual (foo, bar);
	}

	[Test]
	public void TestPerformNotificationsAreUniqueForDifferentTypes() {
		var foo = Global.PerformNotification<Foo> ();
		var bar = Global.PerformNotification<Bar> ();
		Assert.AreNotEqual (foo, bar);
	}
}
