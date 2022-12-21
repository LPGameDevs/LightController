using System;
using System.Collections;
using UnityEngine;

namespace LightController.Test
{
        public class Wait
        {
            static public IEnumerator Until(Func<bool> condition, float timeout = 30f)
            {
                float timePassed = 0f;
                while (!condition() && timePassed < timeout)
                {
                    long before = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    yield return null;
                    long after = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                    float delta = (after - before) / 1000f;

                    timePassed += delta;
                    // Debug.Log(delta);
                    // Debug.Log(timePassed);
                }
                if (timePassed >= timeout) {
                    throw new TimeoutException("Condition was not fulfilled for " + timeout + " seconds.");
                }
            }
        }
}
