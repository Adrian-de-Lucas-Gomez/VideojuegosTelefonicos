package com.example.pcengine;

import javax.swing.JFrame;

public class Window extends JFrame {
    public Window(String title, int width, int height, int numBuffers){
        super(title);

        setSize(width, height);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

        //FullScreen--------------------------------------------
        //setExtendedState(JFrame.MAXIMIZED_BOTH);
        //setUndecorated(true);
        //------------------------------------------------------

        // Vamos a usar renderizado activo. No queremos que Swing llame al
        // método repaint() porque el repintado es continuo en cualquier caso.
        setIgnoreRepaint(true);

        setVisible(true); // Hacemos visible la ventana

        // Buffer strategy con N buffers.
        int intentos = 100;
        while(intentos-- > 0) {
            try {
                createBufferStrategy(numBuffers);
                break;
            }
            catch(Exception e) {
                System.err.println("Peté");
            }
        }
        if (intentos == 0) {
            System.err.println("Couldn't create BufferStrategy");
        }
    }
}
