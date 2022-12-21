using System.Collections;
using System.Drawing;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace LightController.Test
{
    public class LightControllerTest
    {

        /*
         * 1. The light controller should be able to turn on the light
         * 2. The light controller should be able to turn off the light
         * 3. The light controller should not be able to turn on the light if it is off
         * 4. The light controller should not be able to turn off the light if it is on
         * 5. The light controller should be able to dim the light.
         * 6. The light controller should be change the color of the light.
         * 7. The light controller should be able to fade in the light.
         * 8. The light controller should be able to fade out the light.
         */

        // private GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/SurvivalAnnaNew.prefab");


        [Test]
        public void TestLightExists()
        {
            var lightController = new LightController();
            Assert.That(lightController.GetNumberOfLights(), Is.EqualTo(0));

            Assert.Throws<LightNotFoundException>(
                () => { lightController.IsLightOn(0); });

            lightController.SetLights(25);
            Assert.That(lightController.GetNumberOfLights(), Is.EqualTo(25));

            Assert.DoesNotThrow(
                () => { lightController.IsLightOn(0); });
        }


        [Test]
        public void TestLightAbsolute()
        {
            var lightController = new LightController();
            lightController.SetLights(2);

            bool isLightOn = lightController.IsLightOn(0);
            Assert.AreEqual(false, isLightOn);
            isLightOn = lightController.IsLightOn(1);
            Assert.AreEqual(false, isLightOn);

            lightController.TurnOnLight(0);

            isLightOn = lightController.IsLightOn(0);
            Assert.AreEqual(true, isLightOn);
            isLightOn = lightController.IsLightOn(1);
            Assert.AreEqual(false, isLightOn);

            lightController.TurnOffLight(0);
            lightController.TurnOnLight(1);
            isLightOn = lightController.IsLightOn(0);
            Assert.AreEqual(false, isLightOn);
            isLightOn = lightController.IsLightOn(1);
            Assert.AreEqual(true, isLightOn);
        }

        [Test]
        public void TestLightRelative()
        {
            var lightController = new LightController();
            lightController.SetLights(1);

            bool isLightOn = lightController.IsLightOn(0);
            Assert.AreEqual(false, isLightOn);

            lightController.SetLightPercentage(0, 50);
            isLightOn = lightController.IsLightOn(0);
            Assert.AreEqual(true, isLightOn);

            lightController.SetLightPercentage(0, 0);
            isLightOn = lightController.IsLightOn(0);
            Assert.AreEqual(false, isLightOn);
        }

        [Test]
        public void TestLightColor()
        {
            var lightController = new LightController();


            Assert.Throws<LightNotFoundException>(
                () => { lightController.GetLightColor(0); });


            lightController.SetLights(1);

            Color color = lightController.GetLightColor(0);
            Assert.AreEqual(Color.White, color);

            bool isLightOn = lightController.IsLightOn(0);
            Assert.AreEqual(false, isLightOn);

            lightController.SetLightColor(0, Color.Red);

            color = lightController.GetLightColor(0);
            Assert.AreEqual(Color.Red, color);

            lightController.SetLightPercentage(0, 50);

            color = lightController.GetLightColor(0);
            Assert.AreEqual(Color.Red, color);

            lightController.SetLightColor(0, Color.Blue);
            color = lightController.GetLightColor(0);
            Assert.AreEqual(Color.Blue, color);

            lightController.SetLightPercentage(0, 0);
            color = lightController.GetLightColor(0);
            Assert.AreEqual(Color.Blue, color);
        }

        [UnityTest]
        public IEnumerator TestLightFade()
        {
            DummyLightEvents old = new DummyLightEvents();

            var lightController = new LightController();
            lightController.SetLights(1);

            bool isLightOn = lightController.IsLightOn(0);
            Assert.AreEqual(false, isLightOn);

            lightController.FadeIn(0);

            isLightOn = lightController.IsLightOn(0);
            Assert.AreEqual(true, isLightOn);

            yield return Wait.Until(() =>
            {
                return lightController.IsLightOn(0);
            }, timeout: 1f);

            lightController.FadeOut(0);

            isLightOn = lightController.IsLightOn(0);
            Assert.AreEqual(true, isLightOn);

            yield return Wait.Until(() =>
            {
                return !lightController.IsLightOn(0);
            }, timeout: 1f);

            old.Remove();
        }
    }
}
