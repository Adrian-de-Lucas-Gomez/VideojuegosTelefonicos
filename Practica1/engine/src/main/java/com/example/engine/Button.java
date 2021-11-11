package com.example.engine;

public class Button {
    private Image _img = null;
    private float _x, _y, _height, _width;

    public Button(float posX, float posY, float width, float height, Image img){
        _img = img;
        _x = posX;
        _y = posY;
        _width = width;
        _height = height;
    }

    /*public Button(int posX1, int posY1, int posX2, int posY2, Font font, String text) { //No se xd
        _font = font;
        _text = text;
        _x1 = posX1;
        _x2 = posX2;
        _y1 = posY1;
        _y2 = posY2;
    }*/

    public boolean isPressed(int x, int y){
        return(_x <= x && _y <= y && _x + _width >= x && _y + _height >= y);
    }

    public float getX() { return _x; }

    public float getY() { return _y; }

    public float getWidth() { return _width; }

    public float getHeight() { return _height; }

    public Image getImage() {return _img;}
}
