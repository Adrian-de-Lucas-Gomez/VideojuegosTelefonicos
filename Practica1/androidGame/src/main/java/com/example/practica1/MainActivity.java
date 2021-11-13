package com.example.practica1;

import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AppCompatActivity;

//import android.app.Application;
import android.content.Context;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.os.Build;
import android.os.Bundle;
import android.view.View;

import com.example.androidengine.AndroidEngine;
import com.example.engine.Engine;
import com.example.engine.Application;
import com.example.logic.Logic;

public class MainActivity extends AppCompatActivity {
    AndroidEngine engine;
    Logic logic;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        engine =  new AndroidEngine(this, 400, 600);    //Hace que pete, ni idea tho
        com.example.engine.Application logic = new Logic(engine);
        logic.init();
        engine.setApplication(logic);

        setContentView(engine.getSurfaceView());
    }


    protected void onResume() {
        super.onResume();
        //Volver a lanzar la hebra
        engine.resume();
    }


    protected void onPause() {
        super.onPause();
        //Parar la hebra
        engine.pause();
    }
}