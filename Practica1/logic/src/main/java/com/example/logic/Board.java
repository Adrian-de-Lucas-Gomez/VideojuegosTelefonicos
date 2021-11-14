package com.example.logic;

import com.example.engine.Button;
import com.example.engine.Font;
import com.example.engine.Graphics;
import com.example.engine.Image;

import java.util.ArrayList;
import java.util.Collections;
import java.util.Random;
import java.util.Stack;

enum TileType{
    Unknown, Dot, Wall, Value, None
}

enum HintType{
    VeLasNecesarias, //Ve las casillas necesarias y hay que cerrarla
    SiExpandeSuperaValor, //Si expande con una casilla azul, supera el valor
    RellenaTodosLosHuecos,
    AzulObligatorioEnDireccion, //En todas las soluciones imaginables hay una dirección en la que hay que poner azules
    TableroResuelto, //Utilizada para resolver el tablero. Se devuelve si no ha encontrado ninguna otra pista y no hay casillas vacias.
    //SoloUnaDireccion, //Una ficha solo se puede expandir en una dirección

    //Errores del jugador
    VaciaEsMuro, //Si a un espacio vacio no le ve nadie (no tiene a ningun value en x o en y) tiene que ser muro
    AzulEsMuro, //Si a un azul no le ve nadie (igual que la pista anterior) tiene que ser un muro
    FichaCerradaEsIncorrecta, //Una ficha resuelta ve menos azules de los que debería
    VeDemasiadas, //Ve más puntos azules de los necesarios

    None
}

public class Board{
    //Funcionamiento interno del tablero
    private Tile[][] _tiles; //Almacena todas las casillas con el estado actual de la partida
    private Tile[][] _empty; //Almacena el estado inicial de la partida. Se utiliza para determinar que fichas son "fijas"
    private Tile _nullTile;
    private int _size; //Tamaño del tablero, tiene en cuenta los muros externos.
    private int numTilesFilled = 0; //Casillas que el jugador ha rellenado.
    private int _numPlayeableTiles = 0; //Número de casillas del tablero sin tener en cuenta los muros externos.
    private boolean _solved = false;

    //Generación del tablero
    ArrayList<Tile> _blueTiles = new ArrayList<Tile>(); //Lista de fichas azules
    ArrayList<Tile> _redTiles = new ArrayList<Tile>(); //Lista de muros
    ArrayList<Tile> _auxTiles = new ArrayList<Tile>(); //Lista con todas las fichas
    private Random _rand;

    //Resolución del tablero
    private TileHint _tileHint; //Clase para almacenar datos sobre pistas.
    private Direction[] _directions; //4 direcciones cardinales {{1, 0}, {-1, 0}, ... }
    private TileDirectionInfo[] _tileDirInfo; //Almacena información (fichas visibles, ...) por cada dirección

    //Deshacer jugadas
    private Stack<Tile> _moves;

    //Pintado
    private Graphics _graphics;
    private int _dotColor, _wallColor, _emptyColor, _textColor, _hintColor;
    private Font _tileFont;
    private Button[][] _tileButtons; //Matriz que almacena los botones de cada ficha
    private int _paintingSize; //Tamaño de pintado del tablero
    private String _boardFeedbackText = ""; //Texto que tiene que pide Logic cuando pide una pista o deshace una jugada
    private float _circleDiameter = 0.8f; //El radio del círculo medirá n veces el tamaño de su casilla asignada
    private Tile _hintTile; //Ficha a la que se refiere la pista
    private Image _lockImage; //Lock.png
    boolean _pressedLockedTile = false; //Indica si el jugador ha pulsado una ficha fijada (forma parte de la disposición inicial de la partida)

    //Animacion de los botones
    private float _iButtonAlpha = 0f, _eButtonAlpha = 1f, _iButtonScale = 1f, _eButtonScale = 1.15f;
    private int _nScalingRepetitions = 4;
    private float _buttonAnimTime = 0.25f;

    public Board(Graphics graphics, int paintingSize){
        _graphics = graphics;
        _rand = new Random();
        _tileHint = new TileHint();
        _tileDirInfo = new TileDirectionInfo[4];
        _paintingSize = paintingSize;
        _nullTile = new Tile(0, 0, 0, TileType.None);
        _hintTile = new Tile(0, 0, 0, TileType.None);
        _moves = new Stack<Tile>();
        _lockImage = _graphics.newImage("lock.png");
        for(int k = 0; k < 4; k++){
            _tileDirInfo[k] = new TileDirectionInfo();
        }
        _directions = new Direction[]{new Direction(-1, 0), new Direction(1, 0), new Direction(0, 1), new Direction(0, -1)};
    }

    public void setSize(int size){
        _size = size + 2; //Muros alrededor del tablero
        _numPlayeableTiles = (_size * _size);
        _tiles = new Tile[_size][_size];
        _empty = new Tile[_size][_size];
    }

    public void setPaintColors(int dotColor, int wallColor, int emptyColor, int textColor, int hintColor){
        _dotColor = dotColor;
        _wallColor = wallColor;
        _emptyColor = emptyColor;
        _textColor = textColor;
        _hintColor = hintColor;
    }

    public void clear(){
        _hintTile.setX(0);
        _hintTile.setY(0);
        _pressedLockedTile = false;
        _boardFeedbackText = "";
    }

    public void setFonts(Font tileFont){
        _tileFont = tileFont;
        _graphics.setFont(_tileFont);
    }

    /**
     * Establece la posición y las animaciones de los botones.
     * @param offsetX Offset lógico del tablero en el eje X(Si el tablero empieza en (200, 100) el offset en X es 200
     * @param offsetY Offset lógico del tablero en el eje Y
     */
    public void setButtons(int offsetX, int offsetY){
        _tileButtons = new Button[_size - 2][_size - 2];
        int sizePerButton = _paintingSize / (_size - 2);
        for(int k = 1; k < _size - 1; k++){
            for(int l = 1; l < _size - 1; l++){
                _tileButtons[k - 1][l - 1] = new Button(offsetX + sizePerButton * (l - 1) + (sizePerButton -  sizePerButton * _circleDiameter) / 2, offsetY + sizePerButton * (k - 1) + (sizePerButton -  sizePerButton * _circleDiameter) / 2,
                        sizePerButton * _circleDiameter, sizePerButton * _circleDiameter, _lockImage, 1f, 1f);
                //Si la casilla es de tipo desconocido en _empty, es que no es fija
                if(_empty[k][l].getType() == TileType.Unknown) {
                    _tileButtons[k - 1][l - 1].setAnimationAlpha(_iButtonAlpha, _eButtonAlpha, _buttonAnimTime);
                    _tileButtons[k - 1][l - 1].setScalingAnimation(_iButtonScale, _eButtonScale, _buttonAnimTime, 1);
                }
                else{
                    _tileButtons[k - 1][l - 1].setScalingAnimation(_iButtonScale, _eButtonScale, _buttonAnimTime * 5, _nScalingRepetitions);
                }
            }
        }
    }

    /**
     * Deshace la jugada anterior
     */
    public void revertPlay(){
        if(!_moves.empty()){
            //Extrae una ficha y le aplica un toggle inverso
            Tile t = _moves.pop();
            //Orden inverso: Empty -> Wall -> Dot
            _boardFeedbackText = "Esta casilla se devolvió a ser ";
            _tiles[t.getY()][t.getX()].toggleType_i();
            _tileButtons[t.getY() - 1][t.getX() - 1].activateAnimation();
            if (t.getType() == TileType.Dot) {
                numTilesFilled++; //Para visualizarlo en el porcentaje de fichas rellenadas
                _boardFeedbackText += "azul";
            }
            else if (t.getType() == TileType.Wall) {
                _boardFeedbackText += "roja";
            }
            else if(t.getType() == TileType.Unknown){
                numTilesFilled--;
                _boardFeedbackText += "vacía";
            }
            _hintTile = t;
        }
        else {
            _boardFeedbackText = "No quedan jugadas por revertir.";
            _hintTile = _nullTile; //Se deja de apuntar a la ficha de pista anterior.
        }
    }

    /**
     * Pintado del tablero
     */
    public void render(){
        int sizePerTile = _paintingSize / (_size - 2); //Cuadrado asignado a cada casilla para pintar
        Tile auxTile;
        Button auxButton;
        _graphics.setFont(_tileFont);
        for(int k = 1; k < _size - 1; k++){
            _graphics.save();
            for(int l = 1; l < _size - 1; l++){
                auxTile = _tiles[k][l];
                auxButton = _tileButtons[auxTile.getY() - 1][auxTile.getX() - 1];

                //Si la casilla a dibujar es a la que apunta la pista actual, se le dibuja un círculo negro alrededor.
                if(auxTile.getX() == _hintTile.getX() && auxTile.getY() == _hintTile.getY()){
                    _graphics.setColor(_hintColor);
                    _graphics.fillCircle(sizePerTile/2.0f, sizePerTile/2.0f, sizePerTile * 0.5f * auxButton.getScale(), auxButton.getAlpha());
                }

                if(auxTile.getType() == TileType.Dot || auxTile.getType() == TileType.Value) _graphics.setColor(_dotColor);
                else if (auxTile.getType() == TileType.Wall) _graphics.setColor(_wallColor);
                else _graphics.setColor(_emptyColor);

                _graphics.fillCircle(sizePerTile/2.0f, sizePerTile/2.0f, sizePerTile * _circleDiameter * 0.5f * auxButton.getScale(), auxButton.getAlpha());

                //Los muros fijos tienen que tener una imagen de candado dibujada si el jugador ha presionado una ficha fija
                if(_empty[k][l].getType() == TileType.Wall && _pressedLockedTile) {
                    _graphics.translate(sizePerTile * 0.5f, sizePerTile * 0.5f);
                    _graphics.drawImage(_lockImage, auxButton.getWidth() * 0.7f * auxButton.getScale(), auxButton.getHeight() * 0.7f * auxButton.getScale(), 0.5f, true);
                    _graphics.translate(-sizePerTile * 0.5f, -sizePerTile * 0.5f);
                }

                _graphics.setColor(_textColor);

                if(auxTile.getType() == TileType.Value)
                    _graphics.drawText(Integer.toString(auxTile.getValue()), sizePerTile * 0.5f, sizePerTile * 0.5f, true, true);

                _graphics.translate(sizePerTile, 0);
            }
            _graphics.restore();
            _graphics.translate(0, sizePerTile);
        }
        _graphics.translate(0, -sizePerTile * (_size - 2)); //Devolvemos el pincel a la posición inicial.
    }

    public void generate(){
        //Reparto de numero de fichas
        _solved = false;
        float percentageBlues = 0.7f, minPercentageBlues = 0.2f;
        numTilesFilled = _numPlayeableTiles;
        int maxNumBlues = (int)(_numPlayeableTiles * percentageBlues);
        int maxNumReds = _numPlayeableTiles - maxNumBlues;
        int maxEliminatedBlues = (int)((percentageBlues - minPercentageBlues) * _numPlayeableTiles);

        //Generación de tablero "resuelto"
        int auxBlues = maxNumBlues, auxReds = maxNumReds; //Para controlar el número de fichas que quedan por poner por cada tipo
        for(int k = 0; k < _size; k++){
            for(int l = 0; l < _size; l++){
                _tiles[k][l] = new Tile(l, k, 0, TileType.None);
                _empty[k][l] = new Tile(l, k, 0, TileType.None);
                //Las fichas exteriores tienen que ser muros
                if(k == 0 || l == 0 || k == _size - 1 || l == _size - 1){
                    _tiles[k][l].setType(TileType.Wall);
                }
                else{
                    //Si no podemos poner más fichas azules, podemos rojas
                    if(auxBlues == 0){
                        _tiles[k][l].setType(TileType.Wall);
                        auxReds--;
                    }
                    //Si no podemos poner más fichas rojas, podemos azules
                    else if (auxReds == 0){
                        _tiles[k][l].setType(TileType.Value);
                        auxBlues--;
                    }
                    else{
                        //Si podemos poner cualquiera de las dos, ponemos una aleatoria
                        if(_rand.nextInt(2) == 0){
                            _tiles[k][l].setType(TileType.Wall);
                            auxReds--;
                        }
                        else{
                            _tiles[k][l].setType(TileType.Value);
                            auxBlues--;
                        }
                    }
                }
            }
        }

        //Asignación de valores a los values y limpieza de fichas azules solitarias
        calculateValues();
        //Hay que cambar las fichas azules con valor superior a tamaño del tablero - 2. Para ello, vamos a ordenarlas.
        Collections.sort(_blueTiles);
        //La manera en la que se eliminan las fichas azules con valores demasiado elevados es eliminando la azul y cambiándola por una roja.
        //Para mantener el equilibrio, por cada conversión hay que convertir una ficha roja en una ficha azul.
        Collections.shuffle(_redTiles);

        while(_blueTiles.get(0).getValue() > _size - 2){
            Tile toRed = _blueTiles.remove(0); //Ficha con valor demasiado elevado que se va a convertir a roja.
            Tile toBlue = _redTiles.remove(0); //Ficha roja aleatoria que se va a convertir a azul.
            _tiles[toRed.getY()][toRed.getX()].setType(TileType.Wall);
            _tiles[toRed.getY()][toRed.getX()].setValue(0); //No debería ser necesario pero lo dejo por seguridad.
            _tiles[toBlue.getY()][toBlue.getX()].setType(TileType.Value);
            calculateValues();
            Collections.sort(_blueTiles);
            Collections.shuffle(_redTiles);
        }

        //Ahora que tenemos una solución completa, toca eliminar fichas aleatorias hasta que se obtenga un tablero lo suficientemente vacío pero resoluble.

        Tile[][] auxBoard = new Tile[_size][_size];
        for(int k = 0; k < _size; k++){
            for(int l = 0; l < _size; l++){
                auxBoard[k][l] = new Tile(_tiles[k][l]);
            }
        }

        //Convertimos fichas a desconocidas hasta que conseguimos un tablero con el número suficiente de fichas desconocidas
        //pero resoluble.
        //Al convertir a desconocida una ficha aleatoria es probable que ya no se pueda encontrar una solución. Para ello damos
        //un número limitado de intentos.
        int numTries = 4;
        Collections.shuffle(_auxTiles);
        Tile randomTile;
        while(numTries > 0 && _auxTiles.size() > 0){ //La última comprobación no debería ser necesaria pero la dejo por seguridad.
            //Nos guardamos el tablero actual en un tablero auxiliar y convertimos en desconocida una ficha aleatoria.
            //si al convertirla no podemos resolverlo, restamos 1 intento al número de intentos disponibles y restauramos
            //el tablero al paso anterior. Si es resoluble, eliminamos otra ficha.
            copyBoard(_tiles, auxBoard);
            randomTile = _auxTiles.remove(0);
            if(randomTile.getType() == TileType.Value){
                if(auxBlues >= maxEliminatedBlues) {
                    while(randomTile.getType() == TileType.Value && _auxTiles.size() > 0){
                        randomTile = _auxTiles.remove(0);
                    }
                }
                else auxBlues++;
            }
            _tiles[randomTile.getY()][randomTile.getX()].setType(TileType.Unknown);
            _tiles[randomTile.getY()][randomTile.getX()].setValue(0);
            _tileHint = getFirstHint();
            boolean isSolveable = solveBoard();
            copyBoard(auxBoard, _tiles);
            if (!isSolveable) { //No se puede quitar esta ficha. Se vuelve a intentar
                numTries--;
                _auxTiles.add(randomTile);
            }
            else{ //Se puede solucionar con la disposicion actual de fichas
                numTilesFilled--;
                _tiles[randomTile.getY()][randomTile.getX()].setType(TileType.Unknown);
                _tiles[randomTile.getY()][randomTile.getX()].setValue(0);
            }
        }
        copyBoard(_tiles, _empty);
    }

    public boolean handleInput(int posX, int posY){
        for(int k = 1; k < _size - 1; k++){
            for(int l = 1; l < _size - 1; l++){
                if(_tileButtons[k - 1][l - 1].isPressed(posX, posY)){
                    Tile pressedTile = _tiles[k][l];
                    //Si la ficha es parte de la disposición inicial es que está "bloqueada" y no se puede modificar
                    if(_empty[pressedTile.getY()][pressedTile.getX()].getType() == TileType.Unknown){
                        switch (pressedTile.getType()){
                            //Orden de toggle: Dot -> Wall -> Unknown
                            case Dot:
                                _moves.add(pressedTile);
                                _tiles[k][l].toggleType();
                                //Comprobamos si el tablero está resuelto
                                _solved = (getFirstHint().getType() == HintType.TableroResuelto);
                                break;
                            case Wall:
                                _moves.add(pressedTile);
                                _tiles[k][l].toggleType();
                                numTilesFilled--;
                                break;
                            case Unknown:
                                numTilesFilled++;
                                _moves.add(pressedTile);
                                _tiles[k][l].toggleType();
                                //Comprobamos si el tablero está resuelto
                                _solved = (getFirstHint().getType() == HintType.TableroResuelto);
                                break;
                        }
                        _pressedLockedTile = false;
                        _hintTile = _nullTile; //Como se ha tocado otra ficha, la pista anterior se elimina
                        _boardFeedbackText = "";
                        if(isSolved()) setAllDotToValue();
                    }
                    else _pressedLockedTile = true;
                    //Activamos la animacion de la ficha.
                    _tileButtons[k - 1][l - 1].activateAnimation();
                    return true;
                }
            }
        }
        return false;
    }

    public String get_boardFeedbackText(){
        return _boardFeedbackText;
    }

    public boolean isSolved(){
        return _solved;
    }

    /**
     * Devuelve el porcentaje de fichas rellenadas. (No son desconocidas)
     */
    public int getPercentageFilled(){
        if(_solved) return 100;
        float percentageFilled = (float)numTilesFilled / _numPlayeableTiles * 100;
        double fractionalPart = percentageFilled % 1;
        return (int)(percentageFilled - fractionalPart);
    }

    /**
     * Resuelve el tablero y devuelve si el tablero es resoluble.
     */
    public boolean solveBoard( ){
        _tileHint = getFirstHint();
        //Si el tablero detecta una pista que solo puede ocurrir por error del jugador, es que es irresoluble.
        //Si el tablero no detecta ninguna pista y no está completado (Pista None), es que tampoco es resoluble.
        while(!isHintPlayerMistake(_tileHint.getType()) || _tileHint.getType() != HintType.None){
            if(_tileHint.getType() == HintType.TableroResuelto) return true;
            else if(_tileHint.getType() == HintType.None) return false;
            solveHint(_tileHint);;
            _tileHint = getFirstHint();
        }
        return false;
    }

    /**
     * Busca una pinta y establece el texto que deberá ser pintado en Logic.
     */
    public void searchHintInTile(){
        TileHint hint = getFirstHint();
        _hintTile = hint.getOriginalTile();
        switch (hint.getType()) {
            case VeDemasiadas:
                _boardFeedbackText = "Esta casilla ve demasiadas.";
                break;
            case VeLasNecesarias:
                _boardFeedbackText = "Esta casilla ve las necesarias.";
                break;
            case RellenaTodosLosHuecos:
                _boardFeedbackText = "Esta casilla debe rellenar todos sus huecos.";
                break;
            case FichaCerradaEsIncorrecta:
                _boardFeedbackText = "Esta casilla no ve las necesarias.";
                break;
            case AzulObligatorioEnDireccion:
                _boardFeedbackText = "Un hueco está incluido en todas las soluciones.";
                break;
            case SiExpandeSuperaValor:
                _boardFeedbackText = "Si expande en un sentido supera el valor.";
                break;
            case AzulEsMuro:
                _boardFeedbackText = "Ninguna casilla ve a esta ficha azul";
                break;
            case VaciaEsMuro:
                _boardFeedbackText = "Ninguna casilla ve a esta ficha vacia";
                break;
            default:
                _boardFeedbackText = "Completado!!!";
                break;
        }
    }

    /**
     * Actualiza los botones para sus animaciones.
     */
    public void onUpdate(double deltaTime){
        for(int k = 0; k < _size - 2; k++){
            for(int l = 0; l < _size - 2; l++){
                _tileButtons[k][l].update(deltaTime);
            }
        }
    }

    /**
     * Asigna valores a las fichas azules del tablero y convierte en muro a las fichas azules que no son vistas por nadie.
     */
    private void calculateValues(){
        _blueTiles.clear();
        _redTiles.clear();
        _auxTiles.clear();
        for(int k = 1; k < _size - 1; k++){ //Tiene que ir desde [1][1] hasta [size-2][size-2] por el muro externo
            for(int l = 1; l < _size - 1; l++){
                if(_tiles[k][l].getType() == TileType.Value){
                    int numDotsSeen = getNumDotsSeen(_tiles[k][l]);
                    //Si una ficha azul no ve a nadie tiene que ser muro.
                    if(numDotsSeen == 0) {_tiles[k][l].setType(TileType.Wall); _redTiles.add(_tiles[k][l]);}
                    else {_tiles[k][l].setValue(numDotsSeen); _blueTiles.add(_tiles[k][l]);}
                }
                else _redTiles.add(_tiles[k][l]);
                _auxTiles.add(_tiles[k][l]);
            }
        }
    }

    /**
     * Copia el estado de un tablero de uno a otro.
     * @param src Tablero a copiar
     * @param dst Tablero al que se quiere volcar la copia
     */
    private void copyBoard(Tile[][] src, Tile[][] dst){
        for(int k = 1; k < _size - 1; k++){
            for(int l = 1; l < _size - 1; l++){
                dst[k][l].copyFromTile(src[k][l]);
            }
        }
    }

    /**
     * Al resolver un tablero, todas las fichas azules deben mostrar el número de casillas azules que ven.
     */
    private void setAllDotToValue(){
        Tile currentTile;
        for(int k = 0; k < _size; k++){
            for(int l = 0; l < _size; l++){
                currentTile = _tiles[k][l];
                if(currentTile.getType() == TileType.Dot) _tiles[k][l].setType(TileType.Value);
            }
        }
        calculateValues();
    }

    /**
     * Obtiene la primera ficha del tablero.
     */
    private TileHint getFirstHint(){
        TileHint hint = _tileHint;
        boolean seenEmpties = false;
        for(int auxY = 1; auxY < _size - 1; auxY++){
            for(int auxX = 1; auxX < _size-1; auxX++){
                if(_tiles[auxY][auxX].getType() == TileType.Unknown) seenEmpties = true;
                hint = searchHintInTile(auxX, auxY);
                if(hint.getType() != HintType.None && hint.getType() != HintType.TableroResuelto) return hint;
            }
        }
        if(!seenEmpties) hint.setHint(HintType.TableroResuelto, _nullTile);
        else hint.setHint(HintType.None, _nullTile);
        return hint;
    }

    /**
     * Busca una pista en una ficha determinada
     */
    private TileHint searchHintInTile(int x, int y){
        Tile tileToChange;
        Tile originalTile = _tiles[y][x];
        _tileHint.setOriginalTile(_tiles[y][x]);
        if(originalTile.getType() == TileType.Value){
            //Datos de la casilla
            boolean isExpandable = false;
            int totalNumDotsSeen = 0;
            int totalSpaceAvailable = 0;
            Tile auxTile;

            for(int k = 0; k < 4; k++){ //One per Direction (Up, Down, ...)
                TileDirectionInfo dirInfo = _tileDirInfo[k];
                dirInfo.reset(); //Reseteamos los datos de la clase para evitarnos news
                Direction currentDir = _directions[k];
                //Primero miramos el número de Dots en esta dirección
                auxTile = navigateTile(originalTile, currentDir);
                while(auxTile.getType() == TileType.Dot || auxTile.getType() == TileType.Value){
                    dirInfo.numDotsFilled++;
                    auxTile = navigateTile(auxTile, currentDir);
                }
                //Al terminar de recorrer los dots, miramos los espacios en blanco.
                if(auxTile.getType() == TileType.Unknown){
                    //Si a continuación de los dots hay un espacio en blanco, la ficha puede crecer en esa dirección.
                    isExpandable = true;
                    dirInfo.minGrowthSize++;
                    dirInfo.spaceAvailable++;
                    auxTile = navigateTile(auxTile, currentDir);
                    //Si la casilla siguiente a la vacía es otra vacía, el crecimiento mínimo de esa ficha en esa dirección
                    //es de numDots+1
                    if(auxTile.getType() == TileType.Unknown){
                        dirInfo.spaceAvailable++;
                        auxTile = navigateTile(auxTile, currentDir);
                        while(auxTile.getType() != TileType.Wall){
                            dirInfo.spaceAvailable++;
                            auxTile = navigateTile(auxTile, currentDir);
                        }
                    }
                    //Si en su lugar hay un dot, el crecimiento mínimo de la casilla será mayor. Hay que buscar todos los dots
                    //consecutivos al espacio en blanco.
                    else if(auxTile.getType() == TileType.Dot || auxTile.getType() == TileType.Value){
                        dirInfo.spaceAvailable++;
                        dirInfo.minGrowthSize++;
                        auxTile = navigateTile(auxTile, currentDir);
                        while(auxTile.getType() == TileType.Dot || auxTile.getType() == TileType.Value){
                            dirInfo.minGrowthSize++;
                            dirInfo.spaceAvailable++;
                            auxTile = navigateTile(auxTile, currentDir);
                        }
                        //Tras obtener el número de dots consecutivos, miramos el resto de casillas que no sean un wall para actualizar
                        //spaceAvailable.
                        while(auxTile.getType() != TileType.Wall){
                            dirInfo.spaceAvailable++;
                            auxTile = navigateTile(auxTile, currentDir);
                        }
                    }
                }
                totalNumDotsSeen += dirInfo.numDotsFilled;
                totalSpaceAvailable += dirInfo.spaceAvailable;
                if(totalNumDotsSeen > originalTile.getValue()){ //return HintType.VeDemasiadas;
                    _tileHint.setHint(HintType.VeDemasiadas, originalTile);
                    return _tileHint;
                }
            }
            if(totalNumDotsSeen == originalTile.getValue() && isExpandable) {
                tileToChange = getFirstEmptySeen(originalTile);
                _tileHint.setHint(HintType.VeLasNecesarias, tileToChange);
                return _tileHint;
            }
            else if (totalNumDotsSeen < originalTile.getValue() && !isExpandable){
                _tileHint.setHint(HintType.FichaCerradaEsIncorrecta, originalTile);
                return _tileHint;
            }
            else if (isExpandable && totalSpaceAvailable + totalNumDotsSeen == originalTile.getValue()) {
                tileToChange = getFirstEmptySeen(originalTile);
                _tileHint.setHint(HintType.RellenaTodosLosHuecos, tileToChange);
                return _tileHint;
            }
            for(int k = 0; k < 4; k++){
                int aux = totalNumDotsSeen - _tileDirInfo[k].numDotsFilled;
                if(aux + _tileDirInfo[k].minGrowthSize > originalTile.getValue()){
                    tileToChange = getFirstEmptySeenByDir(originalTile, _directions[k]);
                    _tileHint.setHint(HintType.SiExpandeSuperaValor, tileToChange);
                    return _tileHint;
                }
            }
            for(int k = 0; k < 4; k++){
                if(totalNumDotsSeen + totalSpaceAvailable - _tileDirInfo[k].spaceAvailable < originalTile.getValue()){
                    tileToChange = getFirstEmptySeenByDir(originalTile, _directions[k]);
                    _tileHint.setHint(HintType.AzulObligatorioEnDireccion, tileToChange);
                    return _tileHint;
                }
            }
        }
        else if (originalTile.getType() == TileType.Dot || originalTile.getType() == TileType.Unknown){
            boolean valueTileFound = false;
            for(int k = 0; k < 4 && !valueTileFound; k++){
                Tile currentTile = navigateTile(originalTile, _directions[k]);
                while(currentTile.getType() != TileType.Wall && !valueTileFound){
                    valueTileFound = (currentTile.getType() == TileType.Value);
                    currentTile = navigateTile(currentTile, _directions[k]);
                }
            }
            if(!valueTileFound){
                if(originalTile.getType() == TileType.Dot) {
                    _tileHint.setHint(HintType.AzulEsMuro, originalTile);
                    return _tileHint;
                }
                else {
                    _tileHint.setHint(HintType.VaciaEsMuro, originalTile);
                    return _tileHint;
                }
            }
        }
        _tileHint.setHint(HintType.None, _nullTile);
        return _tileHint;
    }

    /**
     * Resuelve una pista
     */
    private void solveHint(TileHint tileHint){
        switch(tileHint.getType()){
            case VeLasNecesarias:
                _tiles[tileHint.getTileToChange().getY()][tileHint.getTileToChange().getX()].setType(TileType.Wall);
                break;
            case RellenaTodosLosHuecos:
                _tiles[tileHint.getTileToChange().getY()][tileHint.getTileToChange().getX()].setType(TileType.Dot);
                break;
            case AzulObligatorioEnDireccion:
                _tiles[tileHint.getTileToChange().getY()][tileHint.getTileToChange().getX()].setType(TileType.Dot);
                break;
            case SiExpandeSuperaValor:
                _tiles[tileHint.getTileToChange().getY()][tileHint.getTileToChange().getX()].setType(TileType.Wall);
                break;
            case VaciaEsMuro:
                _tiles[tileHint.getTileToChange().getY()][tileHint.getTileToChange().getX()].setType(TileType.Wall);
                break;
        }
    }

    private Tile getFirstEmptySeen(Tile tile){
        Tile auxTile;
        for(int k = 0; k < 4; k++){
            auxTile = getFirstEmptySeenByDir(tile, _directions[k]);
            if(auxTile.getType() == TileType.Unknown) return auxTile;
        }
        return _nullTile;
    }

    private Tile getFirstEmptySeenByDir(Tile tile, Direction dir){
        tile = navigateTile(tile, dir);
        if(tile.getType() == TileType.Unknown) return tile;
        else if(tile.getType() == TileType.Value || tile.getType() == TileType.Dot) return getFirstEmptySeenByDir(tile, dir);
        else return _nullTile;
    }

    private int getNumDotsSeen(Tile tile){
        int numDotsSeen = 0;
        for(int k = 0; k < 4; k++){
            numDotsSeen += getNumDotsSeenAux(tile, _directions[k]);
        }
        return numDotsSeen;
    }

    private int getNumDotsSeenAux(Tile tile, Direction dir){
        tile = navigateTile(tile, dir);
        if(tile.getType() == TileType.Unknown || tile.getType() == TileType.Wall) return 0;
        else return getNumDotsSeenAux(tile, dir) + 1;
    }

    private Tile navigateTile(Tile tile, Direction dir){
        return (_tiles[tile.getY() + dir.getY()][tile.getX() + dir.getX()]);
    }

    private boolean isHintPlayerMistake(HintType type){
        return (type == HintType.AzulEsMuro || type == HintType.FichaCerradaEsIncorrecta || type == HintType.VeDemasiadas);
    }
}