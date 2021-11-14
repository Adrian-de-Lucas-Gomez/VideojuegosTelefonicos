package com.example.androidengine;

import android.content.Context;
import android.content.res.AssetManager;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Rect;
import android.os.Build;
import android.view.SurfaceView;

import com.example.engine.AbstractGraphics;
import com.example.engine.Font;
import com.example.engine.Image;

import java.io.IOException;
import java.io.InputStream;


public class AndroidGraphics extends AbstractGraphics {

    private android.graphics.Canvas _canvas;
    private Paint _paint;
    private Context _context;
    private SurfaceView _surface;

    private AndroidFont fontInUse;

    public AndroidGraphics(Context c, int width, int height) {
        _context = c;   //Adjudicamos el contexto
        _gameWidth = width;
        _gameHeight = height;
        _aspect = (float)_gameWidth / (float)_gameHeight;

        _paint = new Paint();
        _paint.setColor(Color.GREEN); //TODO
        _paint.setStyle(Paint.Style.FILL);
    }

    public Image newImage(String name){
        Image imagen= null;
        InputStream inputStream = null;

        try{
            AssetManager assetManager = _context.getAssets();
            inputStream = assetManager.open("sprites/"+name);
            Bitmap bMap = BitmapFactory.decodeStream(inputStream);
            imagen = new AndroidImage(bMap);
        }
        catch(IOException io){
            android.util.Log.e("AndroidGraphics", "Error leyendo abriendo imagen");
        }
        finally{
            if(inputStream != null) {
                try {
                    inputStream.close();
                } catch (Exception io) {
                }
            }
        }
        return imagen;
    }


    public AndroidFont newFont(String filename, float size, boolean isBold){
        AssetManager manager = _context.getAssets();
        return new AndroidFont(manager, filename, size, isBold);
    }

    @Override
    public void clear(int color){
        color = (0x000000ff << 24) | (color & 0x00ffffff);
        _canvas.drawColor(color);

    }

    @Override
    public void translate(float x, float y){
        _canvas.translate(x,y);
    }
    @Override
    public void scale(float x, float y) {
        _canvas.scale(x,y);
    }
    public void save(){ _canvas.save(); }
    public void restore(){ _canvas.restore(); }

    public void setSurfaceView(SurfaceView surfaceView){
        _surface = surfaceView;
    }

    @Override
    public void drawImage(Image image, float w, float h, float alpha, boolean isCentered) {

        if(image != null) {
            Rect src;
            Rect dst;

            if (isCentered){
                src = new Rect(0, 0, image.getWidth(), image.getHeight());
                dst = new Rect((int)(-w * 0.5f), (int)(-h * 0.5f), (int)(w * 0.5f), (int)(h * 0.5f));
            }
            else{
                src = new Rect(0, 0, image.getWidth(), image.getHeight());
                dst = new Rect(0, 0, (int)w, (int)h);
            }
            if(alpha*255f < (_maxAlpha * 255f)) _paint.setAlpha((int)(alpha * 255f));
            else _paint.setAlpha((int)(_maxAlpha * 255));
            _canvas.drawBitmap(((AndroidImage)image).getImage(), src, dst, _paint);
            _paint.setAlpha((int)(_maxAlpha * 255f));
        }
    }

    @Override
    public void setColor(int color) {
        _paint.setColor(color);
    }

    public void setFont(Font font){
        fontInUse = ((AndroidFont)font);
        _paint.setTypeface(fontInUse.getFont());
        //Revisar
    }

    public void fillCircle(float cx, float cy, float r, float alpha){
        if(alpha*255 < _maxAlpha * 255) _paint.setAlpha((int)(alpha * 255));
        else _paint.setAlpha((int)(_maxAlpha * 255));
        _canvas.drawCircle(cx,cy,r,_paint);
        _paint.setAlpha((int)(_maxAlpha * 255));

    }

    @Override
    public void drawText(String text, float x, float y, Boolean isCenteredX, Boolean isCenteredY) {

        int color = _paint.getColor();
        _paint.setARGB((int)(_maxAlpha * 255),Color.red(color), Color.green(color), Color.blue(color));
        _paint.setFakeBoldText(fontInUse.isBold());
        _paint.setTextSize(fontInUse.getSize());

        if(isCenteredX) x -= getTextWidth(text) * 0.5;
        if(isCenteredY) y += getTextHeight(text)* 0.5;

        _canvas.drawText(text, x, y, _paint);
    }

    @Override
    public void setMaxAlpha(float alpha) {
        _maxAlpha = alpha;
        _paint.setAlpha((int)(_maxAlpha * 255f));
    }

    //Referente a pillar y soltar canvas -------------------------------------------------

    public void lockCanvas(){
        //Tratamos de acceder a una surface
        while(!_surface.getHolder().getSurface().isValid()) { /*No hago nada*/}

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            _canvas = _surface.getHolder().lockHardwareCanvas();
        }
        else{
            _canvas = _surface.getHolder().lockCanvas();
        }

        if(_canvas == null) System.out.println("El canvas devolvio null");
    }

    //Lo soltamos y renderizamos
    public void releaseCanvas() { _surface.getHolder().unlockCanvasAndPost(_canvas); }

    //----------------------------------------------------------------------------------

    @Override
    public int getTextHeight(String string) {
        Rect result = new Rect();
        _paint.getTextBounds(string, 0, string.length(), result);
        return result.height();
    }

    @Override
    public int getTextWidth(String string) {
        Rect result = new Rect();
        _paint.getTextBounds(string, 0, string.length(), result);
        return result.width();
    }

    @Override
    public int getWindowWidth(){
        return _surface.getWidth();
    }
    @Override
    public int getWindowHeight() {
        return _surface.getHeight();
    }
}
