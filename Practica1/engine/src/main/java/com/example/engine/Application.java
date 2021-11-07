package com.example.engine;

public interface Application {
    void init();
    void onDestroy();
    void onEvent(TouchEvent event);
    void onUpdate(double deltaTime);
    void onRender(Graphics graphics);
}
