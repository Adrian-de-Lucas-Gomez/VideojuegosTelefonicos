package com.example.engine;

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
    void translate(float x, float y);
    void scale(float x, float y);
    void save();
    void restore();
    void setScaleAspect();

    /**
     * Recibe una imagen y la muestra en la pantalla
     */
    void drawImage(Image image, float w, float h, float alpha, boolean isCentered);

    /**
     * Establece el color a utilizar en las operaciones de
     * dibujado posteriores
     */
    void setColor(int color);

    /**
     * Establece la fuente a utilizar en las operaciones de
     * dibujado posteriores
     */
    void setFont(Font font);

    /**
     * Dibuja un circulo relleno del color activo
     */
    void fillCircle(float cx, float cy, float r, float alpha);

    /**
     * Escribe el texto con la fuente y color activos
     */
    void drawText(String text, float x, float y, Boolean isCenteredX, Boolean isCenteredY);

    /**
     * Escribe el valor alpha máximo que pueden alcanzar los elementos gráficos
     */
    void setMaxAlpha(float alpha);

    /**
     * Devuelven el tamaño de la ventana real y logico
     */
    int getWindowWidth();
    int getWindowHeight();
    int getTextHeight(String string);
    int getTextWidth(String string);
    float getLogicScaleAspect();
    int getOffsetX();
    int getOffsetY();
}
