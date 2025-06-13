using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSearchApp
{
    public class AVLTree<T> where T : IComparable<T>
    {
        private class Node
        {
            public T Value;
            public Node Left, Right;
            public int Height = 1;
            public Node(T value) => Value = value;
        }

        private Node root;

        public void Insert(T value) => root = Insert(root, value);

        private Node Insert(Node node, T value)
        {
            if (node == null) return new Node(value);
            if (value.CompareTo(node.Value) < 0) node.Left = Insert(node.Left, value);
            else node.Right = Insert(node.Right, value);
            node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));
            return Balance(node);
        }

        private Node Balance(Node node)
        {
            int balance = GetBalance(node);
            if (balance > 1)
            {
                if (GetBalance(node.Left) < 0) node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            }
            if (balance < -1)
            {
                if (GetBalance(node.Right) > 0) node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }
            return node;
        }

        private int Height(Node node) => node?.Height ?? 0;
        private int GetBalance(Node node) => Height(node.Left) - Height(node.Right);
        private Node RotateRight(Node y) => Rotate(y, true);
        private Node RotateLeft(Node x) => Rotate(x, false);

        private Node Rotate(Node node, bool right)
        {
            Node child = right ? node.Left : node.Right;
            if (right)
            {
                node.Left = child.Right;
                child.Right = node;
            }
            else
            {
                node.Right = child.Left;
                child.Left = node;
            }
            node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));
            child.Height = 1 + Math.Max(Height(child.Left), Height(child.Right));
            return child;
        }

        public IEnumerable<T> InOrder()
        {
            var stack = new Stack<Node>();
            var node = root;
            while (stack.Count > 0 || node != null)
            {
                while (node != null)
                {
                    stack.Push(node);
                    node = node.Left;
                }
                node = stack.Pop();
                yield return node.Value;
                node = node.Right;
            }
        }
    }
}