package com.example.androidengine;

import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Color;
import android.graphics.Paint;
import android.os.Build;

import androidx.annotation.RequiresApi;

import com.example.engine.AbstractGraphics;
import com.example.engine.Font;
import com.example.engine.Image;

//Ahora implementa AbstractGraphics para no repetir codigo

public class AndroidGraphics extends AbstractGraphics {

    private android.graphics.Canvas _graphics;
    private Paint _paint;

    public AndroidGraphics() {
        _paint = new Paint();
        _paint.setColor(Color.WHITE);
        _paint.setStyle(Paint.Style.FILL);

        _graphics.drawPaint(_paint);
    }

    public Image newImage(String name){

        AndroidImage img = null;

        Bitmap bMap = BitmapFactory.decodeFile("./assets/sprites/" + name);

        return new AndroidImage(bMap);
    }
    public AndroidFont newFont(String filename, float size, boolean isBold){
        //paint.setTextSize(20); ???
        return new AndroidFont();
    }
    public void clear(int color){
        _graphics.drawRect(0,0,_graphics.getWidth(),_graphics.getHeight(), _paint);

    }
    public void translate(int x, int y){}

    @Override
    public void scale(float x, float y) {

    }

    public void save(){}
    public void restore(){}

    @Override
    public void drawImage(Image image, int w, int h) {
        _graphics.drawBitmap(((AndroidImage)image).getImage(),w,h,_paint);
    }

    @RequiresApi(api = Build.VERSION_CODES.O)
    public void setColor(int color) {

        //Color ole= new Color().valueOf(0xffff0000);
        _graphics.drawColor(color);
        _paint.setColor(color);      //Revisar
    }

    public void fillCircle(int cx, int cy, int r){
        Paint a= new Paint();
        _graphics.drawCircle(cx,cy,r,a);
    }

    @Override
    public void drawText(Font font, String text, int x, int y) {
        //font.size ??? paint.setTextSize(20);
        _graphics.drawText(text, x, y, _paint);
    }


    @Override
    public int getTextHeight(Font font, String string) {
        return 0;
    }

    @Override
    public int getWindowWidth(){
        return width;
    }
    @Override
    public int getWindowHeight() {
        return height;
    }

    private int width = 0;
    private int height = 0;
}
