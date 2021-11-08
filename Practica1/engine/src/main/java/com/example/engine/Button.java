package com.example.engine;

public class Button {
    private Image _img = null;
    private Font _font = null; //No se xd
    private String _text; //No se xd
    private int _x1, _x2, _y1, _y2;

    public Button(int posX1, int posY1, int posX2, int posY2, Image img){
        _img = img;
        _x1 = posX1;
        _x2 = posX2;
        _y1 = posY1;
        _y2 = posY2;
    }

    public Button(int posX1, int posY1, int posX2, int posY2, Font font, String text) { //No se xd
        _font = font;
        _text = text;
        _x1 = posX1;
        _x2 = posX2;
        _y1 = posY1;
        _y2 = posY2;
    }

    public boolean isPressed(int x, int y){
        return(_x1 <= x && _y1 <= y && _x2 >= x && _y2 >= y);
    }

    public int getX1(){ return _x1; }

    public int getX2(){ return _x2; }

    public int getY1(){ return _y1; }

    public int getY2(){ return _y2; }

    public Image getImage() {return _img;}

    public Font getText() {return _font;} //No se xd
}
