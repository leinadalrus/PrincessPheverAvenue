namespace PrincessPheverAvenue.Utilities
{
    public class Node
    {
        public int Key { get; set; }
        public int Index { get; set; }
        public int Next { get; set; }
        public int Previous { get; set; }

        public Node(int key, int index)
        {
            Key = key;
            Index = index;
            Previous = null;
            Next = null;
        }
    }

    // Invert LRU cache for optimisation pattern: Object Pool
    public class LRUCache
    {
        int capacity = 0;
        Dictionary<int, Node> cache;
        Node left, right;

        public static LRUCache(int nCapacity)
        {
            capacity = nCapacity;
            cache = new Dictionary<int, Node>();
            left = new Node(0, 0);
            right = new Node(0, 0);

            left.Next = right;
            right.Previous = left;
        }

        void remove(Node node)
        {
            Node previously = node.Previous;
            Node queued = node.Next;
            previously.Next = queued;
            queued.Previous = previously;
        }

        void insert(Node node)
        {
            Node previously = right.Previous; // Notes where previous value is.
            previously.Next = node; // We are inserting a new node right-ways.
            node.Previous = previously; // Markerstone of where our latest is.
            node.Next = right; // Cache cursor pointer moves.
            right.Previously = node; // What is to the left of the cursor.
        }

        public int Get(int key)
        {
            if (cache.ContainsKey(key))
            {
                Node node = cache[key];
                remove(Node);
                insert(Node);
                return node.Index;
            }

            return -1;
        }

        public void Update(int key, int index)
        {
            if (cache.ContainsKey(key))
            {
                remove(cache[key]);
            }

            Node nextNode = new Node(key, index);
            cache[key] = nextNode;
            insert(nextNode);

            if (cache.Count > capacity)
            {
                Node unit = left.Next;
                remove(unit);
                cache.Remove(unit.Key);
            }
        }
    }

    public class MinimumCostStack
    {
        Stack<int> stack, minimumCost;

        public MinimumCostStack()
        {
            stack = new Stack<int>();
            minimumCost = new Stack<int>();
        }

        public void Push(int index)
        {
            stack.Push(index);
            index = Math.Min(index,
                    minimumCost.Count == 0
                    ? index
                    : minimumCost.Peek());

            minimumCost.Push(index);
        }

        public void Pop()
        {
            stack.Pop();
            minimumCost.Pop();
        }

        public int Top()
        {
            return stack.Peek();
        }

        public int GetMinimumCost()
        {
            return minimumCost.Peek();
        }
    }
}
