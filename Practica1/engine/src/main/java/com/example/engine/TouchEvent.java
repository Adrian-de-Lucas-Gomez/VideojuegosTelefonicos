package com.example.engine;

public class TouchEvent {

    public enum EventType {buttonPressed, buttonReleased, pointerMoved};
    public TouchEvent(int x, int y, EventType type) {posX = x; posY = y; eventType = type; }
    public int posX;
    public int posY;
    public EventType eventType;
}
