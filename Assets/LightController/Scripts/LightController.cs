using System;
using System.Collections.Generic;
using System.Drawing;

namespace LightController
{
    public class LightController
    {
        public static event Action OnResetBoard;
        public static event Action<Light> OnLightAdded;
        public static event Action<Light> OnLightRemoved;

        private List<Light> _lights = new List<Light>();
        private int _lightCount;
        private bool _lightOn;
        private bool _lightExists;

        ~LightController()
        {
            ClearLights();
        }

        public void SetLights(int number)
        {
            ClearLights();
            for (int i = 0; i < number; i++)
            {
                AddLight(i);
            }
            OnResetBoard?.Invoke();
        }

        private void AddLight(int position)
        {
            Light light = new Light(position);
            _lights.Add(light);
            OnLightAdded?.Invoke(light);
        }

        private void ClearLights()
        {
            foreach (Light light in _lights)
            {
                OnLightRemoved?.Invoke(light);
                light.Remove();
            }
            _lights.Clear();
        }

        public int GetNumberOfLights()
        {
            return _lights.Count;
        }

        public bool IsLightOn(int i)
        {
            if (i >= _lights.Count)
            {
                throw new LightNotFoundException();
            }
            return _lights[i].IsOn;
        }

        public void TurnOnLight(int i)
        {
            _lights[i].IsOn = true;
        }

        public void TurnOffLight(int i)
        {
            _lights[i].IsOn = false;
        }

        public void SetLightPercentage(int i, int percent)
        {
            if (percent == 0)
            {
                _lights[i].IsOn = false;
            }
            else
            {
                _lights[i].IsOn = true;
            }
        }

        public void SetLightColor(int i, Color color)
        {
            _lights[i].Color = color;
        }

        public Color GetLightColor(int i)
        {
            if (i >= _lights.Count)
            {
                throw new LightNotFoundException();
            }

            return _lights[i].Color;
        }

        public void FadeIn(int i)
        {
            _lights[i].FadeIn();
        }

        public void FadeOut(int i)
        {
            _lights[i].FadeOut();
        }
    }

    public class Light : EventListener<FadeInEvent>, EventListener<FadeOutEvent>
    {
        private int _position;
        private bool _isFading = false;
        public Color Color { get; set; }
        public bool IsOn { get; set; }

        public Light(int position)
        {
            _position = position;
            EventRegister.EventStartListening<FadeInEvent>(this);
            EventRegister.EventStartListening<FadeOutEvent>(this);
            Color = Color.White;
            IsOn = false;
        }

        public int GetPosition()
        {
            return _position;
        }

        public void Remove()
        {
            EventRegister.EventStopListening<FadeInEvent>(this);
            EventRegister.EventStopListening<FadeOutEvent>(this);
        }



        public void FadeIn()
        {

            FadeInEvent.Trigger(FadeStatus.Start);

            _isFading = true;
            IsOn = true;
        }

        public void FadeOut()
        {
            FadeOutEvent.Trigger(FadeStatus.Start);

            _isFading = true;
        }

        public void OnEvent(FadeInEvent eventType)
        {
            if (eventType.State != FadeStatus.End)
            {
                return;
            }
            _isFading = false;
        }

        public void OnEvent(FadeOutEvent eventType)
        {
            if (eventType.State != FadeStatus.End)
            {
                return;
            }

            _isFading = false;
            IsOn = false;
        }
    }

    public class LightNotFoundException : Exception
    {
    }
}
