using MineSweeper.Game;

/*

    This is a game of MineSweeper within the console.
    There are no graphics and the logic is quite simple.
    Game allows marking bombs on the board and checking if you have won or lost.
    Each new game is created with randomly generated bombs.
    
*/
string? player = "Brett";
int[,] board = Game.CreateReferenceBoard();
Game game = new Game("Brett", board);

Game.EncodeNumbers(board);
char[,] playBoard = Game.PlayBoard();
Game.PlayGame(board, playBoard, player);


