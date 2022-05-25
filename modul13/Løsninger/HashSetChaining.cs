using Hashing;

/**
 * This class implements a hash set using separate chaining.
 */
public class HashSetChaining : HashSet {
    private Node[] buckets;
    private int currentSize;

    private class Node {
        public Node(Object data, Node next) {
            this.Data = data;
            this.Next = next;
        }
        public Object Data { get; set; }
        public Node Next{ get; set; }
    }

    /**
     * Constructs a hash table.
     * @param bucketsLength the length of the buckets array
     */
    public HashSetChaining(int size) {
        buckets = new Node[size];
        currentSize = 0;
    }

    /**
     * Tests for set membership.
     * @param x an object
     * @return true if x is an element of this set
     */
    public bool Contains(Object x) {
        int h = hashValue(x);
        Node bucket = buckets[h];
        bool found = false;
        while (!found && bucket != null) {
            if (bucket.Data.Equals(x)) {
                found = true;
            } else {
                bucket = bucket.Next;
            }
        }
        return found;
    }

    /**
     * Adds an element to this set.
     * @param x an object
     * @return true if x is a new object, false if x was already in the set
     */
    public bool Add(Object x) {	  
        int h = hashValue(x);
        
        Node bucket = buckets[h];
        bool found = false;
        while (!found && bucket != null) {
            if (bucket.Data.Equals(x)) {
                found = true;
            } else {
                bucket = bucket.Next;
            }
        }
        
        if (!found) { 
            Node newNode = new Node(x, buckets[h]);
            buckets[h] = newNode;
            currentSize++;
        }

        return !found;
    }

    /**
     * Removes an object from this set.
     * @param x an object
     * @return true if x was removed from this set, false if x was not an element of this set
     */
    public bool Remove(Object x) {
        	    int h = hashValue(x);

        Node current = buckets[h];
        Node previous = null!;
        bool found = false;
        while (!found && current != null) {
            if (current.Data.Equals(x)) {
                found = true;
            } else {
                previous = current;
                current = current.Next;
            }
        }
        
        if (found) {
            if (previous != null) {
                previous.Next = current.Next;
            } else {
                buckets[h] = buckets[h].Next;
            }
            currentSize--;
        }
        
		return found;
    }

    private int hashValue(Object x) {
        int h = x.GetHashCode();
        if (h < 0) {
            h = -h;
        }
        h = h % buckets.Length;
        return h;
    }

    /**
     * Gets the number of elements in this set.
     * @return the number of elements
     */
    public int Size() {
        return currentSize;
    }

    public override String ToString() {
        String result = "";
        for (int i = 0; i < buckets.Length; i++) {
            Node temp = buckets[i];
            if (temp != null) {
                result += i + "\t";
                while (temp != null) {
                    result += temp.Data + " (h:" + hashValue(temp.Data) + ")\t";
                    temp = temp.Next;
                }
                result += "\n";
            }
        }
        return result;
    }
}
