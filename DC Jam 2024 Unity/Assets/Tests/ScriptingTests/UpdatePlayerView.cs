// using System.Collections;

// using CaptainCoder.Dungeoneering.DungeonCrawler.Scripting;
// using CaptainCoder.Dungeoneering.Player.Unity;
// using CaptainCoder.Dungeoneering.Unity;

// using NUnit.Framework;

// using UnityEngine;
// using UnityEngine.TestTools;

// public class PlayerView_should
// {
//     // A Test behaves as an ordinary method
//     [Test]
//     public void Update_when_view_changed_by_script()
//     {
//         // Use the Assert class to test conditions
//         GameObject gameObject = new();
//         CrawlingModeController context = gameObject.AddComponent<CrawlingModeController>();
//         PlayerViewController viewController = gameObject.AddComponent<PlayerViewController>();
//         context.PlayerViewController = viewController;
//         JavaScriptEventAction teleportAction = new(@"
//         context.SetPlayerView(4, 7, 1)
//         ");
//         teleportAction.Invoke(context);
//         Assert.AreEqual(gameObject.transform.position.x, 7);
//         Assert.AreEqual(gameObject.transform.position.z, 4);
//         Assert.AreEqual(gameObject.transform.rotation.eulerAngles.y, 0);
//     }

//     // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
//     // `yield return null;` to skip a frame.
//     [UnityTest]
//     public IEnumerator UpdatePlayerViewWithEnumeratorPasses()
//     {
//         // Use the Assert class to test conditions.
//         // Use yield to skip a frame.
//         yield return null;
//     }
// }
