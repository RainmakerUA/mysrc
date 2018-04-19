using System;
using System.Collections.Generic;
using System.Text;

namespace RM.Fun.Loop.Lib
{
    public interface IFieldInitializer
    {
	    FieldSides InitializeCell(int horizontalIndex, int verticalIndex, Field field);
    }
}
