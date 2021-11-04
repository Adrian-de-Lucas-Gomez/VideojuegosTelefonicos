package com.example.engine;

public interface Application {
    abstract void init();
    abstract void onDestroy();
    abstract void onEvent(TouchEvent event);
    abstract void onUpdate(double deltaTime);
    abstract void onRender(Graphics graphics);
}
