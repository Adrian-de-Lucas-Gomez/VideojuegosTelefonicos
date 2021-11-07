package com.example.pcengine;

import java.awt.event.ComponentEvent;
import java.awt.event.ComponentListener;

import javax.swing.JFrame;
import javax.swing.JLabel;

public class Window extends JFrame implements ComponentListener {

    int _width;
    int _height;
    JLabel label;

    public Window(String title, int width, int height, int numBuffers){
        super(title);

        _width = width;
        _height = height;

        setSize(_width, _height);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        label = new JLabel();
        getContentPane().add(label);
        getContentPane().addComponentListener(this);

        // Vamos a usar renderizado activo. No queremos que Swing llame al
        // método repaint() porque el repintado es continuo en cualquier caso.
        setIgnoreRepaint(true);

        // Hacemos visible la ventana.
        setVisible(true);

        // Intentamos crear el buffer strategy con N buffers.
        int intentos = 100;
        while(intentos-- > 0) {
            try {
                createBufferStrategy(numBuffers);
                break;
            }
            catch(Exception e) {
                System.err.println("Peté");
            }
        } // while pidiendo la creación de la buffeStrategy
        if (intentos == 0) {
            System.err.println("Couldn't create BufferStrategy");
        }
    }

    @Override
    public int getWidth(){
        return _width;
    }
    @Override
    public int getHeight(){
        return _height;
    }

    @Override
    public void componentResized(ComponentEvent componentEvent) {
        _height = this.getHeight();
        _width = this.getWidth();
        //System.out.println("Tamaño cambiado a: " + _width + "x" + _height);
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
