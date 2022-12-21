public struct FadeOutEvent
{
    public FadeStatus State;

    public FadeOutEvent(FadeStatus status)
    {
        State = status;
    }

    static FadeOutEvent e;
    public static void Trigger(FadeStatus status)
    {
        e.State = status;
        EventManager.TriggerEvent(e);
    }

}
