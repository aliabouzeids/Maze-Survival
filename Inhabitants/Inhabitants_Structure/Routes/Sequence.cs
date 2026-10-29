using System.Collections.Generic;

public class Sequence : Composite
{

    public Sequence(List<Node> _children) : base(_children) { }
    public override Node_States Tree_Tick()
    {
        if (children.Count == 0) return Node_States.Success;

        for (; nodes_index < children.Count;)
        {
            var child = children[nodes_index];

            if (!child.has_entered) child.On_Enter();

            var result = child.Tree_Tick();

            if (result == Node_States.Running)
            {
                state = Node_States.Running;
                return state;
            }

            if (result == Node_States.Failure)
            {
                child.On_Exit();
                Exit_All_Children();
                nodes_index = 0;
                state = Node_States.Failure;
                return state;
            }

            child.On_Exit();
            nodes_index++;
        }

        Exit_All_Children();
        nodes_index = 0;
        state = Node_States.Success;
        return state;

    }
}