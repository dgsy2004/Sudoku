using Godot;
using System;

public partial class ThreeByThree : Sprite2D
{
	[Export] SudokuCell[] cells;




	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		for (int i=0; i<cells.Length; i++)
		{
			cells[i].Clear();
		}
	}

	public SudokuCell CheckIfCellHaveMouseIn()
	{
		for (int i = 0; i < cells.Length; i++)
		{
			if(cells[i].isMouseInCell)
			{

				return cells[i];
			}

		}
		return null;
	}

	public SudokuCell GetCellByIndex(int index)
	{
		if(index < 0 || index >= cells.Length) return null;

		return cells[index];
	}

	public int GetCellsCount()
	{
		return cells.Length;
	}
}