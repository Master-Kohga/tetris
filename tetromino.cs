using System;
using System.IO;

using System.Numerics;

namespace tetromino {
    
    class Tetromino {
	public int[,] current;
	public int[] position;

	public Tetromino(int index) {
	    string[] file = System.IO.File.ReadAllLines(@"tetrominoes/" + index);

	    current = new int[file.Length, file[0].Length];
	    for (int i = 0; i < current.GetLength(0); i++) {
		for (int j = 0; j < current.GetLength(1); j++) {
		    current[i, j] = Convert.ToInt32(file[i][j].ToString());
		}
	    }

	    Console.WriteLine("H");

	    position = new int[] {0, 0};
	}

	public Tetromino(int[] position_, int[,] current_) {
	    current = ReplicateArray(current_);
	    position = new int[] {position_[0], position_[1]};
	}

	private static int[,] ReplicateArray(int[,] array) {
	    int[,] array_new = new int[array.GetLength(0), array.GetLength(1)];
	    for (int i = 0; i < array.GetLength(0); i++) {
		for (int j = 0; j < array.GetLength(1); j++) {
		    array_new[i, j] = array[i, j];
		}
	    }

	    return array_new;
	}

	private void Rotate(int index) {
	    int[,] copy = ReplicateArray(current);
	    for (int i = current.GetLength(0) - 1; i >= 0; i--) {
		for (int j = 0; j < current.GetLength(1); j++) {
		    current[i, j] = copy[(current.GetLength(0) - i) - 1, j];
		}
	    }

	    copy = ReplicateArray(current);
	    for (int i = 0; i < current.GetLength(0); i++) {
		for (int j = 0; j < current.GetLength(1); j++) {
		    current[i, j] = copy[j, i];
		}
	    }

	}

	public void Rotate() {
	    Rotate(1);
	}

	public void Move(int[] offset) {
	    position[0] += offset[0];
	    position[1] += offset[1];
	}

	public override string ToString() {
	    string str = "";
	    for (int i = 0; i < current.GetLength(0); i++) {
		for (int j = 0; j < current.GetLength(1); j++) {
		    str += current[i, j].ToString();
		}
		str += "\n";
	    }

	    return str;
	}

	public Tetromino Copy() {
	    return new Tetromino(position, current);
	}
	
    }
}
