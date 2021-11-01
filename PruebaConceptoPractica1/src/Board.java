import java.util.Vector;

enum TileType{
    Unknown, Dot, Wall, Value //Nunca debería ser value. No se cuando lo utiliza
}

enum HintType{
    //Terminadas
    VeDemasiadas, //Ve más puntos azules de los necesarios
    VeLasNecesarias, //Ve las casillas necesarias y hay que cerrarla
    FichaCerradaEsIncorrecta, //Una ficha resuelta ve menos azules de los que debería
    SiExpandeSuperaValor, //Si expande con una casilla azul, supera el valor
    RellenaTodosLosHuecos,
    AzulObligatorioEnDireccion, //En todas las soluciones imaginables hay una dirección en la que hay que poner azules
    //Pendientes 
    VaciaEsMuro, //Si a un espacio vacio no le ve nadie (no tiene a ningun value en x o en y) tiene que ser muro
    AzulEsMuro, //Si a un azul no le ve nadie (igual que la pista anterior) tiene que ser un muro
    //SoloUnaDireccion, //Una ficha solo se puede expandir en una dirección
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
        _empty = new int[][]{{-1,-1,-1,-1,-1,-1,-1,-1},
                             {-1, 1, 0, 0,-1,-1, 0,-1},
                             {-1, 0, 3, 0, 0, 0, 1,-1},
                             {-1,-1, 0, 0, 0, 3, 0,-1},
                             {-1, 4, 0, 5, 0, 0, 0,-1},
                             {-1, 0, 3, 0, 0, 0, 2,-1},
                             {-1, 0, 3, 0, 1, 0, 2,-1},
                             {-1,-1,-1,-1,-1,-1,-1,-1}};
        _tiles = new Tile[_size][_size];
        _tileDirInfo = new DirInfo[4];
        for(int k = 0; k < 4; k++){
            _tileDirInfo[k] = new DirInfo();
        }
        _directions = new Direction[]{new Direction(-1, 0), new Direction(1, 0), new Direction(0, 1), new Direction(0, -1)};
        TileType type;
        for(int k = 0; k < _size; k++){
            for(int l = 0; l < _size; l++){
                if(_empty[k][l] == -1) type = TileType.Wall;
                else if (_empty[k][l] == 0) type = TileType.Unknown;
                else type = TileType.Value;

                _tiles[k][l] = new Tile(l, k, _empty[k][l], type);
            }
        }
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
            else System.out.println("No puedes cambiar una casilla fija bobo\n");
        }
    }

    private void printTile(int x, int y){
        System.out.println("[" + Integer.toString(y - 1) + "," + Integer.toString(x - 1) + "]  Type: " + _tiles[y][x].getType());
    }

    private void printTile(Tile tile){
        printTile(tile.getX(), tile.getY());
    }

    public boolean update(){
        //System.out.println("La casilla ve: " + Integer.toString(getVisibleTiles(_posX, _posY)) + " casillas.");
        HintType hint = HintType.None;
        int auxX=1, auxY=1;
        for(auxY = 1; auxY < _size - 1; auxY++){
            for(auxX = 1; auxX < _size-1; auxX++){
                hint = getHint(auxX, auxY);
                if(hint != HintType.None) break; //Esto es to feo. Hay que cambiarlo
            }
            if(hint != HintType.None) break;
        }
        System.out.println("Tile:[" + Integer.toString(auxY-1) + "," + Integer.toString(auxX-1) + "]");
        switch (hint) {
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

    private HintType getHint(int x, int y){
        Tile originalTile = _tiles[y][x];
        if(originalTile.getType() == TileType.Value){
            boolean isExpandable = false;
            if(originalTile.getX()==1 && originalTile.getY()==4){
                int t = 0;
            }
            int totalNumDotsSeen = 0;
            int totalSpaceAvailable = 0;
            Tile currentTile = originalTile;
            for(int k = 0; k < 4; k++){ //One per Direction (Up, Down, ...)
                DirInfo dirInfo = _tileDirInfo[k];
                dirInfo.reset();
                Direction currentDir = _directions[k];
                //Primero miramos el número de Dots en esta dirección
                currentTile = navigateTile(originalTile, currentDir);
                //printTile(currentTile);
                while(currentTile.getType() == TileType.Dot || currentTile.getType() == TileType.Value){
                    dirInfo.numDotsFilled++;
                    currentTile = navigateTile(currentTile, currentDir);
                }
                //Al terminar de recorrer los dots, recorremos los espacios en blanco. Como podemos habernos topado con 
                //un muro en la primera casilla en esa dirección, hay que comprobarlo.
                if(currentTile.getType() == TileType.Unknown){
                    isExpandable = true;
                    dirInfo.minGrowthSize++;
                    dirInfo.spaceAvailable++;
                    currentTile = navigateTile(currentTile, currentDir);
                    if(currentTile.getType() == TileType.Unknown){
                        dirInfo.spaceAvailable++;
                        currentTile = navigateTile(currentTile, currentDir);
                        while(currentTile.getType() != TileType.Wall){
                            dirInfo.spaceAvailable++;
                            currentTile = navigateTile(currentTile, currentDir);
                        }
                    } 
                    else if(currentTile.getType() == TileType.Dot || currentTile.getType() == TileType.Value){
                        dirInfo.spaceAvailable++;
                        dirInfo.minGrowthSize++;
                        currentTile = navigateTile(currentTile, currentDir);
                        while(currentTile.getType() == TileType.Dot || currentTile.getType() == TileType.Value){
                            dirInfo.minGrowthSize++;
                            dirInfo.spaceAvailable++;
                            currentTile = navigateTile(currentTile, currentDir);
                        }

                        while(currentTile.getType() != TileType.Wall){
                            dirInfo.spaceAvailable++;
                            currentTile = navigateTile(currentTile, currentDir);
                        }
                    }
                }
                totalNumDotsSeen += dirInfo.numDotsFilled;
                totalSpaceAvailable += dirInfo.spaceAvailable;
                if(totalNumDotsSeen > originalTile.getValue()) return HintType.VeDemasiadas;
            }
            if(totalNumDotsSeen == originalTile.getValue() && isExpandable) return HintType.VeLasNecesarias;
            else if (totalNumDotsSeen < originalTile.getValue() && !isExpandable) return HintType.FichaCerradaEsIncorrecta;
            else if (isExpandable && totalSpaceAvailable + totalNumDotsSeen == originalTile.getValue()) {
                return HintType.RellenaTodosLosHuecos;
            }
            for(int k = 0; k < 4; k++){
                int aux = totalNumDotsSeen - _tileDirInfo[k].numDotsFilled;
                if(aux + _tileDirInfo[k].minGrowthSize > originalTile.getValue()) return HintType.SiExpandeSuperaValor;
            }
            for(int k = 0; k < 4; k++){
                if(totalNumDotsSeen + totalSpaceAvailable - _tileDirInfo[k].spaceAvailable < originalTile.getValue()) return HintType.AzulObligatorioEnDireccion;
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
                if(originalTile.getType() == TileType.Dot) return HintType.AzulEsMuro;
                else return HintType.VaciaEsMuro;
            } 
        }
        return HintType.None;
    }

    private Tile navigateTile(Tile tile, Direction dir){
        return (_tiles[tile.getY() + dir.getY()][tile.getX() + dir.getX()]);
    }   

    private int _posX = 1, _posY = 1;
    private int[][] _empty;
    private Tile[][] _tiles;
    private Direction[] _directions;
    private int _size;
    private DirInfo[] _tileDirInfo; 
}