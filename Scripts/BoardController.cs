using Godot;
using System;
using System.Collections.Generic;
public partial class BoardController : Node2D
{
	[Export] int boardSize = 3;
	[Export] PackedScene threeByThreeScene = null;
	[Export] float cellFillChance = 0.5f;
	private ThreeByThree[] gameBoard;
	private int currentButtonNumber = 1;
	private int[,] gameNumbers;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameBoard = new ThreeByThree[boardSize*boardSize];
		gameNumbers = new int[boardSize*boardSize, boardSize*boardSize];

		CreateGameBoard();

		for (int i = 0; i < gameBoard.Length; i++)
		{
			PopulateGameBoardWithInitialValues(i);
			
		}

		FillBoardWithAssignedNumbers();
		PrintGameBoard();

	}

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseButton mouseButton) 
		{
			if(!mouseButton.Pressed || mouseButton.ButtonIndex != MouseButton.Left) return;

			for (int i = 0; i < gameBoard.Length; i++)
			{
				SudokuCell cell=gameBoard[i].CheckIfCellHaveMouseIn();

				if(cell==null) continue;
				if(cell.hasDefaultValue) break;

				cell.Clear();
				cell.AppendText($"[center][color=yellow]{currentButtonNumber}[/color][/center]");
			}
			
		}
    }
	public void SetSelectedCellContentNumber(int buttonNumber)
	{
		currentButtonNumber = buttonNumber;

	}

	private void  CreateGameBoard()
	{
		int boardChunksIndex = 0;

		for (int x = 0; x < boardSize; x++)
		{
			for (int y = 0; y < boardSize; y++)
			{
				ThreeByThree newThreeByThree = threeByThreeScene.Instantiate<ThreeByThree>();

				AddChild(newThreeByThree);
				newThreeByThree.Name = "BoardChunk" + boardChunksIndex;
				Texture2D texture2D = newThreeByThree.Texture;

				float halfX = texture2D.GetSize().X * 0.5f;
				float halfY = texture2D.GetSize().Y * 0.5f;


				Vector2 newPosition = new Vector2((float)(texture2D.GetSize().X * x) + halfX, (float)(texture2D.GetSize().Y * y)+ halfY);

				
				newThreeByThree.Position = newPosition;
				gameBoard[boardChunksIndex] = newThreeByThree;
				boardChunksIndex++;
			}
		}
	}	
	private void PopulateGameBoardWithInitialValues(int index)
	{
		List<int> numbers = new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9};

		for (int x = 0; x < 3; x++)
		{
				for (int y = 0; y < 3; y++)
				{
					float randomNumber = GD.Randf();

					int xCoor = x + (index / 3 * 3);
					int yCoor = y + (index % 3 * 3);

					if(randomNumber > cellFillChance) continue;
					
					int randomSudokuNumber = GD.RandRange(0,numbers.Count -1);

					while(CheckIfNumberIsInRow(numbers[randomSudokuNumber],xCoor, yCoor) || 
						  CheckIfNumberIsInColumn(numbers[randomSudokuNumber],xCoor, yCoor))
					{
						//TODO upgrade this

						randomSudokuNumber = GD.RandRange(0,numbers.Count -1);
					}
					
					gameNumbers[xCoor,yCoor] = numbers[randomSudokuNumber];
					numbers.RemoveAt(randomSudokuNumber);
				}
		}
	}
	private void FillBoardWithAssignedNumbers()
	{
		for (int i = 0; i < gameBoard.Length; i++)
		{
			for (int j = 0; j < gameBoard[i].GetCellsCount(); j++)
			{
				int x = j % 3;
				int y = j / 3;

				x += i / 3 * 3;
				y += i % 3 * 3;

				SudokuCell cell= gameBoard[i].GetCellByIndex(j);

				if(gameNumbers [x,y] <= 0) continue;
				cell.Clear();
				cell.AppendText($"[center][color=blue]{gameNumbers[x,y]}[/color][/center]");
				cell.hasDefaultValue = true;
				
			}
		}

	}
	private bool CheckIfNumberIsInRow(int number, int numberX, int numberY)
	{
		bool wasNumberFound = false;
		int y = numberY;
		
		
		for (int x = 0; x < gameBoard.GetLength(0); x++)
		{
			if(x == numberX) continue;

			if(gameNumbers[x,y] == number) wasNumberFound = true;
		}
		return wasNumberFound;
	}
	private bool CheckIfNumberIsInColumn(int number, int numberX, int numberY)
	{
		bool wasNumberFound = false;
		int x = numberX;		
		
		for (int y = 0; y < gameNumbers.GetLength(1); y++)
		{
			if(y == numberY) continue;

			if(gameNumbers[x,y] == number) wasNumberFound = true;
		}
		return wasNumberFound;
	}
	private void PrintGameBoard()
	{
		for (int y = 0; y < gameNumbers.GetLength(1); y++)
		{
			string rowNumbers = "";
			for (int x = 0; x < gameNumbers.GetLength(0); x++)
			{
				rowNumbers += gameNumbers[x, y];
			}
			GD.Print(rowNumbers);
		}
	}
	public void RestartSudoku()
	{	
		gameNumbers = new int[boardSize*boardSize, boardSize*boardSize];
		for (int i = 0; i < gameBoard.Length; i++)
		{
			for (int j = 0; j < gameBoard[i].GetCellsCount(); j++)
			{
								
				int x = j % 3;
				int y = j / 3;

				x += i / 3 * 3;
				y += i % 3 * 3;

				SudokuCell cell= gameBoard[i].GetCellByIndex(j);				
				cell.Clear();
				cell.AppendText($"[center][color=blue]{gameNumbers[x,y]}[/color][/center]");
				cell.hasDefaultValue = true;				
				
			}
		}
	}
}

