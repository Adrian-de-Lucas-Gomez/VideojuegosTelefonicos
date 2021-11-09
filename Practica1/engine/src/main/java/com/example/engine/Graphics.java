package com.example.engine;

import java.awt.Color;

import jdk.internal.net.http.common.Pair;

public interface Graphics {
    /**
     * Carga una imagen almacenada en el contenedor de recursos
     * de la aplicacion a partir de su nombre
     */
    Image newImage(String name);

    /**
     * Crea una nueva fuente del tamanho especificado a partir
     * de un fichero .ttf. Se indica si se desea o no fuente
     * en negrita.
     */
    Font newFont(String filename, float size, boolean isBold);

    /**
     * Borra el contenido completo de la ventana, rellenandola
     * con un color recibido como parametro
     */
    void clear(int color);

    /**
     * Metodos de control de la transformacion sobre el canvas.
     * Las operaciones de dibujado se veran afectadas por la
     * transformacion establecida
     */
    void translate(int x, int y);
    void scale(float x, float y);
    void save();
    void restore();

    /**
     * Recibe una imagen y la muestra en la pantalla. Se pueden
     * necesitar diferentes versiones de este metodo dependiendo
     * de si se permite o no escalar la imagen, si se permite
     * elegir que porción de la imagen original se muestra, etc.
     */
    void drawImage(Image image, int w, int h);

    /**
     * Establece el color a utilizar en las operaciones de
     * dibujado posteriores
     */
    void setColor(Color color);

    /**
     * Dibuja un circulo relleno del color activo
     */
    void fillCircle(int cx, int cy, int r);

    /**
     * Escribe el texto con la fuente y color activos
     */
    void drawText(Font font, String text, int x, int y);

    /**
     * Devuelven el tamaño de la ventana real y logico
     */
    int getWindowWidth();
    int getWindowHeight();
    int getGameWidth();
    int getGameHeight();
    int getTextHeight(Font font, String string);
    int getTextWidth(Font font, String string);
    float getLogicScaleAspect();
    int getOffsetX();
    int getOffsetY();
}
