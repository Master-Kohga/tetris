using System;
using System.Collections.Generic;
using tetromino;

namespace screen {
    class Screen {
	int[,] environment;
	int[] movement;
	int interval;
	int index;
	List<int> down;
	bool rotate;
	public bool end;
	Tetromino current_t;
	Random rnd;

	public Screen(int interval_) {
	    environment = new int[20, 10];
	    for (int i = 0; i < environment.GetLength(0); i++) {
		for (int j = 0; j < environment.GetLength(1); j++) {
		    environment[i, j] = 0;
		}
	    }

	    end = false;
	    interval = interval_;
	    index = 0;
	    down = new List<int>();
	    movement = new int[] {0, 0};
	    rotate = false;

	    rnd = new Random();
	    current_t = new Tetromino(rnd.Next(0, 7));
	}


	public void Input(ConsoleKeyInfo input) {
	    switch (input.Key) {
		case ConsoleKey.LeftArrow:
		    movement[1]--;
		    break;
		case ConsoleKey.RightArrow:
		    movement[1]++;
		    break;
		case ConsoleKey.UpArrow:
		    rotate = true;
		    break;
	        case ConsoleKey.DownArrow:
		    movement[0]++;
		    break;
	    }
	}

	public void Update() {
	    index++;
	    bool move_down = false;
	    Tetromino previous_t = current_t.Copy();

	    bool rotated = false;
	    if (rotate) {
		current_t.Rotate();
		rotate = false;
		rotated = true;
	    }

	    if (index % interval == 0) {
		movement[0] += 1;
		move_down = true;
	    }

	    if (movement[1] > 1) {
		movement[1] = 1;
	    }
	    
	    this.ScanForRemoval(move_down);
	    current_t.Move(movement);

	    for (int i = 0; i < current_t.current.GetLength(0); i++) {
		for (int j = 0; j < current_t.current.GetLength(1); j++) {
		    int check_h = i + current_t.position[0];
		    int check_w = j + current_t.position[1];
		    
		    if (current_t.current[i, j] == 1 &&
			(check_h >= environment.GetLength(0)
			 || check_h < 0)) {
			this.Plaster(environment, previous_t.current, previous_t.position);
			current_t = new Tetromino(rnd.Next(0, 7));
			return;
		    }
		    else if (current_t.current[i, j] == 1 &&
			     (check_w >= environment.GetLength(1)
			      || check_w < 0)) {
			current_t = previous_t.Copy();
			return;
		    }
		    else if (current_t.current[i, j] == 1) {
			if (environment[check_h, check_w] == 1) {
			    if (current_t.position[0] == 0 && current_t.position[1] == 0) {
				end = true;
			    }
			    else if (movement[1] == 0 && rotated == false) {
				current_t = new Tetromino(rnd.Next(0, 7));
				this.Plaster(environment, previous_t.current, previous_t.position);
			    }
			    else {
				current_t = previous_t.Copy();
			    }
			    return;
			}
		    }
		}
	    }
	}

	public void ResetMovement() {
	    movement = new int[] {0, 0};
	}
	
	public string Print() {
	    string str = "";
	    int[,] environment_str = new int[environment.GetLength(0), environment.GetLength(1)];
	    
	    for (int i = 0; i < environment.GetLength(0); i++) {
		for (int j = 0; j < environment.GetLength(1); j++) {
		    environment_str[i, j] = environment[i, j];
		}
	    }
	    
	    this.Plaster(environment_str, current_t.current, current_t.position);
	    
	    for (int i = 0; i < environment_str.GetLength(0); i++) {
		str += "<!";
		for (int j = 0; j < environment_str.GetLength(1); j++) {
		    if (environment_str[i, j] == 0) {
			str += " .";
		    }
		    else {
			str += "[]";
		    }
		}
		str += "!>\n";
	    }
	    
	    str += "<!";
	    for (int i = 0; i < environment_str.GetLength(1); i++) { str += "=="; }
	    str += "!>\n";

	    return str;
	}

	private int[,] Plaster(int[,] large_array, int[,] array, int[] position) {
	    for (int i = 0; i < array.GetLength(0); i++) {
		for (int j = 0; j < array.GetLength(1); j++) {
		    if (array[i, j] != 0) {
			if (i + position[0] >= 0
			    && i + position[0] <= environment.GetLength(0)
			    && j + position[1] >= 0
			    && j + position[1] <= environment.GetLength(1)) {
			    large_array[i + position[0], j + position[1]] = array[i, j];

			}
		    }
		}
	    }

	    return large_array;
	}

	private void ScanForRemoval(bool move_down = false) {
	    int fillindex;
	    for (int i = 0; i < environment.GetLength(0); i++) {
		fillindex = 0;
		for (int j = 0; j < environment.GetLength(1); j++) {
		    if (environment[i, j] == 1) {
			fillindex++;
		    }
		}

		if (fillindex == environment.GetLength(1)) {
		    for (int j = 0; j < environment.GetLength(1); j++) {
			environment[i, j] = 0;
		    }
		    down.Add(i);
		}
		else if (down.Contains(i) && move_down) {
		    for (int id = i - 1; id >= 0; id--) {
			for (int j = 0; j < environment.GetLength(1); j++) {
			    environment[id + 1, j] = environment[id, j];
			}
		    }

		    down.Remove(i);
		}
	    }
	    
	}

    }
}
