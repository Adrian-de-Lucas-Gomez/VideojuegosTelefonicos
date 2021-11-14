package com.example.engine;

public interface Engine {

    Graphics getGraphics();
    Input getInput();
    void setApplication(Application a);
    void run();
}