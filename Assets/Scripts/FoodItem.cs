using System;
using System.Collections.Generic;

public class FoodItem
{
    public  Int32 Ids;
    //x
    public  float PosX;
    //y
    public  float PosY ;
    //半径
    public  float Radius ;
    //分数
    public  Int32 Score;

    public  Int32 GetId()
    {
        return Ids;
    }

    public  void SetId(Int32 id)
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


    public  Int32 GetScore()
    {
        return Score;
    }

    public  void SetScore(Int32 score)
    {
        Score = score;
    }
}
