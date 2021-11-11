package com.example.logic;

public class Direction {
    public Direction(int x, int y){
        _x = x;
        _y = y;
    }

    public int getX(){
        return _x;
    }

    public int getY(){
        return _y;
    }

    private int _x;
    private int _y;
}