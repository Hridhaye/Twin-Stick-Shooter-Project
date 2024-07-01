using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A min heap that provides easy access to the node with the lowest HCost. 
/// The class uses a generic T but relies on the IHeap<T> and IComparable<T> interfaces to ensure that items have 
/// a HeapIndex and an appropriate CompareTo method.
/// </summary>
public class Heap<T> where T : IHeap<T>
{
    T[] items;
    int currentItemCount = 0;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirstItem()
    {
        T itemToBeReturned = items[0];
        items[0] = items[currentItemCount - 1];
        currentItemCount--;
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return itemToBeReturned;
    }

    public void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        //Sort item up if its HCost is smaller than its parent.
        while (true)
        {
            T parentItem = items[parentIndex];

            if (item.CompareTo(parentItem) > 0)
            {
                SwapIndex(item, parentItem);
            }
            else
            {
                break;
            }
        }

        parentIndex = (item.HeapIndex - 1) / 2;
    }

    public void SortDown(T item)
    {
        //Sort item down if its HCost is bigger than one of its children.
        while (true)
        {
            int childIndexLeft = (2 * item.HeapIndex) + 1;
            int childIndexRight = (2 * item.HeapIndex) + 2;
            int swapIndex = 0;

            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount)
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    SwapIndex(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

    }

    public void SwapIndex(T item1, T item2)
    {
        items[item1.HeapIndex] = item2;
        items[item2.HeapIndex] = item1;

        int item1Index = item1.HeapIndex;
        item1.HeapIndex = item2.HeapIndex;
        item2.HeapIndex = item1Index;
    }


    public void Clear()
    {
        currentItemCount = 0;
    }

    public bool Contains(T item)
    {
        if (item.HeapIndex < currentItemCount)
        {
            return Equals(item, items[item.HeapIndex]);
        }
        else
        {
            return false;
        }
    }

    public int Count
    {
        get { return currentItemCount; }
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }


}


public interface IHeap<T> : IComparable<T>
{
    public int HeapIndex { get; set; }
}


