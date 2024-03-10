using Godot;
using System;

public partial class SudokuButton : Button
{

	[Export] int buttonNumber = 0;
	[Export] BoardController boardController;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += ButtonPressed;
	}

	private void ButtonPressed()
	{
		boardController.SetSelectedCellContentNumber(buttonNumber);

	}
}
