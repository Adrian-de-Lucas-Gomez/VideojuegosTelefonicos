package com.example.pcengine;

import com.example.engine.Font;

import java.awt.FontFormatException;
import java.io.File;
import java.io.IOException;

import javax.swing.text.Style;

public class PcFont implements Font {
    private java.awt.Font _font;    //Almacenamos el tipo de fuente que soporta la plataforma

    public PcFont(String name, int style, float size){
        //Todavia sin saber que hace
        //_font = new java.awt.Font(type, style, size);
        try {
            _font = java.awt.Font.createFont(java.awt.Font.TRUETYPE_FONT, new File("assets\\fonts\\"+name+".ttf")).deriveFont(style,size);
            System.out.println("Me cree correctamente");
        } catch (FontFormatException e) {
            System.out.println("Peto en el formato");
        } catch (IOException e) {
            System.out.println("Peto en entrada/salida");
        }
    }
    //Propio de cada font ya que se usa para escribir el formato de la plataforma
    public java.awt.Font getFont(){
        return _font;
    }
    @Override
    public void test() {

    }
}

