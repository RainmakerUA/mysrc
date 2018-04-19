using System;

namespace RM.Fun.Loop.Lib
{
    public class Game
    {
	    private readonly IFieldVisualizer _visualizer;
	    private readonly Field _field;

	    public Game(int width, int height, IFieldInitializer initializer, IFieldVisualizer visualizer)
	    {
		    _field = new Field(width, height, initializer);
		    _visualizer = visualizer ?? throw new ArgumentNullException(nameof(visualizer));
	    }


	    public void DrawField()
	    {
			_field.Initialize();

		    for (int horizontalIndex = 0; horizontalIndex < _field.Width; horizontalIndex++)
		    {
				for (int verticalIndex = 0; verticalIndex < _field.Height; verticalIndex++)
				{
					_visualizer.VisualizeCell(_field, horizontalIndex, verticalIndex, false);
				}
			}
	    }
    }
}
