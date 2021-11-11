package com.example.logic;

public class TileDirectionInfo {
    public TileDirectionInfo(){}

    public int numDotsFilled = 0;
    public int spaceAvailable = 0;
    public int minGrowthSize = 0;
    // public boolean expandable = false;

    public void reset(){
        numDotsFilled = 0;
        spaceAvailable = 0;
        minGrowthSize = 0;
        //expandable = false;
    }
}
