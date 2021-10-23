import javax.swing.JFrame;
import javax.swing.JButton;

import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics;

import java.io.InputStream;
import java.util.Scanner;
import java.io.FileInputStream;


enum TileType{
    Unknown, Dot, Wall, Value //Nunca debería ser value. No se cuando lo utiliza
}

enum Directions{
    Left, Right, Up, Down
}


public class App extends JFrame{

	public static void main(String[] args) {
        Board board = new Board(4);
        Scanner sc = new Scanner(System.in);
        System.out.println("Inizialicacion terminada.");

        board.paint();
        while(true){
            char c = sc.next().charAt(0);
            System.out.print("\033[H\033[2J");
            System.out.flush();
            board.update(c);
            board.paint();
        }
    }
}