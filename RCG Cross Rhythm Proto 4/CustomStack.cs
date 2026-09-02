using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCG_Cross_Rhythm_Proto_4
{
    //personalised stack class
    public class CustomStack<T> where T : class
    {
        private List<T> stack;
        private int pointer;

        public CustomStack()
        {
            stack = new List<T>();
            pointer = -1;
        }

        //adds to stack
        public void Push(T item)
        {
            stack.Add(default);
            pointer++;
            stack[pointer] = item;
        }

        //returns from top of stack
        //the try-catch acts as a way to show the stack is empty (through default values)
        public T Pop()
        {
            try
            {
                T ret = stack[pointer];
                pointer--;

                return ret;
            }
            catch (ArgumentOutOfRangeException)
            {
                return default;
            }
        }
    }
}
