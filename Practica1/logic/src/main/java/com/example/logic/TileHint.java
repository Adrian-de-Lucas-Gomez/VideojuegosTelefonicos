package com.example.logic;

import javax.lang.model.util.Elements.Origin;

public class TileHint {
    TileHint(){
        _hintTile = new Tile(0, 0, 0, TileType.Unknown);
    }
    public void setHint(HintType hint, Tile hintTile) {
        _hint = hint;
        _hintTile = hintTile;
    }

    public void setOriginalTile(Tile tile){_originalTile = tile;}

    public HintType getType(){return _hint;}
    public Tile getTileToChange(){return _hintTile;}
    public Tile getOriginalTile(){return _originalTile;}

    private HintType _hint = HintType.None;
    private Tile _hintTile;
    private Tile _originalTile;
}
