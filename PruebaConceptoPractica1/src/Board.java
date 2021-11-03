import java.util.Vector;

enum TileType{
    Unknown, Dot, Wall, Value, None
}

enum HintType{
    VeLasNecesarias, //Ve las casillas necesarias y hay que cerrarla
    SiExpandeSuperaValor, //Si expande con una casilla azul, supera el valor
    RellenaTodosLosHuecos,
    AzulObligatorioEnDireccion, //En todas las soluciones imaginables hay una dirección en la que hay que poner azules
    //SoloUnaDireccion, //Una ficha solo se puede expandir en una dirección

    //Errores del jugador
    VaciaEsMuro, //Si a un espacio vacio no le ve nadie (no tiene a ningun value en x o en y) tiene que ser muro
    AzulEsMuro, //Si a un azul no le ve nadie (igual que la pista anterior) tiene que ser un muro
    FichaCerradaEsIncorrecta, //Una ficha resuelta ve menos azules de los que debería
    VeDemasiadas, //Ve más puntos azules de los necesarios

    None 
}
/*
{{-1,-1,-1,-1,-1,-1},
{-1, 0, 2, 0, 0,-1},
{-1,-1, 0, 4, 0,-1},
{-1, 0, 0, 3, 3,-1},
{-1, 1, 0, 2, 0,-1},
{-1,-1,-1,-1,-1,-1}};
*/
public class Board{

    public Board(int size){
        _size = size+2;
        _empty = new int[][]{{-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                             {-1, 0, 8, 0, 0, 0, 0, 0, 0, 2,-1},
                             {-1, 7, 0, 0, 8, 0, 0, 0, 4, 0,-1},
                             {-1, 0, 0,-1, 0,-1, 0, 0, 0, 4,-1},
                             {-1, 0, 0, 0, 0, 4, 0,-1, 0, 0,-1},
                             {-1, 3, 6, 0,-1, 0, 6, 4, 0, 0,-1},
                             {-1, 0, 0, 8, 0, 0, 0, 0, 0, 0,-1},
                             {-1, 0, 0, 0, 0, 7, 0,-1, 0, 3,-1},
                             {-1, 0, 0, 0, 0, 0, 0, 1, 0, 0,-1},
                             {-1, 2, 3, 0, 2, 0, 0,-1, 3, 0,-1},
                             {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}};
        _tiles = new Tile[_size][_size];
        _tileHint = new TileHint();
        _tileDirInfo = new TileDirectionInfo[4];
        _nullTile = new Tile(0, 0, 0, TileType.None);
        for(int k = 0; k < 4; k++){
            _tileDirInfo[k] = new TileDirectionInfo();
        }
        _directions = new Direction[]{new Direction(-1, 0), new Direction(1, 0), new Direction(0, 1), new Direction(0, -1)};
        TileType type;
        for(int k = 0; k < _size; k++){
            for(int l = 0; l < _size; l++){
                if(_empty[k][l] == -1) type = TileType.Wall;
                else if (_empty[k][l] == 0) type = TileType.Unknown;
                else type = TileType.Value;

                _tiles[k][l] = new Tile(l, k, _empty[k][l], type);
                _solution[k][l] = new Tile(l, k, _empty[k][l], type);
            }
        }
    }

    public void generate(){
        
    }

    public void paint(){
        for(int k = 1; k < _size-1; k++){
            //Borde
            for(int l = 1; l < _size-1; l++){
                System.out.print("+---");
            }
            System.out.print("+\n");
            //Casillas
            for(int l = 1; l < _size-1; l++){
                System.out.print("| ");
                TileType type = _tiles[k][l].getType();
                
                if(k == _posY && l == _posX) System.out.print("* ");
                else if (type == TileType.Wall) System.out.print("X ");
                else if (type == TileType.Dot) System.out.print("O ");
                else if (type == TileType.Value){
                    System.out.print(_tiles[k][l].getValue());
                    System.out.print(" ");
                }
                else { //Type.Unknown
                    System.out.print("  ");
                }
            }
            System.out.print("|\n");
        }
        //Borde
        for(int l = 1; l < _size-1; l++){
            System.out.print("+---");
        }
        System.out.print("+\n");
    }

    public void handleInput(char input){
        if(input == 'd'){
            _posX = (_posX + 1) % (_size -1);
            if(_posX == 0) _posX = 1;
        }
        else if (input == 's'){
            _posY =  (_posY + 1) % (_size -1); 
            if(_posY == 0) _posY = 1;
        }
        else if (input == 'a'){
            _posX = java.lang.Math.abs(_posX - 1) % (_size -1);
            if(_posX == 0) _posX = _size-2;
        }
        else if (input == 'w'){
            _posY = java.lang.Math.abs(_posY - 1) % (_size -1);
            if(_posY == 0) _posY = _size-2;
        } 
        else if (input == 'e'){
            if(_empty[_posY][_posX] == 0){
                _tiles[_posY][_posX].toggleType();
            }
        }
    }

    public boolean solveBoard(){ //Returns false if it can't be solved.
        _tileHint = getFirstHint();
        while(!isHintPlayerMistake(_tileHint.getType())){
            if(_tileHint.getType() == HintType.None) return true;
            System.out.println(" ");
            processHint(_tileHint);
            _tileHint = getFirstHint();
        }
        return false;
    }

    public boolean update(){
        //System.out.println("La casilla ve: " + Integer.toString(getVisibleTiles(_posX, _posY)) + " casillas.");
        TileHint hint = _tileHint;
        
        hint = getFirstHint();
        System.out.println("Tile:[" + Integer.toString(hint.getOriginalTile().getY() - 1) + "," + Integer.toString(hint.getOriginalTile().getX() - 1) + "]");
        switch (hint.getType()) {
            case VeDemasiadas:
                System.out.println("This tile sees too many dots.");
                break;
            case VeLasNecesarias:
                System.out.println("This tile sees all necessary dots.");
                break;
            case RellenaTodosLosHuecos:
                System.out.println("El numero de casillas que le faltan a rellenar es el de casillas libres.");
                break;
            case FichaCerradaEsIncorrecta:
                System.out.println("Has cerrado esta casilla pero no ve los azules necesarios.");
                break;
            case AzulObligatorioEnDireccion:
                System.out.println("En todas las soluciones posibles de esta casilla hay azules obligatorios.");
                break;
            case SiExpandeSuperaValor:
                System.out.println("Si se pone un punto azul supera el valor de la casilla.");
                break;
            case AzulEsMuro:
                System.out.println("Ninguna casilla ve a esta ficha azul");
                break;
            case VaciaEsMuro:
                System.out.println("Ninguna casilla ve a esta ficha vacia");
                break;
            default:
                System.out.println("Completado!!!");
                break;
        }
        return false;
    }

    private TileHint getFirstHint(){
        TileHint hint = _tileHint;
        int auxX=1, auxY=1;
        for(auxY = 1; auxY < _size - 1; auxY++){
            for(auxX = 1; auxX < _size-1; auxX++){
                hint = getHint(auxX, auxY);
                if(hint.getType() != HintType.None) return hint;
            }
        }
        hint.setHint(HintType.None, _nullTile);
        return hint;
    }

    private TileHint getHint(int x, int y){
        Tile tileToChange = _tiles[y][x];
        Tile originalTile = _tiles[y][x];
        _tileHint.setOriginalTile(_tiles[y][x]);
        if(originalTile.getType() == TileType.Value){
            //Datos de la casilla
            boolean isExpandable = false;
            int totalNumDotsSeen = 0;
            int totalSpaceAvailable = 0;
            Tile auxTile = originalTile;

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
            Boolean valueTileFound = false;
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

    private void processHint(TileHint tileHint){
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
        default:

            break;
        }
    }

    private Tile getFirstEmptySeen(Tile tile){
        Tile auxTile = tile;
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

    private Tile navigateTile(Tile tile, Direction dir){
        return (_tiles[tile.getY() + dir.getY()][tile.getX() + dir.getX()]);
    }

    private boolean isHintPlayerMistake(HintType type){
        return (type == HintType.AzulEsMuro || type == HintType.FichaCerradaEsIncorrecta || type == HintType.VeDemasiadas);
    }

    private int _posX = 1, _posY = 1;
    private int[][] _empty;
    private Tile[][] _tiles;
    private Tile[][] _solution;
    private Tile _nullTile;
    private Direction[] _directions;
    private TileHint _tileHint;
    private int _size;
    private TileDirectionInfo[] _tileDirInfo;
}