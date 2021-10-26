public class TileInfo {
    public TileInfo(){

    }

    public int getDotsSeen() {
        return _dotsSeen;
    }

    public boolean seesEmtpies(){
        return _seesEmpties;
    }

    public void setDotsSeen(int dotsSeen){
        _dotsSeen = dotsSeen;
    }

    public void setSeesEmpties(boolean seesEmpties){
        _seesEmpties = seesEmpties;
    }

    private int _dotsSeen = 0;
    private boolean _seesEmpties = false;
}
