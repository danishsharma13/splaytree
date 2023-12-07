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
    //          and sets root and previousRoot to null
    public SplayTree()
    {
        this.root = null;
        this.previousRoot = null;
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
            // NOTE: This while also inserts the "item" node in the stack
            //      Need to pop the stack to get parent's information
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

            // Multiple if statements to ensure the parent or grandparent's
            //      left and/or right nodes are not null before comparing

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

                // If S.Count < 1 then return, also returning it
                //      because we do not want to hit any other if
                //      statements if we are done rotating
                //      (same logic for other if statements)
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
                    else if (grandgrandparent.Left != null && grandparent.Item.CompareTo(grandgrandparent.Left.Item) == 0)
                    {
                        grandgrandparent.Left = RotateRight(grandparent);
                    }
                    else if (grandgrandparent.Right != null && grandparent.Item.CompareTo(grandgrandparent.Right.Item) == 0)
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
                    else if (grandgrandparent.Left != null && parent.Item.CompareTo(grandgrandparent.Left.Item) == 0)
                    {
                        grandgrandparent.Left = RotateRight(parent);
                    }
                    else if (grandgrandparent.Right != null && parent.Item.CompareTo(grandgrandparent.Right.Item) == 0)
                    {
                        grandgrandparent.Right = RotateRight(parent);
                    }

                    // If S.Count < 1 then return
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
                    else if (grandgrandparent.Left != null && grandparent.Item.CompareTo(grandgrandparent.Left.Item) == 0)
                    {
                        grandgrandparent.Left = RotateLeft(grandparent);
                    }
                    else if (grandgrandparent.Right != null && grandparent.Item.CompareTo(grandgrandparent.Right.Item) == 0)
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
                    else if (grandgrandparent.Left != null && parent.Item.CompareTo(grandgrandparent.Left.Item) == 0)
                    {
                        grandgrandparent.Left = RotateLeft(parent);
                    }
                    else if (grandgrandparent.Right != null && parent.Item.CompareTo(grandgrandparent.Right.Item) == 0)
                    {
                        grandgrandparent.Right = RotateLeft(parent);
                    }

                    // If S.Count < 1 then return
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

                    // If S.Count < 1 then return
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

                    // If S.Count < 1 then return
                    if (S.Count < 1) return;
                }
            }
        }
    }

    // Summary: RotateLeft helper method is taken from prof's code as reference
    //      which rotates the tree to left and re-adjusts the child nodes
    private Node<T> RotateLeft(Node<T> node)
    {
        // These steps will left rotate the "parent node" with it's child node
        //      and return the rotated child node
        Node<T> right = node.Right;
        node.Right = right.Left;
        right.Left = node;

        return right;
    }

    // Summary: RotateRight helper method is taken from prof's code as reference
    //      which rotates the tree to right and re-adjusts the child nodes
    private Node<T> RotateRight(Node<T> node)
    {
        // These steps will right rotate the "parent node" with it's child node
        //      and return the rotated child node
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
            // Set root to be newNode, previousRoot to be null and return
            this.root = newNode;
            this.previousRoot = null;
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
            //      since leftMax(root) will not have right node, it can easily
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
        //      stack is same as item we are looking for then splay
        //      and return true
        // If not then return false
        if (path.Count > 0 && item.CompareTo(path.Peek().Item) == 0)
        {
            // Pop the first item as it is the item we are searching for
            //      and store it as a temp
            // Then splay the item to the top
            Node<T> temp = path.Pop();
            Splay(temp, path);

            return true;
        }
        return false;
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
        return node1.Item.CompareTo(node2.Item) == 0
            && EqualsHelper(node1.Left, node2.Left) 
            && EqualsHelper(node1.Right, node2.Right);
    }

    // Summary: Undo method restores the tree to its original form before
    //      the last insertion method
    public SplayTree<T> Undo()
    {
        // If previousRoot is null then return null
        if (this.previousRoot == null)
        {
            return null;
        }

        // Create an insertedRoot to keep track of the inserted node's
        //      operations
        Node<T> insertedRoot = this.root;

        // Create a clone of this tree before we implement
        SplayTree<T> clonedTree = (SplayTree<T>)this.Clone();

        // Create accesspath for nodes to the left and right of
        //      the insertedRoot to splay them to the root
        Stack<Node<T>> path = null;

        // First splay the previousRoot to the root using Access path,
        //      store the popped element and then splay
        path = Access(previousRoot.Item);
        Node<T> temp = path.Pop();
        Splay(temp, path);

        // While insertedRoot.Left and insertedRoot.Right is not null, we need
        //      keep looping until we rotate the root node to the leaf node
        while (insertedRoot.Left != null || insertedRoot.Right != null)
        {
            // Multiple if statements that will rotate the insertedRoot to the
            //      leaf node using zig-zag methods

            // Zig-Zig (Left-Left): If the insertedRoot left and left left
            //      is not null then we can try right rotations
            if (insertedRoot.Left != null && insertedRoot.Left.Left != null)
            {
                Node<T> parent = RotateRight(insertedRoot);
                RotateRight(parent);
            }
            // Zig-Zig (Right-Right): If the insertedRoot right and right right
            //      is not null then we can try left rotations
            else if (insertedRoot.Right != null && insertedRoot.Right.Right != null)
            {
                Node<T> parent = RotateLeft(insertedRoot);
                RotateLeft(parent);
            }
            // Zig-Zag (Left-Right): If the insertedRoot left and left right
            //      is not null then we can try left-right rotations
            else if (insertedRoot.Left != null && insertedRoot.Left.Right != null)
            {
                RotateRight(insertedRoot.Left);
                RotateRight(insertedRoot);
            }
            // Zig-Zag (Right-Left): If the insertedRoot right and right left
            //      is not null then we can try right-left rotations
            else if (insertedRoot.Right != null && insertedRoot.Right.Left != null)
            {
                RotateRight(insertedRoot.Left);
                RotateRight(insertedRoot);
            }
        }

        // If the insertedRoot is at the leaf, then we can find the access
        //      path for it, pop the first element(insertedRoot) and then
        //      store the next pop element that will be the parent of the
        //      newly inserted element (insertedRoot)
        if (insertedRoot.Left == null && insertedRoot.Right == null)
        {
            path = Access(insertedRoot.Item);
            path.Pop();
            temp = path.Pop();

            // If the left side of temp (parent) is not null and parent left item
            //       is same as insertedRoot then we remove the node
            // If the right side of temp (parent) is not null and parent right item
            //       is same as insertedRoot then we remove the node
            if (temp.Left != null && temp.Left.Item.CompareTo(insertedRoot.Item) == 0)
            {
                temp.Left = null;
            }
            else if (temp.Right != null && temp.Left.Item.CompareTo(insertedRoot.Item) == 0)
            {
                temp.Right = null;
            }
        }

        // Return splay tree
        return this;
    }

    // Summary: Print class is used to print out the Inorder, Preorder
    //      and Postorder for the splay tree
    public void Print()
    {
        Console.Write("\nInorderTraversal: ");
        InorderTraversal(this.root);
        Console.Write("\nPreorderTraversal: ");
        PreorderTraversal(this.root);
        Console.Write("\nPostorderTraversal: ");
        PostorderTraversal(this.root);
        
    }

    // Summary: IndorderTraversal is a recursive function that gives
    //      inorder items in the tree
    public void InorderTraversal(Node<T> node)
    {
        // If the node is not null then do the recursive calls
        if (node != null)
        {
            // Steps: Move left until null, print node, move
            //      right until null (recursive)
            InorderTraversal(node.Left);
            Console.Write(node.Item + " ");
            InorderTraversal(node.Right);
        }
    }

    // Summary: PostorderTraversal is a recursive function that gives
    //      postorder items in the tree
    public void PostorderTraversal(Node<T> node)
    {
        // If the node is not null then do the recursive calls
        if (node != null)
        {
            // Steps: Move left until null, move right until null,
            //      print item (recursive)
            PostorderTraversal(node.Left);
            PostorderTraversal(node.Right);
            Console.Write(node.Item + " ");
        }
    }

    // Summary: PreorderTraversal is a recursive function that gives
    //      preorder items in the tree
    public void PreorderTraversal(Node<T> node)
    {
        // If the node is not null then do the recursive calls
        if (node != null)
        {
            // Steps: Print item, Move left until null,
            //      move right until null (recursive)
            Console.Write(node.Item + " ");
            PreorderTraversal(node.Left);
            PreorderTraversal(node.Right);
        }
    }

}

// Summary: Program class that will create the SplayTree and test
//      methods that were implemented by our team
class Program
{
    public static void Main(string[] args)
    {
        SplayTree<int> tree = new SplayTree<int>();

        Console.WriteLine("INSERTING 10, 20, 5, 30, 40, 50");

        tree.Insert(10);
        tree.Insert(20);
        tree.Insert(5);
        tree.Insert(30);
        tree.Insert(40);
        tree.Insert(50);

        tree.Print();

        Console.WriteLine("\n\nUNDO 50 AND THEN RE-INSERTING 50 BACK IN, RESULT: ");

        tree.Undo();

        tree.Insert(50);

        tree.Print();

        Console.WriteLine();

    }
}



