using System.Collections.Generic;


public abstract class Composite : Node
{
    protected readonly List<Node> children;
    public int nodes_index = 0;


    public Composite(List<Node> _children)
    {
        this.children = _children ?? new List<Node>();
    }

    public override void Reset()
    {
        base.Reset();

        nodes_index = 0;
        foreach (var child in children) child.Reset();
    }
    protected void Exit_All_Children()
    {
        foreach (var child in children)
        {
            if (child.get_state == Node_States.Running) child.On_Exit();
            child.Reset();
        }
    }


}