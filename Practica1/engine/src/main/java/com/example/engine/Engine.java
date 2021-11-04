package com.example.engine;

public interface Engine {
    abstract Graphics getGraphics();

    abstract Input getInput();

    abstract void setApplication(Application a);

    abstract void run();
}