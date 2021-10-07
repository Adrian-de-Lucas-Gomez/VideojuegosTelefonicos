package com.example.practica1;

public interface Graphics {
    public Image newImage(String name);
    public Font newFont (String filename, int size, boolean isBold);
    public void clear (int color);
}
