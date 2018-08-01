using System;
using System.Collections.Generic;
using System.Text;

namespace RM.Fun.Loop.Lib
{
    public interface IFieldVisualizer
    {
	    void VisualizeCell(Field field, int i, int j, bool applyRotation);
    }
}
