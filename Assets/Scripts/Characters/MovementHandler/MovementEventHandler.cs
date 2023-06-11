
public static class MovementEventHandler
{
    public delegate void InMove(bool inMove);
    public delegate void Step();

    public static event InMove startMove;
    public static event InMove endMove;

    public static event Step startStep;
    public static event Step endStep;



    public static void InvokeStartMove()
    {
        startMove?.Invoke(true);
        startStep?.Invoke();
    }

    public static void InvokeEndMove()
    {
        endMove?.Invoke(false);
        endStep?.Invoke();
    }
}
