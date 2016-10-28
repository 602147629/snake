using System;
using System.Collections.Generic;

public class FoodItem
{
    public  UInt32 Ids;
    //x
    public  float PosX;
    //y
    public  float PosY ;
    //半径
    public  float Radius ;
    //分数
    public  UInt32 Score;

    public  UInt32 GetId()
    {
        return Ids;
    }

    public  void SetId(UInt32 id)
    {
        Ids = id;
    }

    public  float GetPosX()
    {
        return PosX;
    }

    public  void SetPosX(float posX)
    {
        PosX = posX;
    }

    public  float GetPosY()
    {
        return PosY;
    }

    public  void SetPosY(float posY)
    {
        PosY = posY;
    }
    public float GetRadius()
    {
        return Radius;
    }

    public  void SetRadius(float radius)
    {
        Radius = radius;
    }


    public  UInt32 GetScore()
    {
        return Score;
    }

    public  void SetScore(UInt32 score)
    {
        Score = score;
    }
}
