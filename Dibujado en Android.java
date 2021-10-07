Dibujado en Android

Renderizado pasivo:

Renderizado activo:
///////////////////////////////////////////////////////////////////////

import android.view.view

static class myView extends View{
    public myView(Context context){
        super(context)
    }

    Bitmap sprite;

    public void onDraw(Canvas c){
        c.draw......() //Hay muchas posibilidades

        c.drawColor(0xF0F0F0F0);

        if(sprite != null){
            long currentTime = System.nanoTime;
            long nanoElapsedTime = currentTime - lastFrameTime;
            lastFrameTime = currentTime;
            double deltaTime= (double) nanoElapsedTime / 1.0E09;

            double += deltaTime * 5;

            

            c.drawBitmap(sprite, x,100,null);
        }
    }

}

Carga de imagenes:
Pinchar sobre modulo > nuevo > carpeta > moduloMain

en OnCreate():
//InputStream is= null;

AssetManager assetManager = this.getAssets();
/*
try{
    is = assetManager.open("android.png");
    Bitmap sprite= BitmapFactory.decodeStream(is);
}
catch (IOException){
    e.printStackTrace();
}
finally{
    try{
        is.close();
    }
    catch (IOException){
        e.printStackTrace();
    }
}
*/

try(InputStream is=  assetManager.open("android.png");){
    Bitmap sprite= BitmapFactory.decodeStream(is);
}
catch (IOException){
    e.printStackTrace();
}

setContentView(new myView(this));


Creacion de hebras:

class hebra extends Thread{
    public void run(){
        while(true){
            System.out.println("hebra");

            try{
                Thread.sleep(50);
            }
            catch(Exception e){
                
            }
        }
        
    }
}

en main:

hebra h = new hebra();

h.start(); //Llama a run por defecto

Thread aux = new Thread(hebra); //hebra es una clase runable, que se la podemos pasar al thread
