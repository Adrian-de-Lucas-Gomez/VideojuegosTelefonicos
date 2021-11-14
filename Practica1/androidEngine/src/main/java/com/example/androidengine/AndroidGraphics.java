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

    private android.graphics.Canvas _canvas;    //Zona donde pintamos (DisplayBuffer)
    private Paint _paint;                       //Clase que nos permite pintar en el canvas dando color, tamaño. etc
    private Context _context;                   //Contexto de la app pasado por el engine
    private SurfaceView _surface;               //Referencia a una surface

    private AndroidFont fontInUse;              //Variable para guardar que fuente está activa en el _paint.Typeface

    public AndroidGraphics(Context c, int width, int height) {
        _context = c;   //Adjudicamos el contexto

        //Ponemos el tamaño logico
        _gameWidth = width;
        _gameHeight = height;
        //Calculamos relacion de aspecto
        _aspect = (float)_gameWidth / (float)_gameHeight;

        //Creamos Paint por defecto
        _paint = new Paint();
        _paint.setColor(Color.WHITE);
        _paint.setStyle(Paint.Style.FILL);
    }

    //Genera una imagen dado un nombre
    public Image newImage(String name){
        Image imagen= null;
        InputStream inputStream = null;

        try{
            //Tratamos de abrir el archivo
            AssetManager assetManager = _context.getAssets();
            inputStream = assetManager.open("sprites/"+name);
            //Tratamos de decodificarlo como un Bitmap
            Bitmap bMap = BitmapFactory.decodeStream(inputStream);

            //Se lo pasamos a la constructora de la clase AndroidImage
            imagen = new AndroidImage(bMap);
        }
        catch(IOException io){
            android.util.Log.e("AndroidGraphics", "Error leyendo abriendo imagen");
        }
        finally{    //Si o si hay que cerrar el archivo que hemos intentado leer
            if(inputStream != null) {
                try {
                    inputStream.close();
                } catch (Exception io) {
                }
            }
        }
        return imagen;
    }

    //Cargamos una fuente especificando el nombre, el tamaño y si se escribe en negrita
    public AndroidFont newFont(String filename, float size, boolean isBold){
        AssetManager manager = _context.getAssets();
        //Pasamos la referencia al manager de recursos y los otros parametros y nos da la Font
        return new AndroidFont(manager, filename, size, isBold);
    }

    @Override
    public void clear(int color){
        //Pintamos toda la pantalla con un color al que añadimos el canal alpha
        color = (0x000000ff << 24) | (color & 0x00ffffff);
        _canvas.drawColor(color);

    }
    //Trasladamos el canvas dados x,y
    @Override
    public void translate(float x, float y){
        _canvas.translate(x,y);
    }

    //Escalamos el canvas dados x,y
    @Override
    public void scale(float x, float y) {
        _canvas.scale(x,y);
    }

    //Metodos pàra salvar y restaurar el estado de tranfromación que tiene el canvas
    public void save(){ _canvas.save(); }
    public void restore(){ _canvas.restore(); }

    @Override
    public void drawImage(Image image, float w, float h, float alpha, boolean isCentered) {

        if(image != null) {     //Comprobamos que no sea una referencia nula
            Rect src;
            Rect dst;

            //Si la imagen debe de estar centrada se le resta la mitad del ancho y alto
            if (isCentered){
                src = new Rect(0, 0, image.getWidth(), image.getHeight());
                dst = new Rect((int)(-w * 0.5f), (int)(-h * 0.5f), (int)(w * 0.5f), (int)(h * 0.5f));
            }
            else{
                src = new Rect(0, 0, image.getWidth(), image.getHeight());
                dst = new Rect(0, 0, (int)w, (int)h);
            }

            //Comprobamos si el valor de alpha no se pasa del maximo permitido en ese momento
            if(alpha*255f < (_maxAlpha * 255f)) _paint.setAlpha((int)(alpha * 255f));
            else _paint.setAlpha((int)(_maxAlpha * 255));

            //Con el alpha decidido se pinta en pantalla el bitmap
            _canvas.drawBitmap(((AndroidImage)image).getImage(), src, dst, _paint);

            //Ponemos el alpha al valor maximo permitido
            _paint.setAlpha((int)(_maxAlpha * 255f));
        }
    }

    //Metodo para el dibujado de un circulo en pantalla (usa el color de _paint)
    public void fillCircle(float cx, float cy, float r, float alpha){
        if(alpha*255 < _maxAlpha * 255) _paint.setAlpha((int)(alpha * 255));
        else _paint.setAlpha((int)(_maxAlpha * 255));
        _canvas.drawCircle(cx,cy,r,_paint);
        _paint.setAlpha((int)(_maxAlpha * 255));

    }

    //Dibujado del texto en pantalla usando la fuente activa y con el texto dado
    @Override
    public void drawText(String text, float x, float y, Boolean isCenteredX, Boolean isCenteredY) {

        int color = _paint.getColor();
        //Se crea un color con el alpha maximo
        _paint.setARGB((int)(_maxAlpha * 255),Color.red(color), Color.green(color), Color.blue(color));
        //Se mira si se debe de pintar en negrita
        _paint.setFakeBoldText(fontInUse.isBold());
        //Se pinat del tamaño con el que se especificó
        _paint.setTextSize(fontInUse.getSize());

        //Centrado en los ejes correspondientes
        if(isCenteredX) x -= getTextWidth(text) * 0.5;
        if(isCenteredY) y += getTextHeight(text)* 0.5;

        _canvas.drawText(text, x, y, _paint);
    }

    //Referente a pillar y soltar canvas -------------------------------------------------

    public void lockCanvas(){
        //Tratamos de acceder a una surface
        while(!_surface.getHolder().getSurface().isValid()) { /*No hago nada, solo esperar*/}

        //Si la version de API lo permite usamos el HardwareCanvas
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            _canvas = _surface.getHolder().lockHardwareCanvas();
        }
        else{   //En caso contrario usamos el normal
            _canvas = _surface.getHolder().lockCanvas();
        }

        if(_canvas == null) System.out.println("El canvas devolvio null");
    }

    //Lo soltamos y renderizamos en pantalla
    public void releaseCanvas() { _surface.getHolder().unlockCanvasAndPost(_canvas); }

    //----------------------------------------------------------------------------------

    //Seleccionamos el color con el que va adibujar el _paint
    @Override
    public void setColor(int color) {
        _paint.setColor(color);
    }

    //Elegimos y marcamos en _paint y en FontInUse la fuente con la que vamos a escribir
    public void setFont(Font font){
        fontInUse = ((AndroidFont)font);
        _paint.setTypeface(fontInUse.getFont());
    }

    //Marcamos el valor maximo de alfa que queremos usar (Efecto para transiciones)
    @Override
    public void setMaxAlpha(float alpha) {
        _maxAlpha = alpha;
        _paint.setAlpha((int)(_maxAlpha * 255f));
    }

    //Adjudicamos la referencia _surface al que nos pasa el buvle principal de juego en engine
    public void setSurfaceView(SurfaceView surfaceView){
        _surface = surfaceView;
    }

    //Obtenemos el tamaño que ocupa el texto verticalmente en pantalla (tema de centrado)
    @Override
    public int getTextHeight(String string) {
        Rect result = new Rect();
        _paint.getTextBounds(string, 0, string.length(), result);
        return result.height();
    }

    //Obtenemos el tamaño que ocupa el texto horizontalmente en pantalla (tema de centrado)
    @Override
    public int getTextWidth(String string) {
        Rect result = new Rect();
        _paint.getTextBounds(string, 0, string.length(), result);
        return result.width();
    }

    //Metodos para conocer el alto y ancho de surface
    @Override
    public int getWindowWidth(){
        return _surface.getWidth();
    }
    @Override
    public int getWindowHeight() {
        return _surface.getHeight();
    }
}
