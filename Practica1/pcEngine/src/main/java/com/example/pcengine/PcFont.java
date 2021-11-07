package com.example.pcengine;

import com.example.engine.Font;

import javax.swing.text.Style;

public class PcFont implements Font {
    private java.awt.Font _font;

    public PcFont(String type, int style, int size){
        //Todavia sin saber que hace
        _font = new java.awt.Font(type, style, size);
    }

    @Override
    public void test() {

    }
}
