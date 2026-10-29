public abstract class Node
{
    public enum Node_States
    {
        Failure,
        Running,
        Success
    }

    protected Node_States state;
    public Node_States get_state => state;
    public bool has_entered;
    public abstract Node_States Tree_Tick();
    public virtual void On_Enter() => has_entered = true;

    public virtual void On_Exit() => has_entered = false;

    public virtual void Reset()
    {
        state = Node_States.Failure;
    }

}