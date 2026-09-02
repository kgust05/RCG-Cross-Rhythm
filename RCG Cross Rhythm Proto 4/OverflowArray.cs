using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCG_Cross_Rhythm_Proto_4
{
    //custom struct that acts as a fixed max-length list that kicks out the front item when you try and queue an item from the back
    //due to similarities in how the enqueuing functions, inherits from CustomQueue
    public class OverflowArray<T> : CustomQueue<T> where T : class
    {
        private int backPointer;
        private int size;

        public OverflowArray(int size)
        {
            this.size = size;
            backPointer = 0;
            queue = new List<T>();            
            pointer = 0 - this.size;                     
        }

        //repurposed Dequeue method that returns the most recent item kicked from the front of the list
        public override T Dequeue()
        {
            if (pointer > 0)
            {
                return queue[pointer - 1];
            }
            else
            {
                return default;
            }
        }

        //extra functionality added to Enqueue method to also increment pointers
        public override void Enqueue(T item)
        {
            base.Enqueue(item);
            pointer++;
            backPointer++;
        }

        //returns the list of items that currently lie between the front and back pointers
        //if pointer < 0, the lower bound of what is returned is defaulted to 0
        public List<T> GetList()
        {
            int lowerBound;
            List<T> toReturn = new List<T>();

            if (pointer < 0)
            {
                lowerBound = 0;
            }
            else
            {
                lowerBound = pointer;
            }

            for (int i = lowerBound; i < backPointer; i++)
            {
                toReturn.Add(queue[i]);
            }

            return toReturn;
        }
    }
}
