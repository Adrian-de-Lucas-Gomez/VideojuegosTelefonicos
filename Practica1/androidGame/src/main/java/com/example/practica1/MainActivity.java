package com.example.practica1;

import androidx.appcompat.app.AppCompatActivity;
import android.os.Bundle;

import com.example.androidengine.AndroidEngine;
import com.example.logic.Logic;

public class MainActivity extends AppCompatActivity {
    AndroidEngine engine;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        engine =  new AndroidEngine(this, 400, 600);
        com.example.engine.Application logic = new Logic(engine);
        logic.init();
        engine.setApplication(logic);

        setContentView(engine.getSurfaceView());
    }

    protected void onResume() {
        super.onResume();
        engine.resume(); //Volver a lanzar la hebra

    }

    protected void onPause() {
        super.onPause();
        engine.pause(); //Parar la hebra
    }
}