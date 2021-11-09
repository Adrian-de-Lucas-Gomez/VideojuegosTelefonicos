package com.example.practica1;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Context;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.os.Bundle;
import android.view.View;

import com.example.androidengine.AndroidEngine;
import com.example.engine.Engine;
import com.example.logic.Logic;

public class MainActivity extends AppCompatActivity {
    AndroidEngine engine;
    Logic logic;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        //engine =  new AndroidEngine();
        //logic = new Logic(engine);
        pantalla alla = new pantalla(this);
        setContentView(alla);
    }

    class pantalla extends View{
        public pantalla(Context context){
            super(context);
        }

        public void onDraw(Canvas canvas){
            Paint pintura= new Paint();

            pintura.setStyle(Paint.Style.FILL_AND_STROKE);
            pintura.setStrokeWidth(15.0f);
            pintura.setColor(Color.BLUE);
            System.out.println("render");

            canvas.drawCircle(150,150, 50, pintura);
            canvas.drawRect(10,5,500,60,pintura);
        }
    }

/*
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
    */

}