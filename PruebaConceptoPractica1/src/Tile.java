public class Tile{

    public Tile(int x, int y, int value, TileType type){
        _x = x;
        _y = y;
        _value = value;
        _type = type;
    }

    //Getters & Setters

    public void changeType(){
        if(_type == TileType.Unknown) _type = TileType.Dot;
        else if(_type == TileType.Dot) _type = TileType.Wall;
        else if(_type == TileType.Wall) _type = TileType.Dot;
    }

    public TileType getType(){
        return _type;
    }

    public int getValue(){
        return _value;
    }

    private TileType _type = TileType.Value;
    private int _x = 0;
    private int _y = 0;
    private int _value = 0;
}
