using System;

namespace BioInspiredOptimization.Individual
{
    class Group : IComparable<Group>
    {
        public int GroupSize;
        public Agent Head;
        public Agent Tail;

        public Group(Agent head, int groupSize)
        {
            GroupSize = groupSize;
            Head = head;
            Tail = new Agent(head.Coords, head.Error);
        }

        public int CompareTo(Group y)
        {
                return Head.CompareTo(y.Head);
        }
    }
}
