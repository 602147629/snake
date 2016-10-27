using System;
using System.Collections.Generic;

public class FoodItem
{
    public  Int16 Ids;
    //x
    public  float PosX;
    //y
    public  float PosY ;
    //半径
    public  float Radius ;
    //分数
    public  Int16 Score;

    public  Int16 GetId()
    {
        return Ids;
    }

    public  void SetId(Int16 id)
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


    public  Int16 GetScore()
    {
        return Score;
    }

    public  void SetScore(Int16 score)
    {
        Score = score;
    }
}
