package com.example.pcengine;

import javax.swing.JFrame;

public class Window extends JFrame {
    public Window(int width, int height, int numBuffers){
        setSize(width, height);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

        // Vamos a usar renderizado activo. No queremos que Swing llame al
        // método repaint() porque el repintado es continuo en cualquier caso.
        setIgnoreRepaint(true);

        // Hacemos visible la ventana.
        setVisible(true);

        // Intentamos crear el buffer strategy con 2 buffers.
        int intentos = 100;
        while(intentos-- > 0) {
            try {
                createBufferStrategy(numBuffers);
                break;
            }
            catch(Exception e) {
            }
        } // while pidiendo la creación de la buffeStrategy
        if (intentos == 0) {
            System.err.println("Couldn't create BufferStrategy");
            return;
        }
    }
}
