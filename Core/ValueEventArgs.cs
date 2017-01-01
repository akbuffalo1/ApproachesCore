using System;
using System.Collections.Generic;
using System.Text;

namespace AD
{
    public class ValueEventArgs<T> : EventArgs
    {
        public ValueEventArgs(T value)
        {
            this.Value = value;
        }

        public T Value { get; private set; }
    }
}