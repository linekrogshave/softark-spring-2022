namespace LinkedList
{
    class Node
    {
        public Node(User data, Node next)
        {
            this.Data = data;
            this.Next = next;
        }
        public User Data;
        public Node Next;
    }

    class UserLinkedList
    {
        private Node first = null!;

        public void AddFirst(User user)
        {
            Node node = new Node(user, first);
            first = node;
        }

        public User RemoveFirst()
        {
            Node node = first;
            first = first.Next;

            return node.Data;
        }

        public void RemoveUser(User user)
        {
            Node node = first;
            Node previous = null!;
            bool found = false;

            while (!found && node != null)
            {
                if (node.Data.Name == user.Name)
                {
                    found = true;
                    if (node == first)
                    {
                        RemoveFirst();
                    }
                    else
                    {
                        previous.Next = node.Next;
                    }
                }
                else
                {
                    previous = node;
                    node = node.Next;
                }
            }
        }

        public User GetFirst()
        {
            return first.Data;
        }

        public User GetLast()
        {
            Node node = first;
            bool found = false;

            while(!found)
            {
                if(node.Next == null)
                {
                    found = true;
                    node = node.Next;
                }
            }
            return node.Data;
        }

        public int CountUsers()
        {
            Node node = first;
            int count = 0;

            while (node != null)
            {
                count ++;
                node = node.Next;
            }
            return count;
        }

         public bool Contains(User user)
        {
            Node node = first;

            while(true)
            {
                if(node.Data.Equals(user)) 
                {
                    return true;
                }
                if(node.Next == null)
                {
                    return false;
                }
                else
                {
                    node = node.Next;
                }
            }
        }

        public override String ToString()
        {
            Node node = first;
            String result = "";
            while (node != null)
            {
                result += node.Data.Name + ", ";
                node = node.Next;
            }
            return result.Trim();
        }
    }
}