using System;
using System.Collections.Generic;

class Node<T> where T : IComparable<T>
{
    public T Item { get; set; }
    public Node<T> Left { get; set; }
    public Node<T> Right { get; set; }
    public Node<T> Parent { get; set; }

    public Node(T item, Node<T> parent = null, Node<T> left = null, Node<T> right = null)
    {
        this.Item = item;
        this.Parent = parent;
        this.Left = left;
        this.Right = right;
    }
}

class SplayTree<T> where T : IComparable<T>
{
    private Node<T> root { get; set; }
    private Node<T> previousRoot { get; set; }

    public SplayTree()
    {
        root = null;
    }

    private Stack<Node<T>> Access(T item)
    {
        Stack<Node<T>> path = new Stack<Node<T>>();
        Node<T> current = root;

        while (current != null)
        {
            path.Push(current);
            int compareItems = item.CompareTo(current.Item);

            if (compareItems == 0)
                break;

            current = compareItems < 0 ? current.Left : current.Right;
        }

        return path;
    }

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

    private Node<T> RotateLeft(Node<T> node)
    {
        Node<T> right = node.Right;
        node.Right = right.Left;
        right.Left = node;
        right.Parent = node.Parent;
        node.Parent = right;

        if (node.Right != null)
            node.Right.Parent = node;

        return right;
    }

    private Node<T> RotateRight(Node<T> node)
    {
        Node<T> left = node.Left;
        node.Left = left.Right;
        left.Right = node;
        left.Parent = node.Parent;
        node.Parent = left;

        if (node.Left != null)
            node.Left.Parent = node;

        return left;
    }

    public void Insert(T item)
    {
        Node<T> newNode = new Node<T>(item);

        if (root == null)
        {
            root = newNode;
            previousRoot = null;
            return;
        }

        this.previousRoot = root;
        Stack<Node<T>> path = Access(item);
        Node<T> parent = path.Peek();

        if (item.CompareTo(parent.Item) < 0)
        {
            parent.Left = newNode;
        }
        else
        {
            parent.Right = newNode;
        }

        Splay(newNode, path);
    }

    public void Remove(T item)
    {
        Stack<Node<T>> path = Access(item);

        if (item.CompareTo(path.Peek().Item) != 0)
        {
            path.Pop();
            Splay(path.Peek(), path);
            return;
        }

        Node<T> removeNode = path.Pop();
        Splay(removeNode, path);

        Node<T> leftMax = null;
        Node<T> current = this.root.Left;

        while (current != null)
        {
            if (current.Right == null)
            {
                leftMax = current;
                break;
            }

            current = current.Right;
        }

        if (leftMax != null)
        {
            path = Access(leftMax.Item);
            path.Pop();
            Splay(leftMax, path);
            leftMax.Right = leftMax.Right?.Right;
        }
        else
        {
            this.root = this.root.Right;
        }
    }

    public bool Contains(T item)
    {
        Stack<Node<T>> path = Access(item);
        if (path.Count > 0 && item.CompareTo(path.Peek().Item) == 0)
        {
            Node<T> temp = path.Pop();
            Splay(temp, path);
            return true;
        }
        return false;
    }

    public SplayTree<T> Undo()
    {
        // Check if the previousRoot is null, indicating there is no operation to undo
        if (previousRoot == null)
        {
            Console.WriteLine("Undo unsuccessful. No previous operation to undo.");
            return this;
        }

        // Create a deep copy of the tree before the last operation
        SplayTree<T> clonedTree = (SplayTree<T>)this.Clone();

        // Set the current root to be the previous root
        this.root = previousRoot;

        Console.WriteLine("Undo successful.");

        // Clear the previousRoot to indicate that the undo operation has been completed
        previousRoot = null;

        // Return the cloned tree, which represents the state before the last operation
        return clonedTree;
    }



    public object Clone()
    {
        SplayTree<T> clonedTree = new SplayTree<T>();
        clonedTree.root = CloneNodeHelper(this.root, null);

        return clonedTree;
    }

    private Node<T> CloneNodeHelper(Node<T> node, Node<T> parent)
    {
        if (node == null)
        {
            return null;
        }

        Node<T> newNode = new Node<T>(node.Item, parent);
        newNode.Left = CloneNodeHelper(node.Left, newNode);
        newNode.Right = CloneNodeHelper(node.Right, newNode);

        return newNode;
    }


    private Node<T> CloneNodeHelper(Node<T> node)
    {
        if (node == null)
        {
            return null;
        }

        Node<T> newNode = new Node<T>(node.Item);
        newNode.Left = CloneNodeHelper(node.Left);
        newNode.Right = CloneNodeHelper(node.Right);

        return newNode;
    }

    public override bool Equals(Object obj)
    {
        if (obj == null || !(obj is SplayTree<T>))
        {
            return false;
        }

        SplayTree<T> compareTree = (SplayTree<T>)obj;

        return EqualsHelper(this.root, compareTree.root);
    }

    private bool EqualsHelper(Node<T> node1, Node<T> node2)
    {
        if (node1 == null && node2 == null)
        {
            return true;
        }

        if (node1 == null || node2 == null)
        {
            return false;
        }

        Console.WriteLine($"Comparing nodes: {node1.Item} and {node2.Item}");

        bool parentEquality = (node1.Parent == null && node2.Parent == null) ||
                             (node1.Parent != null && node2.Parent != null && EqualityComparer<T>.Default.Equals(node1.Parent.Item, node2.Parent.Item));

        return EqualityComparer<T>.Default.Equals(node1.Item, node2.Item)
            && EqualsHelper(node1.Left, node2.Left)
            && EqualsHelper(node1.Right, node2.Right)
            && parentEquality;
    }



    // Print and traversal methods as you've provided

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

        // Perform Undo
        SplayTree<int> undoTree = tree.Undo();
        undoTree.Print();

        // Insert the same item again and verify using Equals method
        tree.Insert(-2);
        Console.WriteLine("Original tree and tree after Undo are equal: " + tree.Equals(undoTree));
        tree.Print();
    }
}
