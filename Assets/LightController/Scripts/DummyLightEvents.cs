using System;
using System.Threading.Tasks;
using UnityEngine;

public class DummyLightEvents : EventListener<FadeInEvent>, EventListener<FadeOutEvent>
{
    public DummyLightEvents()
    {
        EventRegister.EventStartListening<FadeInEvent>(this);
        EventRegister.EventStartListening<FadeOutEvent>(this);
    }

    public void Remove()
    {
        EventRegister.EventStopListening<FadeInEvent>(this);
        EventRegister.EventStopListening<FadeOutEvent>(this);
    }

    public void FadeIn()
    {
        FadeInEvent.Trigger(FadeStatus.End);
    }

    public void FadeOut()
    {
        var task = new Task(FadeOutLoop);
        task.Start();
    }

    private void FadeOutLoop()
    {
        System.Threading.Thread.Sleep(100);
        FadeOutEvent.Trigger(FadeStatus.End);;
    }


    public void OnEvent(FadeInEvent eventType)
    {
        if (eventType.State != FadeStatus.Start)
        {
            return;
        }
        FadeIn();

    }

    public void OnEvent(FadeOutEvent eventType)
    {
        if (eventType.State != FadeStatus.Start)
        {
            return;
        }
        FadeOut();
    }
}
