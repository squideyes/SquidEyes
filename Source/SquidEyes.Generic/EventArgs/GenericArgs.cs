﻿using System;

namespace SquidEyes.Generic
{
    public class GenericArgs<T> : EventArgs
    {
        public GenericArgs(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }
    }
}
