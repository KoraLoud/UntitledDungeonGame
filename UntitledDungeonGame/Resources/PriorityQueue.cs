﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntitledDungeonGame.Resources
{
    public class PriorityQueue<T>
    {
        private List<Tuple<T, double>> elements = new List<Tuple<T, double>>();

        public int Count
        {
            get { return elements.Count; }
        }

        //enqueue
        public void Enqueue(T element, double priority)
        {
            elements.Add(Tuple.Create(element, priority));
        }

        //dequeue
        public T Dequeue()
        {
            int bestIndex = 0;

            for (int i = 0; i < Count; i++)
            {
                if (elements[i].Item2 < elements[bestIndex].Item2)
                {
                    bestIndex = i;
                }
            }

            T bestItem = elements[bestIndex].Item1;
            elements.RemoveAt(bestIndex);
            return bestItem;
        }

    }
}
