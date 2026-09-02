using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCG_Cross_Rhythm_Proto_4
{
    //personalised queue class
    public class CustomQueue<T> where T : class
    {
        protected List<T> queue;
        protected int pointer;

        public CustomQueue()
        {
            queue = new List<T>();
            pointer = 0;
        }

        //adds to queue
        public virtual void Enqueue(T item)
        {
            queue.Add(item);
        }

        //returns from front of queue
        //the try-catch acts as a 'back pointer' (with default values) without defining a separate back pointer
        public virtual T Dequeue()
        {
            try
            {
                T returnItem = queue[pointer];
                pointer++;

                return returnItem;
            }
            catch (ArgumentOutOfRangeException)
            {
                return default;
            }
        }
    }
}
