package com.example.practica2_moviles_intento3;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;

import com.example.androidengine.AndroidEngine;
import com.example.engine.Engine;

public class MainActivity extends AppCompatActivity {
    AndroidEngine engine;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }
}