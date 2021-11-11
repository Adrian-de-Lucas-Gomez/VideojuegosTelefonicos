package com.example.androidengine;

import android.content.Context;
import android.content.res.AssetManager;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Typeface;
import android.view.SurfaceView;

import com.example.engine.AbstractGraphics;
import com.example.engine.Font;
import com.example.engine.Image;

//Ahora implementa AbstractGraphics para no repetir codigo

public class AndroidGraphics extends AbstractGraphics {

    private android.graphics.Canvas _canvas;
    private Paint _paint;
    private Context _context;
    private SurfaceView _surface;

    public AndroidGraphics(Context c) {
        _context = c;   //Adjudicamos el contexto

        _paint = new Paint();
        _paint.setColor(Color.WHITE);
        _paint.setStyle(Paint.Style.FILL);

    }

    public Image newImage(String name){

        AndroidImage img = null;

        Bitmap bMap = BitmapFactory.decodeFile("./assets/sprites/" + name);

        return new AndroidImage(bMap);
    }

    //Revisar (no se si es mejor cargarlo aqui y asi nos ahorramos pasar el context o mejor hacerlo en AndroidFont como en PcFont)
    public AndroidFont newFont(String filename, float size, boolean isBold){
        AssetManager manager = _context.getAssets();
        return new AndroidFont(manager, filename);
    }

    @Override
    public void clear(int color){
        _canvas.drawRect(0,0, _canvas.getWidth(), _canvas.getHeight(), _paint);

    }

    @Override
    public void translate(float x, float y){}

    @Override
    public void scale(float x, float y) {

    }
    //Revisar-------------------------------------------------------------------------------------------------------
    public void setSurfaceView(SurfaceView surfaceView){_surface = surfaceView;}
    //--------------------------------------------------------------------------------------------------------------
    public void save(){}
    public void restore(){}

    @Override
    public void drawImage(Image image, float w, float h) {
        _canvas.drawBitmap(((AndroidImage)image).getImage(),w,h,_paint);
    }

    @Override
    public void setColor(int color) {

        //Color ole= new Color().valueOf(0xffff0000);   //Necesita API avanzada
        _canvas.drawColor(color);
        _paint.setColor(color);      //Revisar
    }

    public void fillCircle(float cx, float cy, float r){
        Paint a= new Paint();
        _canvas.drawCircle(cx,cy,r,a);
    }

    @Override
    public void drawText(Font font, String text, float x, float y) {
        //paint.setTextSize(20); ???
        _paint.setTypeface(((AndroidFont)font).getFont());
        _canvas.drawText(text, x, y, _paint);
    }

    //Referente a pillar y soltar cavas -----------------------------------------------------------------------------------------------

    public void lockCanvas(){
        //En este while se queda pensando mucho
        while(!_surface.getHolder().getSurface().isValid());    //Tratamos de acceder a una surface
        _canvas = _surface.getHolder().lockCanvas();

        if(_canvas == null) System.out.println("El canvas fue null");
    }

    // unclock canvas -> se libera
    public void releaseCanvas(){ _surface.getHolder().unlockCanvasAndPost(_canvas); }  //Lo soltamos y renderizamos


    @Override
    public int getTextHeight(Font font, String string) {
        return 0;
    }

    @Override
    public int getTextWidth(Font font, String string) { return 0; }

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
