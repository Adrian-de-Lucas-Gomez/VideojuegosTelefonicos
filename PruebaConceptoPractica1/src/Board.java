import java.util.Vector;

enum TileType{
    Unknown, Dot, Wall, Value //Nunca debería ser value. No se cuando lo utiliza
}

enum HintType{
    TooManyVisible, TileSeesAllRequired, None
}

public class Board{

    public Board(int size){
        _size = size;
        _full = new int[][]{{1, 2, -1, 2}, {-1, 3, 4, 4}, {1, -1, 3, 3}, {1, -1, 2, -1}};
        _empty = new int[][]{{0, 2, 0, 0}, {-1, 0, 4, 0}, {0, 0, 3, 3}, {1, 0, 2, 0}};
        _tiles = new Tile[4][4];
        _directions = new Direction[]{new Direction(-1, 0), new Direction(1, 0), new Direction(0, 1), new Direction(0, -1)};
        TileType type;
        for(int k = 0; k < size; k++){
            for(int l = 0; l < size; l++){
                if(_empty[k][l] == -1) type = TileType.Wall;
                else if (_empty[k][l] == 0) type = TileType.Unknown;
                else type = TileType.Value;

                _tiles[k][l] = new Tile(k % size, k / size + k % size, _empty[k][l], type);
            }
        }
    }

    public void paint(){
        for(int k = 0; k < _size; k++){
            //Borde
            for(int l = 0; l < _size; l++){
                System.out.print("+---");
            }
            System.out.print("+\n");
            //Casillas
            for(int l = 0; l < _size; l++){
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
        for(int l = 0; l < _size; l++){
            System.out.print("+---");
        }
        System.out.print("+\n");
    }

    public void handleInput(char input){
        if(input == 'd'){
            _posX = (_posX + 1) % _size; 
        }
        else if (input == 's'){
            _posY =  (_posY + 1) % _size; 
        }
        else if (input == 'a'){
            _posX = java.lang.Math.abs(_posX - 1) % _size; 
        }
        else if (input == 'w'){
            _posY = java.lang.Math.abs(_posY - 1) % _size; 
        }
        else if (input == 'e'){
            if(_empty[_posY][_posX] == 0){
                _tiles[_posY][_posX].changeType();
            }
            else System.out.println("No puedes cambiar una casilla fija bobo\n");
        }
    }

    public boolean update(){
        System.out.println("La casilla ve: " + Integer.toString(getVisibleTiles(_posX, _posY)) + " casillas.");
        HintType hint = getHint(_posX, _posY);
        switch (hint) {
            case TooManyVisible:
                System.out.println("This tile sees too many dots.");
                break;
            case TileSeesAllRequired:
                System.out.println("This tile sees all necessary dots.");
                break;
            default:
                break;
        }
        return false;
    }

    private int getVisibleTiles(int x, int y){
        if(isTileValid(x, y)){
            if (_tiles[y][x].getType() == TileType.Dot || _tiles[y][x].getType() == TileType.Value){
                int visibleTiles = 0; //Se cuenta a ella misma
                for(int k = 0; k < _directions.length; k++){
                    visibleTiles += getVisibleTilesAux(x + _directions[k].getX(), y + _directions[k].getY(), _directions[k]);
                }
                return visibleTiles;
            }
        }
        return 0; //Si es unknown o muro devuelve 0
    }

    private HintType getHint(int x, int y){
        if(_tiles[_posY][_posX].getType() == TileType.Value && getVisibleTiles(_posX, _posY) > _tiles[_posY][_posX].getValue()) return HintType.TooManyVisible;
        

        return HintType.None;
    }

    private int getVisibleTilesAux(int x, int y, Direction dir){
        if(isTileValid(x, y)){
            if (_tiles[y][x].getType() == TileType.Dot || _tiles[y][x].getType() == TileType.Value){
                return 1 + getVisibleTilesAux(x + dir.getX(), y + dir.getY(), dir);
            }
        }
        return 0;
    }

    private boolean isTileValid(int x, int y){
        return(x >= 0 && x < _size && y >= 0 && y < _size);
    }

    // private Direction getDir(Directions dir){
    //     if(dir == Directions.Down) return new Direction(0, 1);
    //     else if(dir == Directions.Up) return new Direction(0, -1);
    //     else if(dir == Directions.Left) return new Direction(-1, 0);
    //     else return new Direction(1, 0); //Right
    // }    

    private int _posX = 0, _posY = 0;
    private int[][] _full;
    private int[][] _empty;
    private Tile[][] _tiles;
    private Direction[] _directions;
    private int _size;
}