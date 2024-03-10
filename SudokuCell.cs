using Godot;
using System;

public partial class SudokuCell : RichTextLabel
{
	public bool isMouseInCell {private set; get;} = false;
	public bool hasDefaultValue = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MouseEntered += PlayerMouseEntered;
		MouseExited += PlayerMouseExited;
	}

	private void PlayerMouseEntered ()
	{
		isMouseInCell = true;
	}

	private void PlayerMouseExited()
	{
		isMouseInCell = false;

	}
}