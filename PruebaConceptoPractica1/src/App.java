import javax.swing.JFrame;
import javax.swing.JButton;

import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics;

import java.io.InputStream;
import java.util.Scanner;
import java.io.FileInputStream;


public class App extends JFrame{

	public static void main(String[] args) {
        Board board = new Board(9);
        Scanner sc = new Scanner(System.in);
        board.paint();
        System.out.println("S = Resolver tablero. D = Jugar");
        char c = sc.next().charAt(0);
        if(c == 's'){
            System.out.println("Resolviendo el tablero...");
            board.paint();
            if(board.solveBoard()) System.out.println("Tablero resuelto!.");
            else System.out.println("No se puede resolver el tablero :(.");
            board.paint();
        }
        else if(c == 'd'){
            board.paint();
            while(true){
                c = sc.next().charAt(0);
                System.out.print("\033[H\033[2J");
                System.out.flush();
                board.handleInput(c);
                board.update();
                board.paint();
            }
        }
    }
}