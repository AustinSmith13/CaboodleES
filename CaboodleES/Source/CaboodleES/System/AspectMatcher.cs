﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaboodleES.System
{
    internal static class AspectMatcher
    {
        internal static bool Match(Aspect aspect, Utils.BitMask left, Utils.BitMask right)
        {
            switch(aspect)
            {
                case Aspect.Has:
                    return left >= right;

                case Aspect.Match:
                    return left == right;

                case Aspect.Complement:
                    for(int i = 0; i < left.Count && i < right.Count; i++)
                    {
                        if (left.Get(i) != !right.Get(i))
                            return false;
                    }
                    return true;
            }

            return false;
        }
    }
}