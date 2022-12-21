public struct FadeInEvent
{
    public FadeStatus State;

    public FadeInEvent(FadeStatus status)
    {
        State = status;
    }

    static FadeInEvent e;
    public static void Trigger(FadeStatus status)
    {
        e.State = status;
        EventManager.TriggerEvent(e);
    }
}
