package com.example.engine;

public interface Application {
    void init();
    void onDestroy();
    void onHandleInput();
    void onUpdate(double deltaTime);
    void onRender(Graphics graphics);
}
