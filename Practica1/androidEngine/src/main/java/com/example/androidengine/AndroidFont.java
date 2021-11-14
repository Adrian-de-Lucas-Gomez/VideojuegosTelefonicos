package com.example.androidengine;

import android.graphics.Typeface;
import com.example.engine.Font;

public class AndroidFont implements Font {

    private Typeface font;  //Variable para fuentes en Android
    private float size;     //Guardamos el tamaño del que queremos la letra(Para asemejarlo a PC)
    private boolean isBold; //Guardamos si se debe pintar o no en negrita

    public AndroidFont(android.content.res.AssetManager manager, String name, float sz, boolean bold){
        //Conseguimos cargar la fuente ttf usando el assetManager que nos otorga el contaxto de la App
        font = Typeface.createFromAsset(manager, "fonts/"+name+".ttf");
        size = sz;
        isBold = bold;
    }

    //Getters para acceder a las variables de la fuente para su pintado en pantalla
    public Typeface getFont(){
        return font;
    }
    public float getSize(){
        return size;
    }
    public boolean isBold(){
        return isBold;
    }
}
