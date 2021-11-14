package com.example.practica1;

import androidx.appcompat.app.AppCompatActivity;
import android.os.Bundle;

import com.example.androidengine.AndroidEngine;
import com.example.logic.Logic;

public class MainActivity extends AppCompatActivity {
    //Variable de clase para poder ser accesible desde todos los métodos
    AndroidEngine engine;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        //Creamos el motor con resolicion lógica de 400x600
        engine =  new AndroidEngine(this, 400, 600);
        //Creamos la logica que deriva de la interfaz Application
        com.example.engine.Application logic = new Logic(engine);
        logic.init();
        //Ponemos la referencia de la aplicacion en el motor
        engine.setApplication(logic);
        
        //Indicamos el surface que debe mostrar en pantalla
        setContentView(engine.getSurfaceView());
    }

    //Si habiamos perdido el foco y volvemos al juego se vuelve a lanzar la hebra del bucle principal
    protected void onResume() {
        super.onResume();
        engine.resume(); //Volver a lanzar la hebra

    }

    //Si hemos minimizado la aplicacion o la hemos quitado el foco paramos la ejecicion y la hebra
    protected void onPause() {
        super.onPause();
        engine.pause(); //Parar la hebra
    }
}