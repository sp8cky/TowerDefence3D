using System;
using System.Collections.Generic;

public class PriorityQueue<T> // TODO: fix
{
    private List<T> data;
    private Dictionary<T, int> itemIndices;  // New dictionary to store item to index mapping
    private IComparer<T> comparer;

    public PriorityQueue(IComparer<T> comparer = null)
    {
        this.data = new List<T>();
        this.itemIndices = new Dictionary<T, int>();  // Initialize the dictionary
        this.comparer = comparer ?? Comparer<T>.Default;
    }

    public int Count => data.Count;

    public void Enqueue(T item)
    {
        data.Add(item);
        itemIndices[item] = data.Count - 1;  // Store the index of the item
        HeapifyUp(data.Count - 1);
    }

    private void HeapifyUp(int childIndex)
    {
        while (childIndex > 0)
        {
            int parentIndex = (childIndex - 1) / 2;
            if (comparer.Compare(data[childIndex], data[parentIndex]) >= 0)
                break;
            Swap(childIndex, parentIndex);
            childIndex = parentIndex;
        }
    }

    public T Dequeue()
    {
        if (data.Count == 0)
            throw new InvalidOperationException("Priority queue is empty");
        T frontItem = data[0];
        int lastIndex = data.Count - 1;
        data[0] = data[lastIndex];
        data.RemoveAt(lastIndex);
        itemIndices.Remove(frontItem);  // Remove item from dictionary
        if (data.Count > 0)
        {
            itemIndices[data[0]] = 0;  // Update index of the new root item
            HeapifyDown(0);
        }
        return frontItem;
    }

    private void HeapifyDown(int parentIndex)
    {
        while (true)
        {
            int leftChildIndex = parentIndex * 2 + 1;
            if (leftChildIndex >= data.Count)
                break;
            int rightChildIndex = leftChildIndex + 1;
            int minIndex = leftChildIndex;
            if (rightChildIndex < data.Count && comparer.Compare(data[rightChildIndex], data[leftChildIndex]) < 0)
                minIndex = rightChildIndex;
            if (comparer.Compare(data[parentIndex], data[minIndex]) <= 0)
                break;
            Swap(parentIndex, minIndex);
            parentIndex = minIndex;
        }
    }

    public T Peek()
    {
        if (data.Count == 0)
            throw new InvalidOperationException("Priority queue is empty");
        return data[0];
    }

    private void Swap(int index1, int index2)
    {
        T temp = data[index1];
        data[index1] = data[index2];
        data[index2] = temp;
        // Update indices in the dictionary after swapping
        itemIndices[data[index1]] = index1;
        itemIndices[data[index2]] = index2;
    }

    public bool Contains(T item)
    {
        return itemIndices.ContainsKey(item);  // Check if item exists in dictionary
    }

    public void UpdatePriority(T item)
    {
        if (!itemIndices.ContainsKey(item))
        {
            throw new ArgumentException("Item not found in priority queue");
        }
        int index = itemIndices[item];
        HeapifyUp(index);  // Perform heapify up operation from the current index
        HeapifyDown(index);  // Perform heapify down operation from the current index
    }
}
