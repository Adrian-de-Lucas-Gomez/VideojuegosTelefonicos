public class Board{

    public Board(int size){
        _size = size;
        _full = new int[]{1, 2, -1, 2, -1, 3, 4, 4, 1, -1, 3, 3, 1, -1, 2, -1};
        _empty = new int[]{0, 2, 0, 0, -1, 0, 4, 0, 0, 0, 3, 3, 1, 0, 2, 0};
        _tiles = new Tile[4 * 4];

        TileType type;
        for(int k = 0; k < size * size; k++){
            if(_empty[k] == -1) type = TileType.Wall;
            else if (_empty[k] == 0) type = TileType.Unknown;
            else type = TileType.Value;

            _tiles[k] = new Tile(k % size, k / size + k % size, _empty[k], type);
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
                TileType type = _tiles[k*_size + l].getType();
                
                if(k == _posY && l == _posX) System.out.print("* ");
                else if (type == TileType.Wall) System.out.print("X ");
                else if (type == TileType.Dot) System.out.print("O ");
                else if (type == TileType.Value){
                    System.out.print(_tiles[k*_size + l].getValue());
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

    public void update(char input){
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
            if(_empty[_posY * _size + _posX] == 0){
                _tiles[_posY*_size + _posX].changeType();
            }
            else System.out.println("No puedes cambiar una casilla fija bobo\n");
        }
    }
    private int _posX = 0, _posY = 0;
    private int[] _full;
    private int[] _empty;
    private Tile[] _tiles;
    private int _size;
}