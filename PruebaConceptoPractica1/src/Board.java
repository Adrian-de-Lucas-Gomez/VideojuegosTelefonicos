import java.util.Vector;

enum TileType{
    Unknown, Dot, Wall, Value //Nunca debería ser value. No se cuando lo utiliza
}

enum HintType{
    TooManyDotsVisible, TileSeesAllRequired, //Terminadas
    EmptyIsWall, DotIsWall, TileMarksAllEmpties, ClosedOffTileIsIncorrect, //Pendientes
    None 
}

public class Board{

    public Board(int size){
        _size = size+2;
        _full = new int[][]{{-1, -1, -1, -1, -1, -1}, {-1, 1, 2, -1, 2, -1}, {-1, -1, 3, 4, 4, -1}, {-1, 1, -1, 3, 3, -1}, {-1, 1, -1, 2, -1, -1}, {-1, -1, -1, -1, -1, -1}};
        _empty = new int[][]{{-1, -1, -1, -1, -1, -1}, {-1, 0, 2, 0, 0, -1}, {-1, -1, 0, 4, 0, -1}, {-1, 0, 0, 3, 3, -1}, {-1, 1, 0, 2, 0, -1}, {-1, -1, -1, -1, -1, -1}};
        _tiles = new Tile[_size][_size];
        _directions = new Direction[]{new Direction(-1, 0), new Direction(1, 0), new Direction(0, 1), new Direction(0, -1)};
        TileType type;
        for(int k = 0; k < _size; k++){
            for(int l = 0; l < _size; l++){
                if(_empty[k][l] == -1) type = TileType.Wall;
                else if (_empty[k][l] == 0) type = TileType.Unknown;
                else type = TileType.Value;

                _tiles[k][l] = new Tile(k % _size, k / _size + k % _size, _empty[k][l], type);
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
                _tiles[_posY][_posX].changeType();
            }
            else System.out.println("No puedes cambiar una casilla fija bobo\n");
        }
    }

    public boolean update(){
        //System.out.println("La casilla ve: " + Integer.toString(getVisibleTiles(_posX, _posY)) + " casillas.");
        HintType hint = HintType.None;
        int auxX=1, auxY=1;
        for(auxY = 1; auxY < _size - 1; auxY++){
            for(auxX = 1; auxX < _size-1; auxX++){
                hint = getHint(auxX, auxY);
                if(hint != HintType.None) break;
            }
            if(hint != HintType.None) break;
        }
        System.out.println("Tile:[" + Integer.toString(auxY-1) + "," + Integer.toString(auxX-1) + "]");
        switch (hint) {
            case TooManyDotsVisible:
                System.out.println("This tile sees too many dots.");
                break;
            case TileSeesAllRequired:
                System.out.println("This tile sees all necessary dots.");
                break;
            default:
                System.out.println("No hint found");
                break;
        }
        return false;
    }

    private HintType getHint(int x, int y){
        TileInfo info = gatherTileInfo(x, y);

        if(_tiles[y][x].getType() == TileType.Value && info.getDotsSeen() > _tiles[y][x].getValue()) return HintType.TooManyDotsVisible;

        if(info.getDotsSeen() == _tiles[y][x].getValue() && info.seesEmtpies()) return HintType.TileSeesAllRequired;

        //if(info.getDotsSeen() == _tiles[y][x].getValue() && !info.seesEmtpies()) return HintType.Correct; //DEBUG. Dice si un tile es correcto pero no fiarse.

        return HintType.None;
    }

    TileInfo gatherTileInfo(int x, int y){
        TileInfo info = new TileInfo(false);

        info.setDotsSeen(getVisibleTiles(x, y, info));

        return info;
    }

    private int getVisibleTiles(int x, int y, TileInfo info){
        if (_tiles[y][x].getType() == TileType.Dot || _tiles[y][x].getType() == TileType.Value){
            int visibleTiles = 0; //Se cuenta a ella misma
            for(int k = 0; k < _directions.length; k++){
                visibleTiles += getVisibleTilesAux(x + _directions[k].getX(), y + _directions[k].getY(), _directions[k], info);
            }
            return visibleTiles;
        }
        
        return 0; //Si es unknown o muro devuelve 0
    }

    private int getVisibleTilesAux(int x, int y, Direction dir, TileInfo info){
        if (_tiles[y][x].getType() == TileType.Dot || _tiles[y][x].getType() == TileType.Value){
            return 1 + getVisibleTilesAux(x + dir.getX(), y + dir.getY(), dir, info);
        }
        else if(_tiles[y][x].getType() == TileType.Unknown) info.setSeesEmpties(true);
        
        return 0;
    }

    // private boolean isTileValid(int x, int y){
    //     return(x >= 0 && x < _size && y >= 0 && y < _size);
    // }

    // private Direction getDir(Directions dir){
    //     if(dir == Directions.Down) return new Direction(0, 1);
    //     else if(dir == Directions.Up) return new Direction(0, -1);
    //     else if(dir == Directions.Left) return new Direction(-1, 0);
    //     else return new Direction(1, 0); //Right
    // }    

    private int _posX = 1, _posY = 1;
    private int[][] _full;
    private int[][] _empty;
    private Tile[][] _tiles;
    private Direction[] _directions;
    private int _size;
}