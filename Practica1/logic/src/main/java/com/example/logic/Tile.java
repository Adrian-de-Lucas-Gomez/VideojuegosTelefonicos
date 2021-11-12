package com.example.logic;
public class Tile implements Comparable<Tile>{

    public Tile(int x, int y, int value, TileType type){
        _x = x;
        _y = y;
        _value = value;
        _type = type;
    }

    public Tile(Tile tile){
        _x = tile.getX();
        _y = tile.getY();
        _value = tile.getValue();
        _type = tile.getType();
    }

    public void copyFromTile(Tile tile){
        _x = tile.getX();
        _y = tile.getY();
        _value = tile.getValue();
        _type = tile.getType();
    }

    @Override
    public int compareTo(Tile o) {
        return Integer.compare(o.getValue(), this._value);
    }


    public void toggleType(){ //Cambia entre wall y dot. Devuelve el cambio en el número de tiles ocupadas del tablero
        //Dot -> Wall -> Unknown
        if(_type == TileType.Wall) _type = TileType.Unknown;
        else if(_type == TileType.Unknown) _type = TileType.Dot;
        else if(_type == TileType.Dot) _type = TileType.Wall;
    }

    public void toggleType_i(){ //Usado para revertir jugadas
        if(_type == TileType.Dot) _type = TileType.Unknown;
        else if(_type == TileType.Unknown) _type = TileType.Wall;
        else if(_type == TileType.Wall) _type = TileType.Dot;
    }

    //Getters & Setters
    public void setType(TileType type) {_type = type;}

    public void setValue(int value) {_value = value;}

    public int getX(){ return _x; }

    public int getY(){ return _y; }

    public void setX(int x) {_x = x;}

    public void setY(int y) {_y = y;}

    public TileType getType(){
        return _type;
    }

    public int getValue(){
        return _value;
    }

    private TileType _type = TileType.Value;
    private int _x = 0; //Revisar si hace falta la posición. Por lógica no debería. En un futuro podría representar la posición en pantalla pero no estoy seguro.
    private int _y = 0;
    private int _value = 0;
}
