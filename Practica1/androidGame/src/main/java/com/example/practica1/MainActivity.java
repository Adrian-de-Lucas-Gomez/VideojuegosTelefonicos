package com.example.practica1;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;

import com.example.androidengine.AndroidEngine;
import com.example.engine.Engine;
import com.example.logic.Logic;

public class MainActivity extends AppCompatActivity {
    AndroidEngine engine;
    Logic logic;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        engine = new AndroidEngine();
        logic = new Logic();
        setContentView(R.layout.activity_main);
    }

    @Override
    protected void onResume() {
        super.onResume();
        //Volver a lanzar la hebra
    }

    @Override
    protected void onPause() {
        super.onPause();
        //Parar la hebra
    }
}