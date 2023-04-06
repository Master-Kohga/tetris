using System;
using screen;

namespace main {
    class Program {
	static void Main() {
	    Console.ForegroundColor = ConsoleColor.Green;
	    Console.CursorVisible = false;
	    Console.Clear();
	    
	    Screen screen = new Screen(500);
	    ConsoleKeyInfo input;

	    while (screen.end == false) {
		if (Console.KeyAvailable) {
		    input = Console.ReadKey(false);
		    screen.Input(input);
		}

		screen.Update();
		screen.ResetMovement();

		Console.SetCursorPosition(0, 0);
		Console.Write(screen.Print());
	    }
	}
    }
}
