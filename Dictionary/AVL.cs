using Dictionary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Dictionary
{

    public class AVL
    {

       public Node root;
        public AVL()
        {

        }

        public bool isempty()
        {
            return root == null;
        }
        public int count(Node root)
        {
            if (root == null)
            {
                return 0;
            }
            else
            {
                return 1 + count(root.right) + count(root.left);
            }
        }


        public void insert(DicIndex data)
        {
            Node newData = new Node(data);
            if (root == null)
            {
                root = newData;
            }
            else
            {

                root = Insert(root, newData);
            }
        }

        private int CompareNode(Node First, Node Second)
        {
            if (First == null)
                return 1;
            else if (Second == null)
                return -1;
            else
            {
                char[] FirstNode = First.data.word;
                char[] SecondNode = Second.data.word;
                string charsStr = new string(FirstNode);
                string charsStr2 = new string(SecondNode);
                int x = charsStr.CompareTo(charsStr2);

                //returns 1,-1 or 0 according to the string
                return x;
            }
        }


        //works recursively to enter the data to the tree
        private Node Insert(Node current, Node n)
        {
            if (current == null)
            {
                current = n;
                return current;
            }
            //when the new node is less than the current
            else if (CompareNode(n, current) == -1)
            {
                current.left = Insert(current.left, n);
                current = balance_tree(current);
            }
            //when the new node is greater than the current
            else if (CompareNode(n, current) == 1)
            {
                current.right = Insert(current.right, n);
                current = balance_tree(current);
            }
            return current;
        }

        private Node balance_tree(Node current)
        {
            int b_factor = getBalance(current);
            if (b_factor > 1)
            {
                //when it is inserted to the left and the left balance is greather than 0
                /*

                    c
                     \
                      b
                       \
                        a
                  */
                if (getBalance(current.left) > 0)
                {
                    current = singleLeft(current);
                }
                //when it is inserted to the left and then to the right
                /*
                   c
                  /
                 a
                  \
                   b
                      */
                else
                {
                    current = doubleLR(current);
                }
            }
            else if (b_factor < -1)
            {
                //when it is inserted to the right and then to the left balance is greather than 0
                /*
                   c
                    \
                     b
                    /
                   a 

                  */
                if (getBalance(current.right) > 0)
                {
                    current = doubleRL(current);
                }

                //when it is inserted to the left and the left balance is greather than 0
                /*
                  c
                 /
                b
               /
              a
                  */
                else
                {
                    current = singleRight(current);
                }
            }
            return current;
        }

        public void Delete(Node target)
        {//and here
            root = Delete(root, target);
        }
        private Node Delete(Node current, Node target)
        {
            Node parent;
            if (current == null) 
                return null;
            else
            {
                //left subtree
                //when target is less than the current
                if (CompareNode(target, current) == -1)
                {
                    current.left = Delete(current.left, target);

                    if (getBalance(current) == -2)
                    {
                        if (getBalance(current.right) <= 0)
                        {
                            current = singleRight(current);
                        }
                        else
                        {
                            current = doubleRL(current);
                        }
                    }
                }
                //right subtree
                else if (CompareNode(target, current) == 1)
                {
                    current.right = Delete(current.right, target);
                    if (getBalance(current) == 2)
                    {
                        if (getBalance(current.left) >= 0)
                        {
                            current = singleLeft(current);
                        }
                        else
                        {
                            current = doubleLR(current);
                        }
                    }
                }
                //if target is found
                else
                {
                    if (current.right != null)
                    {
                        //delete its inorder successor
                        parent = current.right;
                        while (parent.left != null)
                        {
                            parent = parent.left;
                        }
                        current.data = parent.data;
                        current.right = Delete(current.right, parent);
                        if (getBalance(current) == 2)//rebalancing
                        {
                            if (getBalance(current.left) >= 0)
                            {
                                current = singleLeft(current);
                            }
                            else { current = doubleLR(current); }
                        }
                    }
                    else
                    {   //if current.left != null
                        return current.left;
                    }
                }
            }
            return current;
        }
        /*   */
        public Node Find(Node key)
        {
            Node a = Find(key, root);
            if (CompareNode(a, key) == 0)
            {
                return a;
            }
            else
            {
                DicIndex d = new DicIndex();
                d.word = null;
                d.index.Insert(-1);
                Node ret = new Node(d);
                return ret;

            }
        }
        private Node Find(Node target, Node current)
        {
            if (current != null)
            {
                //target < current
             if (CompareNode(target, current) == -1)
                {
                    return Find(target, current.left);
                }
                //target = current
                else if (CompareNode(target, current) == 0)
                {
                        return current;
                }
             else
                        return Find(target, current.right);
                }
            else
                return current;

        }

        private int max(int l, int r)
        {
            return l > r ? l : r;
        }
        private int getHeight(Node current)
        {
            int height = 0;
            if (current != null)
            {
                int l = getHeight(current.left);
                int r = getHeight(current.right);
                int m = max(l, r);
                //we add 1 because to include the main node we send to find the height
                height = m + 1;
            }
            return height;
        }
        private int getBalance(Node node)
        {
            if (node == null)
                return 0;
            return getHeight(node.left) - getHeight(node.right);
        }
        private Node singleRight(Node parent)
        {
            Node pivot = parent.right;
            parent.right = pivot.left;
            pivot.left = parent;
            return pivot;
        }
        private Node singleLeft(Node parent)
        {
            Node pivot = parent.left;
            parent.left = pivot.right;
            pivot.right = parent;
            return pivot;
        }
        private Node doubleLR(Node parent)
        {
            Node pivot = parent.left;
            parent.left = singleRight(pivot);
            return singleLeft(parent);
        }
        private Node doubleRL(Node parent)
        {
            Node pivot = parent.right;
            parent.right = singleLeft(pivot);
            return singleRight(parent);
        }

        private void find_matches(Node node, string regex, List<Node> matches)
        {
            if (node != null)
            {
                find_matches(node.left, regex, matches);
                if (Regex.IsMatch(new string(node.data.word), regex))
                {
                    matches.Add(node);
                }
                find_matches(node.right, regex, matches);
            }
        }

        public void find_matches(string regex, List<Node> matches)
        {
            find_matches(root, regex, matches);
        }
    }
}
