package com.example.pcengine;

import java.awt.event.ComponentEvent;
import java.awt.event.ComponentListener;

import javax.swing.JFrame;

public class Window extends JFrame implements ComponentListener {
    public Window(String title, int width, int height, int numBuffers){
        super(title);

        setSize(width, height);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        getContentPane().addComponentListener(this);

        //FullScreen--------------------------------------------
        //setExtendedState(JFrame.MAXIMIZED_BOTH);
        //setUndecorated(true);
        //------------------------------------------------------

        // Vamos a usar renderizado activo. No queremos que Swing llame al
        // método repaint() porque el repintado es continuo en cualquier caso.
        setIgnoreRepaint(true);

        // Hacemos visible la ventana.
        setVisible(true);

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
        //pack();
    }

    @Override
    public void componentResized(ComponentEvent componentEvent) {
        //setBounds(0, 0, getWidth(), getHeight());
        //this.setSize(getWidth(), getHeight());

        //System.out.println("Tamaño cambiado a: " + getWidth() + "x" + getHeight());
    }
    @Override
    public void componentMoved(ComponentEvent componentEvent) {

    }
    @Override
    public void componentShown(ComponentEvent componentEvent) {
    }
    @Override
    public void componentHidden(ComponentEvent componentEvent) {
    }


}
