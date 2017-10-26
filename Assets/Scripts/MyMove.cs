using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMove {
    public MyMove() { }

    public static implicit operator bool(MyMove v)
    {
        if(v == null)
        {
            return false;
        }
        return true;
    }
}


public sealed class Boost : MyMove
{
    public Boost() { }

    
}