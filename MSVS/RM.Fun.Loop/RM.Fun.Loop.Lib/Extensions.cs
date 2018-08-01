
namespace RM.Fun.Loop.Lib
{
    public static class Extensions
    {
	    public static FieldSides Sides(this ICell cell)
	    {
		    return (FieldSides)cell.Value;
	    }

	    public static bool HasSide(this ICell cell, FieldSides side)
	    {
			// TODO: test `side` to be a single side.
		    return (Sides(cell) & side) == side;
	    }
    }
}
