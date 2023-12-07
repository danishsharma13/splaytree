// Group members: Danish Sharma, Sami Ali, Tushar Dhiman
// Group members IDs: 0623392, 0791752, 0757538
// Assignment 2 : Splay Tree

// Class Node
// Summary: The Node class is used to create a node-tree structure
//          for Splay tree class
class Node<T> where T: IComparable<T>
{
    // Data members
    public T Item { get; set; }
    public Node<T> Left { get; set; }
    public Node<T> Right { get; set; }

    // Summary: 2-args constructor that creates a Node object for SplayTree
    //          and populates the data members
    public Node(T item, Node<T>? left = null, Node<T>? right = null)
    {
        this.Item = item;
        this.Left = left;
        this.Right = right;
    }
}

// -------------------------------------------------------------------------------

// Class SplayTree
// Summary: The Splay tree class is implemented without the use of recursive
//          functions to perform search, insert, removal of items
class SplayTree<T> where T: IComparable<T>
{
    // Data members
    private Node<T> root { get; set; }
    // previousRoot for undo operation
    private Node<T> previousRoot { get; set; }

    // Summary: No-args constructor that creates an object of SplayTree class
    //          and sets root to null
    public SplayTree()
    {
        root = null;
    }

    // Summary: An Access method is a non-recursive method that returns the
    //          access path of the nodes from root to sthe last node accessed 
    private Stack<Node<T>> Access(T item)
    {
        // Create a new stack to store the nodes in the access path
        // Create a new node that traverses through the tree
        Stack<Node<T>> path = new Stack<Node<T>>();
        Node<T> current = root;
        
        // While loop: current is not null, we keep traversing through the
        //      tree to find "item" or we hit null
        while (current != null)
        {
            // Adding nodes into the stack
            path.Push(current);

            // Compare the Items to check if we found out item or we 
            //      need to traverse deeper in the tree
            int compareItems = item.CompareTo(current.Item);
            // If CompareItems is 0 that means we have hit our target Item
            if (compareItems == 0)
                break;

            // If Compareitems is less than 0 then we go tree's left side,
            //      otherwise vice versa
            current = compareItems < 0 ? current.Left : current.Right;
        }
        
        // Return path as it will contain the full access path
        return path;
    }

    // Summary: A Splay method is a non-recursive method that splays the
    //          node to the top using zig and/or zag rotations.
    private void Splay(Node<T> node, Stack<Node<T>> S)
    {
        // While loop: S.Count is greater than/equal 1 that means there is more
        //          access nodes that needs to be adjusted
        while (S.Count >= 1 && node != null)
        {
            // Creating parent, grandparent and grandgrandparent Nodes that
            // will allow us to properly perform the Zig and/or Zag rotation
            Node<T> parent = S.Pop();
            Node<T> grandparent = S.Count > 0 ? S.Pop() : null;
            Node<T> grandgrandparent = S.Count > 0 ? S.Peek() : null;

            // Zig Step: when grandparent is null and parent is not null
            //      means we are at now rotating at the root node
            if (grandparent == null) 
            {
                // If parent left is the node, then we perform Right
                //      rotation and set the root to be the "node"
                // If parent right is the node, then we perform Left
                //      rotation and set the root to be the "node"
                if (parent.Left == node)
                    this.root = RotateRight(parent);
                else
                    this.root = RotateLeft(parent);

                if (S.Count < 1) return;
            }
            // Zig-Zig Step (Right-Right): when the grandparent and
            //      parent is to the left
            if (grandparent.Left != null && parent.Left != null)
            {
                if (parent.Item.CompareTo(grandparent.Left.Item) == 0
                && node.Item.CompareTo(parent.Left.Item) == 0)
                {
                    // If grandparent is the root then we rotate right and
                    //      set the parent to root (grandgrandparent is null through logic)
                    // If grandparent is to the grandgrandparent's left then 
                    //      rotate right and set left to be the parent
                    // If grandparent is to the grandgrandparent's right then 
                    //      rotate right and set right to be the parent
                    if (grandparent == root)
                    {
                        this.root = RotateRight(grandparent);
                    }
                    else if (grandparent.Item.CompareTo(grandgrandparent.Left.Item) == 0)
                    {
                        grandgrandparent.Left = RotateRight(grandparent);
                    }
                    else if (grandparent.Item.CompareTo(grandgrandparent.Right.Item) == 0)
                    {
                        grandgrandparent.Right = RotateRight(grandparent);
                    }

                    // If parent is the root (after above rotation) then we rotate right and
                    //      set the "node" to root (grandgrandparent is null through logic)
                    // If parent is to the grandgrandparent's left then 
                    //      rotate right and set left to be the "node"
                    // If parent is to the grandgrandparent's right then 
                    //      rotate right and set right to be the "node"
                    if (parent == root)
                    {
                        this.root = RotateRight(parent);
                    }
                    else if (parent.Item.CompareTo(grandgrandparent.Left.Item) == 0)
                    {
                        grandgrandparent.Left = RotateRight(parent);
                    }
                    else if (parent.Item.CompareTo(grandgrandparent.Right.Item) == 0)
                    {
                        grandgrandparent.Right = RotateRight(parent);
                    }

                    if (S.Count < 1) return;
                }
            }
            // Zig-Zig Step (Left-Left): when the grandparent and
            //      parent is to the right
            if (grandparent.Right != null && parent.Right != null)
            {
                if (parent.Item.CompareTo(grandparent.Right.Item) == 0
                && node.Item.CompareTo(parent.Right.Item) == 0)
                {
                    // If grandparent is the root then we rotate left and
                    //      set the parent to root (grandgrandparent is null through logic)
                    // If grandparent is to the grandgrandparent's left then 
                    //      rotate left and set left to be the parent
                    // If grandparent is to the grandgrandparent's right then 
                    //      rotate left and set right to be the parent
                    if (grandparent == root)
                    {
                        this.root = RotateLeft(grandparent);
                    }
                    else if (grandparent.Item.CompareTo(grandgrandparent.Left.Item) == 0)
                    {
                        grandgrandparent.Left = RotateLeft(grandparent);
                    }
                    else if (grandparent.Item.CompareTo(grandgrandparent.Right.Item) == 0)
                    {
                        grandgrandparent.Right = RotateLeft(grandparent);
                    }

                    // If parent is the root (after above rotation) then we rotate right and
                    //      set the "node" to root (grandgrandparent is null through logic)
                    // If parent is to the grandgrandparent's left then 
                    //      rotate right and set left to be the "node"
                    // If parent is to the grandgrandparent's right then 
                    //      rotate right and set right to be the "node"
                    if (parent == root)
                    {
                        this.root = RotateLeft(parent);
                    }
                    else if (parent.Item.CompareTo(grandgrandparent.Left.Item) == 0)
                    {
                        grandgrandparent.Left = RotateLeft(parent);
                    }
                    else if (parent.Item.CompareTo(grandgrandparent.Right.Item) == 0)
                    {
                        grandgrandparent.Right = RotateLeft(parent);
                    }

                    if (S.Count < 1) return;
                }
            }
            // Zig-Zag Step: when the node, parent and grandparent are in
            //      zig-zag path, then we rotate in a zig-zag way
            // If parent's left is the node meaning grandparent's right
            //      if parent node, then we rotate right and then left
            // If parent's right is the node meaning grandparent's left
            //      if parent node, then we rotate left and then right
            if (parent.Left != null)
            {
                if (node.Item.CompareTo(parent.Left.Item) == 0)
                {
                    // Set grandparent to be "node" using rotate right
                    grandparent.Right = RotateRight(parent);

                    // If grandparent is at root then set root to be "node"
                    //      using left rotation
                    // If grandparent is of grandgrandparent's left then we rotate
                    //      left and set left to be "node"
                    // If grandparent is of grandgrandparent's right then we rotate
                    //      left and set right to be "node"
                    if (grandparent == root)
                    {
                        this.root = RotateLeft(grandparent);
                    }
                    else if (grandparent.Item.CompareTo(grandgrandparent.Left.Item) == 0)
                    {
                        grandgrandparent.Left = RotateLeft(grandparent);
                    }
                    else if (grandparent.Item.CompareTo(grandgrandparent.Right.Item) == 0)
                    {
                        grandgrandparent.Right = RotateLeft(grandparent);
                    }

                    if (S.Count < 1) return;
                }

            }
            if (parent.Right != null)
            {
                if (node.Item.CompareTo(parent.Right.Item) == 0)
                {
                    // Set grandparent to be "node" using rotate left
                    grandparent.Left = RotateLeft(parent);

                    // If grandparent is at root then set root to be "node"
                    //      using right rotation
                    // If grandparent is of grandgrandparent's left then we rotate
                    //      right and set left to be "node"
                    // If grandparent is of grandgrandparent's right then we rotate
                    //      right and set right to be "node"
                    if (grandparent == root)
                    {
                        this.root = RotateRight(grandparent);
                    }
                    else if (grandparent.Item.CompareTo(grandgrandparent.Left.Item) == 0)
                    {
                        grandgrandparent.Left = RotateRight(grandparent);
                    }
                    else if (grandparent.Item.CompareTo(grandgrandparent.Right.Item) == 0)
                    {
                        grandgrandparent.Right = RotateRight(grandparent);
                    }

                    if (S.Count < 1) return;
                }
            }
        }
    }

    // Summary: RotateLeft helper method is taken from prof's code as reference
    //      which rotates the tree to left and re-adjusts the child nodes
    private Node<T> RotateLeft(Node<T> node)
    {
        Node<T> right = node.Right;
        node.Right = right.Left;
        right.Left = node;

        return right;
    }

    // Summary: RotateRight helper method is taken from prof's code as reference
    //      which rotates the tree to right and re-adjusts the child nodes
    private Node<T> RotateRight(Node<T> node)
    {
        Node<T> left = node.Left;
        node.Left = left.Right;
        left.Right = node;

        return left;
    }

    // Summary: Insert method takes in an item, creates a node and then splays
    //      it inside the tree after adding it into the tree first
    public void Insert(T item)
    {
        // Create a new node for it to be stored in the tree
        Node<T>? newNode = new Node<T>(item);

        // If the root is null, then we simply insert the item in the tree
        if (root == null)
        {
            root = newNode;
            previousRoot = null;
            return;
        }

        // Save the current root node for potential undo
        this.previousRoot = root;  
        // Get the access path for the item
        Stack<Node<T>> path = Access(item);

        // Create a parentNode to store the newNode in the tree
        Node<T> parent = path.Peek();

        // If item is less than parent's item then store it in
        //      left side of the parent
        // If item is greater than parent's item then store it in
        //      right side of the parent
        if (item.CompareTo(parent.Item) < 0)
        {
            parent.Left = newNode;
        }
        else
        {
            parent.Right = newNode;
        }

        // Lastly Splay the newNode to the top using the access path
        //      This will get the newNode to the root
        Splay(newNode, path);
    }

    // Summary: Remove method will splay the item to the root, then
    //      splay the left side max node, then removes the item from
    //      the tree
    public void Remove(T item) 
    {
        // Get the access path for the item to remove
        Stack<Node<T>> path = Access(item);

        // If item is not equal to the path.Peek item (the item itself
        //      in the tree) then we splay the the last accessed item
        //      to the root
        if (item.CompareTo(path.Peek().Item) != 0)
        {
            // Pop the first item from the path as it is the
            //      last accessed item
            path.Pop();

            // Splay the last accessed item to the root using the
            //      access path
            Splay(path.Peek(), path);

            // return as there is no nodes to remove
            return;
        }

        // Create item to remove node (since the item to remove
        //      is within the access path
        Node<T> removeNode = path.Pop();

        // Splay the item to remove to the root
        Splay(removeNode, path);

        // Create a node for left side's max node and a node
        //      for current node set to root's left to traverse the tree
        Node<T> leftMax = null;
        Node<T> current = this.root.Left;

        // While loop: Traverse the tree and find the max node 
        //      in the left side of tree
        while (current != null)
        {
            // If current's right is null, then we have our max
            //      node in the left side of tree, set leftMax to
            //      current
            if (current.Right == null)
            {
                leftMax = current;
                break;
            }

            // If we are not at the max node in the left subtree
            //      then make current to current's right node and
            //      loop again
            current = current.Right;
        }

        // If root node does have left side nodes then we splay it to the root
        // If the root node does not have left side nodes then just set
        //      root node to be root = root.Right
        if (leftMax != null)
        {
            // Get the access path for the leftMax item
            path = Access(leftMax.Item);

            // Pop the leftMax node from path
            path.Pop();

            // Get the leftMax node to the root
            Splay(leftMax, path);

            // Remove the leftMax's right node since it is the item to be removed
            //      since leftMax will not have right node, it can easily
            //      remove the desired node
            leftMax.Right = leftMax.Right.Right;
        }
        else
        {
            this.root = this.root.Right;
        }
    }

    // Summary: Contains method checks if the item is found within the tree
    //      if not then the last accessed item is splayed to the root
    public bool Contains(T item)
    {
        // Get the access path which will either contain the 
        //      item we are looking for or not
        Stack<Node<T>> path = Access(item);
        // If path count is greater than 0 and the item at the top of
        //      stack is same as item we are looking for then return true
        // If not then return false
        return (path.Count > 0 && item.CompareTo(path.Peek().Item) == 0);
    }

    // Summary: Clone method will create a deep copy of the current splay tree
    //      and uses recursive method (stated by prof in class)
    public object Clone()
    {
        // Create a new SplayTree that will be cloned
        SplayTree<T> clonedTree = new SplayTree<T>();
        // Make clonedTree to be equal to CloneNodeHelper as it will
        //      return the root of the cloned tree
        clonedTree.root = CloneNodeHelper(this.root);
        
        return clonedTree;
    }

    // Summary: CloneNodeHelper method is a helper method that is used
    //      for recursive calls to properly clone/copy each node of the tree
    private Node<T> CloneNodeHelper(Node<T> node)
    {
        // If the node of the original tree is null then no need to clone
        if (node == null)
        {
            return null;
        }

        // Create a newNode that will be a copy of the original tree's node
        Node<T> newNode = new Node<T>(node.Item);
        // Then we set left and right nodes of the newNode using recursion calls
        //      of the helper method itself, first going all the way to the left
        //      then all the way to the right side of the tree
        newNode.Left = CloneNodeHelper(node.Left);
        newNode.Right = CloneNodeHelper(node.Right);

        // lastly return the newNode, in the end of recursive call it will be 
        //      the root node
        return newNode;
    }

    // Summary: Equals method checks if the two trees are the same by traversing
    //      node by node
    public override bool Equals(Object obj)
    {
        // If obj is null or obj is not SplayTree<T> then return false
        if (obj == null || !(obj is SplayTree<T>))
        {
            return false;
        }

        // Create compareTree SplayTree based on obj
        SplayTree<T> compareTree = (SplayTree<T>)obj;

        // Use the EqualsHelper recursive function that will check
        //      node by node
        return EqualsHelper(this.root, compareTree.root);
    }

    // Summary: EqualsHelper function checks each node if they are equal
    private bool EqualsHelper(Node<T> node1, Node<T> node2)
    {
        // If Node 1 AND Node 2 are null then return true
        if (node1 == null && node2 == null)
        {
            return true;
        }

        // If Node 1 OR Node 2 is null then return false
        //      both need to be null to be equal
        if (node1 == null || node2 == null)
        {
            return false;
        }

        // Return true if node1 equals node 2 AND
        //      recursive call Equals for node1.Left and node2.Left AND
        //      recursive call Equals for node1.Right and node2.Right
        return node1.Item.Equals(node2.Item) 
            && Equals(node1.Left, node2.Left) 
            && Equals(node1.Right, node2.Right);
    }

    public void Print()
    {
        Console.WriteLine("\n\nInorderTraversal: \n\t");
        InorderTraversal(this.root);
        Console.WriteLine("\n\nPostorderTraversal: \n\t");
        PostorderTraversal(this.root);

        Console.WriteLine("\n\nPreorderTraversal: \n\t");
        PreorderTraversal(this.root);
    }

    public void InorderTraversal(Node<T> node)
    {
        if (node != null)
        {
            InorderTraversal(node.Left);
            Console.Write(node.Item + " ");
            InorderTraversal(node.Right);
        }
    }

    public void PostorderTraversal(Node<T> node)
    {
        if (node != null)
        {
            PostorderTraversal(node.Left);
            PostorderTraversal(node.Right);
            Console.Write(node.Item + " ");
        }
    }

    public void PreorderTraversal(Node<T> node)
    {
        if (node != null)
        {
            Console.Write(node.Item + " ");
            PreorderTraversal(node.Left);
            PreorderTraversal(node.Right);
        }
    }

}

class Program
{
    public static void Main(string[] args)
    {
        SplayTree<int> tree = new SplayTree<int>();

        tree.Insert(1);
        tree.Insert(2);
        tree.Insert(3);
        tree.Insert(5);
        tree.Insert(4);
        tree.Insert(6);
        tree.Insert(-7);
        tree.Insert(-2);

        tree.Print();

        tree.Remove(1);

        tree.Print();

    }
}



