package com.example.engine;

import java.awt.Color;

public interface Graphics {
    abstract Image newImage(String name);
    abstract Font newFont(String filename, int size, boolean isBold);
    abstract void clear(int color);
    abstract void translate(int x, int y);
    abstract void scale(int x, int y);
    abstract void save();
    abstract void restore();
    abstract void drawImage(Image image /*mas parametros?*/);
    abstract void setColor(Color color);
    abstract void fillCircle(int cx, int cy, int r);
    abstract void drawText(String text, int x, int y);
    abstract int getWidth();
    abstract int getHeight();
}
